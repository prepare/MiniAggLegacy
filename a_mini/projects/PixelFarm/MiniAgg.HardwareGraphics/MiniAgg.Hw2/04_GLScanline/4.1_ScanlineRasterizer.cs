//2014 BSD,WinterDev   
//----------------------------------------------------------------------------
// Anti-Grain Geometry - Version 2.4
// Copyright (C) 2002-2005 Maxim Shemanarev (http://www.antigrain.com)
//
// C# Port port by: Lars Brubaker
//                  larsbrubaker@gmail.com
// Copyright (C) 2007
//
// Permission to copy, use, modify, sell and distribute this software 
// is granted provided this copyright notice appears in all copies. 
// This software is provided "as is" without express or implied
// warranty, and with no claim as to its suitability for any purpose.
//
//----------------------------------------------------------------------------
//
// The author gratefully acknowleges the support of David Turner, 
// Robert Wilhelm, and Werner Lemberg - the authors of the FreeType 
// libray - in producing this work. See http://www.freetype.org for details.
//
//----------------------------------------------------------------------------
// Contact: mcseem@antigrain.com
//          mcseemagg@yahoo.com
//          http://www.antigrain.com
//----------------------------------------------------------------------------
//
// Adaptation for 32-bit screen coordinates has been sponsored by 
// Liberty Technology Systems, Inc., visit http://lib-sys.com
//
// Liberty Technology Systems, Inc. is the provider of
// PostScript and PDF technology for software developers.
// 
//----------------------------------------------------------------------------

using poly_subpix = PixelFarm.Agg.AggBasics.PolySubPix;
namespace PixelFarm.Agg
{
    //==================================================rasterizer_scanline_aa
    // Polygon rasterizer that is used to render filled polygons with 
    // high-quality Anti-Aliasing. Internally, by default, the class uses 
    // integer coordinates in format 24.8, i.e. 24 bits for integer part 
    // and 8 bits for fractional - see poly_subpixel_shift. This class can be 
    // used in the following  way:
    //
    // 1. filling_rule(filling_rule_e ft) - optional.
    //
    // 2. gamma() - optional.
    //
    // 3. reset()
    //
    // 4. move_to(x, y) / line_to(x, y) - make the polygon. One can create 
    //    more than one contour, but each contour must consist of at least 3
    //    vertices, i.e. move_to(x1, y1); line_to(x2, y2); line_to(x3, y3);
    //    is the absolute minimum of vertices that define a triangle.
    //    The algorithm does not check either the number of vertices nor
    //    coincidence of their coordinates, but in the worst case it just 
    //    won't draw anything.
    //    The order of the vertices (clockwise or counterclockwise) 
    //    is important when using the non-zero filling rule (fill_non_zero).
    //    In this case the vertex order of all the contours must be the same
    //    if you want your intersecting polygons to be without "holes".
    //    You actually can use different vertices order. If the contours do not 
    //    intersect each other the order is not important anyway. If they do, 
    //    contours with the same vertex order will be rendered without "holes" 
    //    while the intersecting contours with different orders will have "holes".
    //
    // filling_rule() and gamma() can be called anytime before "sweeping".
    //------------------------------------------------------------------------

    public sealed partial class GLScanlineRasterizer
    {
        CellAARasterizer m_cellAARas;
        VectorClipper m_vectorClipper;
        int[] m_gammaLut = new int[AA_SCALE];
        FillingRule m_filling_rule;
        bool m_auto_close;
        /// <summary>
        /// multiplied move to start x
        /// </summary>
        int mul_start_x;
        /// <summary>
        /// multiplied move to starty
        /// </summary>
        int mul_start_y;
        Status m_status;
        int m_scan_y;
        //---------------------------
        const int AA_SHIFT = 8;
        const int AA_SCALE = 1 << AA_SHIFT;
        const int AA_MASK = AA_SCALE - 1;
        const int AA_SCALE2 = AA_SCALE * 2;
        const int AA_MASK2 = AA_SCALE2 - 1;
        //---------------------------

        RectInt userModeClipBox;
        //---------------
        //offset x offset y
        double addVertextOffsetX = 0;
        double addVertextOffsetY = 0;
        enum Status
        {
            Initial,
            MoveTo,
            LineTo,
            Closed
        }

        public GLScanlineRasterizer()
        {
            m_cellAARas = new CellAARasterizer();
            m_vectorClipper = new VectorClipper(m_cellAARas);
            m_filling_rule = FillingRule.NonZero;
            m_auto_close = true;
            mul_start_x = 0;
            mul_start_y = 0;
            m_status = Status.Initial;
            for (int i = AA_SCALE - 1; i >= 0; --i)
            {
                m_gammaLut[i] = i;
            }
        }

        //--------------------------------------------------------------------
        public void Reset()
        {
            m_cellAARas.Reset();
            m_status = Status.Initial;
        }
        public RectInt GetVectorClipBox()
        {
            return userModeClipBox;
        }
        //--------------------------

        internal void SetClipBox(RectInt clippingRect)
        {
            SetClipBox(clippingRect.Left, clippingRect.Bottom, clippingRect.Right, clippingRect.Top);
        }
        public void SetClipBox(int x1, int y1, int x2, int y2)
        {
            userModeClipBox = new RectInt(x1, y1, x2, y2);
            Reset();
            m_vectorClipper.SetClipBox(
                                upscale(x1), upscale(y1),
                                upscale(x2), upscale(y2));
        }
        //---------------------------------
        //from vector clipper
        static int upscale(double v)
        {
            return AggBasics.iround(v * poly_subpix.SCALE);
        }
        static int upscale(int v)
        {
            return v << poly_subpix.SHIFT;
            //return v * poly_subpix.SCALE; 
        }
        ////from vector clipper
        //static int downscale(int v)
        //{
        //    return v / (int)poly_subpix.SCALE;
        //}
        //---------------------------------
        FillingRule ScanlineFillingRule
        {
            get { return this.m_filling_rule; }
            set { this.m_filling_rule = value; }
        }
        bool AutoClose
        {
            get { return m_auto_close; }
            set { this.m_auto_close = value; }
        }
        //--------------------------------------------------------------------
        public void ResetGamma(IGammaFunction gamma_function)
        {
            for (int i = AA_SCALE - 1; i >= 0; --i)
            {
                m_gammaLut[i] = (int)AggBasics.uround(
                    gamma_function.GetGamma((float)(i) / AA_MASK) * AA_MASK);
            }
        }

        //------------------------------------------------------------------------
        public void MoveTo(double x, double y)
        {
            if (m_cellAARas.Sorted) { Reset(); }
            if (m_auto_close) { ClosePolygon(); }

            m_vectorClipper.MoveTo(
                mul_start_x = upscale(x),
                mul_start_y = upscale(y));
            m_status = Status.MoveTo;
        }
        //------------------------------------------------------------------------
        public void LineTo(double x, double y)
        {
            m_vectorClipper.LineTo(upscale(x), upscale(y));
            m_status = Status.LineTo;
        }

        void ClosePolygon()
        {
            if (m_status == Status.LineTo)
            {
                m_vectorClipper.LineTo(mul_start_x, mul_start_y);
                m_status = Status.Closed;
            }
        }

        public double AddVertexOffsetX
        {
            get { return this.addVertextOffsetX; }
            set { this.addVertextOffsetX = value; }
        }
        public double AddVertexOffsetY
        {
            get { return this.addVertextOffsetY; }
            set { this.addVertextOffsetY = value; }
        }
        //void AddVertex(ShapePath.FlagsAndCommand cmd, double x, double y)
        //{
        //    switch (cmd)
        //    {
        //        case ShapePath.FlagsAndCommand.CommandMoveTo:
        //            {
        //                MoveTo(x, y);
        //            } break;
        //        case ShapePath.FlagsAndCommand.CommandLineTo:
        //            {
        //                LineTo(x, y);
        //            } break;
        //        case ShapePath.FlagsAndCommand.CommandCurve3:
        //            {
        //                //generate curve here
        //            } break;
        //        case ShapePath.FlagsAndCommand.CommandCurve4:
        //            {
        //                //generate curve here
        //                LineTo(x, y);
        //            } break;
        //        default:
        //            {
        //                if (ShapePath.IsClose(cmd))
        //                {
        //                    ClosePolygon();
        //                }
        //            } break;
        //    }
        //}
        //------------------------------------------------------------------------
        void Edge(double x1, double y1, double x2, double y2)
        {
            if (m_cellAARas.Sorted) { Reset(); }
            m_vectorClipper.MoveTo(upscale(x1), upscale(y1));
            m_vectorClipper.LineTo(upscale(x2), upscale(y2));
            m_status = Status.MoveTo;
        }
        //-------------------------------------------------------------------

        public void AddPath(VertexStore vxs)
        {
            this.AddPath(new VertexStoreSnap(vxs));
        }
        public void AddPath(VertexStoreSnap snap)
        {
            double x = 0;
            double y = 0;
            if (m_cellAARas.Sorted) { Reset(); }
            //--------------------------------------------
            if (snap.VxsHasMoreThanOnePart)
            {
                var vxs = snap.GetInternalVxs();
                int j = vxs.Count;
                for (int i = 0; i < j; ++i)
                {
                    var cmd = vxs.GetVertex(i, out x, out y);
                    switch (cmd)
                    {
                        case VertexCmd.Stop:
                            {
                                //stop 
                            }
                            break;
                        case VertexCmd.MoveTo:
                            {
                                MoveTo(x, y);
                            }
                            break;
                        case VertexCmd.LineTo:
                        case VertexCmd.P2c:
                        case VertexCmd.P3c:
                            {
                                //curve must be flatten before using here
                                LineTo(x, y);
                            }
                            break;
                        default:
                            {
                                if (VertexHelper.IsClose(cmd))
                                {
                                    ClosePolygon();
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                var snapIter = snap.GetVertexSnapIter();
                VertexCmd cmd;
                bool stop = false;
                while (!stop)
                {
                    cmd = snapIter.GetNextVertex(out x, out y);
                    switch (cmd)
                    {
                        case VertexCmd.Stop:
                            {
                                stop = true;
                            }
                            break;
                        case VertexCmd.MoveTo:
                            {
                                MoveTo(x, y);
                            }
                            break;
                        case VertexCmd.LineTo:
                        case VertexCmd.P2c:
                        case VertexCmd.P3c:
                            {
                                LineTo(x, y);
                            }
                            break;
                        default:
                            {
                                if (VertexHelper.IsClose(cmd))
                                {
                                    ClosePolygon();
                                }
                            }
                            break;
                    }
                }
            }
        }

        public int MinX { get { return m_cellAARas.MinX; } }
        public int MinY { get { return m_cellAARas.MinY; } }
        public int MaxX { get { return m_cellAARas.MaxX; } }
        public int MaxY { get { return m_cellAARas.MaxY; } }


        //--------------------------------------------------------------------
        void Sort()
        {
            if (m_auto_close) { ClosePolygon(); }

            m_cellAARas.SortCells();
        }

        //------------------------------------------------------------------------
        internal bool RewindScanlines()
        {
            if (m_auto_close) { ClosePolygon(); }

            m_cellAARas.SortCells();
            if (m_cellAARas.TotalCells == 0) return false;
            m_scan_y = m_cellAARas.MinY;
            return true;
        }


        //--------------------------------------------------------------------
        int CalculateAlpha(int area)
        {
            int cover = area >> (poly_subpix.SHIFT * 2 + 1 - AA_SHIFT);
            if (cover < 0)
            {
                cover = -cover;
            }

            if (m_filling_rule == FillingRule.EvenOdd)
            {
                cover &= AA_SCALE2;
                if (cover > AA_SCALE)
                {
                    cover = AA_SCALE2 - cover;
                }
            }

            if (cover > AA_MASK)
            {
                cover = AA_MASK;
            }

            return m_gammaLut[cover];
        }

        //--------------------------------------------------------------------
        internal bool SweepScanline(GLScanline scline)
        {
            for (;;)
            {
                if (m_scan_y > m_cellAARas.MaxY)
                {
                    return false;
                }

                scline.ResetSpans();
                CellAA[] cells;
                int offset;
                int num_cells;
                m_cellAARas.GetCells(m_scan_y, out cells, out offset, out num_cells);
                int cover = 0;
                while (num_cells != 0)
                {
                    unsafe
                    {
                        fixed (CellAA* cur_cell_h = &cells[0])
                        {
                            CellAA* cur_cell_ptr = cur_cell_h + offset;
                            int alpha;
                            int x = cur_cell_ptr->x;
                            int area = cur_cell_ptr->area;
                            cover += cur_cell_ptr->cover;
                            //accumulate all cells with the same X
                            while (--num_cells != 0)
                            {
                                offset++; //move next
                                cur_cell_ptr++; //move next
                                if (cur_cell_ptr->x != x)
                                {
                                    break;
                                }

                                area += cur_cell_ptr->area;
                                cover += cur_cell_ptr->cover;
                            }

                            if (area != 0)
                            {
                                alpha = CalculateAlpha((cover << (poly_subpix.SHIFT + 1)) - area);
                                if (alpha != 0)
                                {
                                    scline.AddCell(x, alpha);
                                }
                                x++;
                            }

                            if ((num_cells != 0) && (cur_cell_ptr->x > x))
                            {
                                alpha = CalculateAlpha(cover << (poly_subpix.SHIFT + 1));
                                if (alpha != 0)
                                {
                                    scline.AddSpan(x, (cur_cell_ptr->x - x), alpha);
                                }
                            }
                        }
                    }
                }

                if (scline.SpanCount != 0) { break; }

                ++m_scan_y;
            }

            scline.CloseLine(m_scan_y);
            ++m_scan_y;
            return true;
        }
    }
}

