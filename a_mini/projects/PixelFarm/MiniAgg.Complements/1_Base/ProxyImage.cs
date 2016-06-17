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

namespace PixelFarm.Agg.Image
{
    public abstract class ProxyImage : IImageReaderWriter
    {
        protected IImageReaderWriter linkedImage;
        public ProxyImage(IImageReaderWriter linkedImage)
        {
            this.linkedImage = linkedImage;
        }

        public virtual int Width
        {
            get
            {
                return linkedImage.Width;
            }
        }

        public virtual int Height
        {
            get
            {
                return linkedImage.Height;
            }
        }

        public virtual int Stride
        {
            get { return linkedImage.Stride; }
        }


        public virtual RectInt GetBounds()
        {
            return linkedImage.GetBounds();
        }

        public IPixelBlender GetRecieveBlender()
        {
            return linkedImage.GetRecieveBlender();
        }

        public void SetRecieveBlender(IPixelBlender value)
        {
            linkedImage.SetRecieveBlender(value);
        }

        public virtual ColorRGBA GetPixel(int x, int y)
        {
            return linkedImage.GetPixel(x, y);
        }

        public virtual void CopyFrom(IImageReaderWriter sourceImage, RectInt sourceImageRect, int destXOffset, int destYOffset)
        {
            linkedImage.CopyFrom(sourceImage, sourceImageRect, destXOffset, destYOffset);
        }

        public virtual void SetPixel(int x, int y, ColorRGBA color)
        {
            linkedImage.SetPixel(x, y, color);
        }

        //public virtual void BlendPixel(int x, int y, ColorRGBA sourceColor, byte cover)
        //{
        //    linkedImage.BlendPixel(x, y, sourceColor, cover);
        //}

        public virtual void CopyHL(int x, int y, int len, ColorRGBA sourceColor)
        {
            linkedImage.CopyHL(x, y, len, sourceColor);
        }

        public virtual void CopyVL(int x, int y, int len, ColorRGBA sourceColor)
        {
            linkedImage.CopyVL(x, y, len, sourceColor);
        }

        public virtual void BlendHL(int x1, int y, int x2, ColorRGBA sourceColor, byte cover)
        {
            linkedImage.BlendHL(x1, y, x2, sourceColor, cover);
        }

        public virtual void BlendVL(int x, int y1, int y2, ColorRGBA sourceColor, byte cover)
        {
            linkedImage.BlendVL(x, y1, y2, sourceColor, cover);
        }

        public virtual void BlendSolidHSpan(int x, int y, int len, ColorRGBA c, byte[] covers, int coversIndex)
        {
            linkedImage.BlendSolidHSpan(x, y, len, c, covers, coversIndex);
        }

        public virtual void CopyColorHSpan(int x, int y, int len, ColorRGBA[] colors, int colorIndex)
        {
            linkedImage.CopyColorHSpan(x, y, len, colors, colorIndex);
        }

        public virtual void CopyColorVSpan(int x, int y, int len, ColorRGBA[] colors, int colorIndex)
        {
            linkedImage.CopyColorVSpan(x, y, len, colors, colorIndex);
        }

        public virtual void BlendSolidVSpan(int x, int y, int len, ColorRGBA c, byte[] covers, int coversIndex)
        {
            linkedImage.BlendSolidVSpan(x, y, len, c, covers, coversIndex);
        }

        public virtual void BlendColorHSpan(int x, int y, int len, ColorRGBA[] colors, int colorsIndex, byte[] covers, int coversIndex, bool firstCoverForAll)
        {
            linkedImage.BlendColorHSpan(x, y, len, colors, colorsIndex, covers, coversIndex, firstCoverForAll);
        }

        public virtual void BlendColorVSpan(int x, int y, int len, ColorRGBA[] colors, int colorsIndex, byte[] covers, int coversIndex, bool firstCoverForAll)
        {
            linkedImage.BlendColorVSpan(x, y, len, colors, colorsIndex, covers, coversIndex, firstCoverForAll);
        }

        public byte[] GetBuffer()
        {
            return linkedImage.GetBuffer();
        }

        public int GetBufferOffsetXY(int x, int y)
        {
            return linkedImage.GetBufferOffsetXY(x, y);
        }



        public virtual int BytesBetweenPixelsInclusive
        {
            get { return linkedImage.BytesBetweenPixelsInclusive; }
        }

        public virtual int BitDepth
        {
            get
            {
                return linkedImage.BitDepth;
            }
        }
    }
}
