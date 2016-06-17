//2014,2015 BSD,WinterDev   
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
// Contact: mcseem@antigrain.com
//          mcseemagg@yahoo.com
//          http://www.antigrain.com
//----------------------------------------------------------------------------


namespace PixelFarm.Agg.Transform
{
    //================================================span_interpolator_linear
    public sealed class SpanInterpolatorLinear : ISpanInterpolator
    {
        Transform.ICoordTransformer m_trans;
        LineInterpolatorDDA2 m_li_x;
        LineInterpolatorDDA2 m_li_y;
        const int SUB_PIXEL_SHIFT = 8;
        const int SUB_PIXEL_SCALE = 1 << SUB_PIXEL_SHIFT;
        public SpanInterpolatorLinear(Transform.ICoordTransformer trans)
        {
            m_trans = trans;
        }

        public Transform.ICoordTransformer Transformer
        {
            get { return this.m_trans; }
            set { this.m_trans = value; }
        }
        public void GetLocalScale(out int x, out int y)
        {
            throw new System.NotImplementedException();
        }

        //----------------------------------------------------------------
        public void Begin(double x, double y, int len)
        {
            double tx;
            double ty;
            tx = x;
            ty = y;
            m_trans.Transform(ref tx, ref ty);
            int x1 = AggBasics.iround(tx * (double)SUB_PIXEL_SCALE);
            int y1 = AggBasics.iround(ty * (double)SUB_PIXEL_SCALE);
            tx = x + len;
            ty = y;
            m_trans.Transform(ref tx, ref ty);
            int x2 = AggBasics.iround(tx * (double)SUB_PIXEL_SCALE);
            int y2 = AggBasics.iround(ty * (double)SUB_PIXEL_SCALE);
            m_li_x = new LineInterpolatorDDA2(x1, x2, (int)len);
            m_li_y = new LineInterpolatorDDA2(y1, y2, (int)len);
        }

        //----------------------------------------------------------------
        public void ReSync(double xe, double ye, int len)
        {
            m_trans.Transform(ref xe, ref ye);
            m_li_x = new LineInterpolatorDDA2(m_li_x.Y, AggBasics.iround(xe * (double)SUB_PIXEL_SCALE), (int)len);
            m_li_y = new LineInterpolatorDDA2(m_li_y.Y, AggBasics.iround(ye * (double)SUB_PIXEL_SCALE), (int)len);
        }


        public void Next()
        {
            m_li_x.Next();
            m_li_y.Next();
        }

        //----------------------------------------------------------------
        public void GetCoord(out int x, out int y)
        {
            x = m_li_x.Y;
            y = m_li_y.Y;
        }
    }
}