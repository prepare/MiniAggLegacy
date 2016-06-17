﻿//2014,2015 BSD,WinterDev   

using System;
using System.Collections.Generic;
using System.Text;
namespace PixelFarm.Agg
{
    /// <summary>
    /// vertex command and flags
    /// </summary>

    public enum VertexCmd : byte
    {
        //---------------------------------
        //the order of these fields are significant!
        //---------------------------------
        //first lower 4 bits compact flags
        Stop = 0x00,
        HasMore = 0x01, //has more vertex
        //-----------------------
        //end figure command 2 lower bits 
        //is end command when 2 lower bit > HasMore
        EndFigure = 0x02,
        CloseAndEndFigure = 0x03,
        //----------------------- 
        //start from move to is 
        MoveTo = 0x04,
        LineTo = 0x05,
        P2c = 0x06, // 2nd p for Curve3,Curve4 
        P3c = 0x07, // 3rd p for Curve4 
    }
    public enum EndVertexOrientation
    {
        Unknown, //0
        CCW,//1
        CW//2
    }

    public static class VertexHelper
    {
        public static bool IsVertextCommand(VertexCmd c)
        {
            return c >= VertexCmd.MoveTo;
        }
        public static bool IsEmpty(VertexCmd c)
        {
            return c == VertexCmd.Stop;
        }
        public static bool IsMoveTo(VertexCmd c)
        {
            return c == VertexCmd.MoveTo;
        }
        public static bool IsEndFigure(VertexCmd c)
        {
            return ((int)c & 0x3) > (int)VertexCmd.EndFigure;
        }
        public static bool IsClose(VertexCmd c)
        {
            return c == VertexCmd.CloseAndEndFigure;
        }
        public static bool IsNextPoly(VertexCmd c)
        {
            //?
            return c <= VertexCmd.MoveTo;
        }

        public static void ShortenPath(VertexDistanceList vertexDistanceList, double s, bool closed)
        {
            if (s > 0.0 && vertexDistanceList.Count > 1)
            {
                double d;
                int n = (int)(vertexDistanceList.Count - 2);
                while (n != 0)
                {
                    d = vertexDistanceList[n].dist;
                    if (d > s) break;
                    vertexDistanceList.RemoveLast();
                    s -= d;
                    --n;
                }
                if (vertexDistanceList.Count < 2)
                {
                    vertexDistanceList.Clear();
                }
                else
                {
                    n = (int)vertexDistanceList.Count - 1;
                    VertexDistance prev = vertexDistanceList[n - 1];
                    VertexDistance last = vertexDistanceList[n];
                    d = (prev.dist - s) / prev.dist;
                    double x = prev.x + (last.x - prev.x) * d;
                    double y = prev.y + (last.y - prev.y) * d;
                    last.x = x;
                    last.y = y;
                    if (!prev.IsEqual(last))
                    {
                        vertexDistanceList.RemoveLast();
                    }
                    vertexDistanceList.Close(closed);
                }
            }
        }
        public static void ArrangeOrientationsAll(VertexStore myvxs, bool closewise)
        {
            int start = 0;
            while (start < myvxs.Count)
            {
                start = ArrangeOrientations(myvxs, start, closewise);
            }
        }
        //---------------------------------------------------------------- 
        // Arrange the orientation of a polygon, all polygons in a path, 
        // or in all paths. After calling arrange_orientations() or 
        // arrange_orientations_all_paths(), all the polygons will have 
        // the same orientation, i.e. path_flags_cw or path_flags_ccw
        //--------------------------------------------------------------------
        static int ArrangePolygonOrientation(VertexStore myvxs, int start, bool clockwise)
        {
            //if (orientation == ShapePath.FlagsAndCommand.FlagNone) return start;

            // Skip all non-vertices at the beginning
            //ShapePath.FlagsAndCommand orientFlags = clockwise ? ShapePath.FlagsAndCommand.FlagCW : ShapePath.FlagsAndCommand.FlagCCW;

            int vcount = myvxs.Count;
            while (start < vcount &&
                  !VertexHelper.IsVertextCommand(myvxs.GetCommand(start)))
            {
                ++start;
            }

            // Skip all insignificant move_to
            while (start + 1 < vcount &&
                  VertexHelper.IsMoveTo(myvxs.GetCommand(start)) &&
                  VertexHelper.IsMoveTo(myvxs.GetCommand(start + 1)))
            {
                ++start;
            }

            // Find the last vertex
            int end = start + 1;
            while (end < vcount && !VertexHelper.IsNextPoly(myvxs.GetCommand(end)))
            {
                ++end;
            }
            if (end - start > 2)
            {
                bool isCW;
                if ((isCW = IsCW(myvxs, start, end)) != clockwise)
                {
                    // Invert polygon, set orientation flag, and skip all end_poly
                    InvertPolygon(myvxs, start, end);
                    VertexCmd flags;
                    int myvxs_count = myvxs.Count;
                    var orientFlags = isCW ? (int)EndVertexOrientation.CW : (int)EndVertexOrientation.CCW;
                    while (end < myvxs_count &&
                          VertexHelper.IsEndFigure(flags = myvxs.GetCommand(end)))
                    {
                        myvxs.ReplaceVertex(end++, orientFlags, 0);
                        //myvxs.ReplaceCommand(end++, flags | orientFlags);// Path.set_orientation(cmd, orientation));
                    }
                }
            }
            return end;
        }

        static int ArrangeOrientations(VertexStore myvxs, int start, bool closewise)
        {
            while (start < myvxs.Count)
            {
                start = ArrangePolygonOrientation(myvxs, start, closewise);
                if (VertexHelper.IsEmpty(myvxs.GetCommand(start)))
                {
                    ++start;
                    break;
                }
            }

            return start;
        }
        static bool IsCW(VertexStore myvxs, int start, int end)
        {
            // Calculate signed area (double area to be exact)
            //---------------------
            int np = end - start;
            double area = 0.0;
            int i;
            for (i = 0; i < np; i++)
            {
                double x1, y1, x2, y2;
                myvxs.GetVertexXY(start + i, out x1, out y1);
                myvxs.GetVertexXY(start + (i + 1) % np, out x2, out y2);
                area += x1 * y2 - y1 * x2;
            }
            return (area < 0.0);
            //return (area < 0.0) ? ShapePath.FlagsAndCommand.FlagCW : ShapePath.FlagsAndCommand.FlagCCW;
        }
        //--------------------------------------------------------------------
        public static void InvertPolygon(VertexStore myvxs, int start)
        {
            // Skip all non-vertices at the beginning
            int vcount = myvxs.Count;
            while (start < vcount &&
                  !VertexHelper.IsVertextCommand(myvxs.GetCommand(start)))
            { ++start; }

            // Skip all insignificant move_to
            while (start + 1 < vcount &&
                  VertexHelper.IsMoveTo(myvxs.GetCommand(start)) &&
                  VertexHelper.IsMoveTo(myvxs.GetCommand(start + 1)))
            { ++start; }

            // Find the last vertex
            int end = start + 1;
            while (end < vcount && !VertexHelper.IsNextPoly(myvxs.GetCommand(end))) { ++end; }

            InvertPolygon(myvxs, start, end);
        }



        static void InvertPolygon(VertexStore myvxs, int start, int end)
        {
            int i;
            VertexCmd tmp_PathAndFlags = myvxs.GetCommand(start);
            --end; // Make "end" inclusive 
            // Shift all commands to one position
            for (i = start; i < end; i++)
            {
                myvxs.ReplaceCommand(i, myvxs.GetCommand(i + 1));
            }

            // Assign starting command to the ending command
            myvxs.ReplaceCommand(end, tmp_PathAndFlags);
            // Reverse the polygon
            while (end > start)
            {
                myvxs.SwapVertices(start++, end--);
            }
        }
        public static void FlipX(VertexStore vxs, double x1, double x2)
        {
            int i;
            double x, y;
            int count = vxs.Count;
            for (i = 0; i < count; ++i)
            {
                VertexCmd flags = vxs.GetVertex(i, out x, out y);
                if (VertexHelper.IsVertextCommand(flags))
                {
                    vxs.ReplaceVertex(i, x2 - x + x1, y);
                }
            }
        }

        public static void FlipY(VertexStore vxs, double y1, double y2)
        {
            int i;
            double x, y;
            int count = vxs.Count;
            for (i = 0; i < count; ++i)
            {
                VertexCmd flags = vxs.GetVertex(i, out x, out y);
                if (VertexHelper.IsVertextCommand(flags))
                {
                    vxs.ReplaceVertex(i, x, y2 - y + y1);
                }
            }
        }
    }
}
