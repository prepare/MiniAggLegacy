﻿//BSD 2014, WinterDev
//MattersHackers
//AGG 2.4


namespace PixelFarm.Agg
{
    public class Square
    {
        double m_size;
        public Square(double size)
        {
            m_size = size;
        }

        public void Draw(
            ScanlineRasToDestBitmapRenderer sclineRasToBmp,
            ScanlineRasterizer ras,
            Scanline sl,
            IImageReaderWriter destImage, ColorRGBA color,
            double x, double y)
        {
            ras.Reset();
            ras.MoveTo(x * m_size, y * m_size);
            ras.LineTo(x * m_size + m_size, y * m_size);
            ras.LineTo(x * m_size + m_size, y * m_size + m_size);
            ras.LineTo(x * m_size, y * m_size + m_size);
            sclineRasToBmp.RenderWithColor(destImage, ras, sl, color);
        }
    }
}