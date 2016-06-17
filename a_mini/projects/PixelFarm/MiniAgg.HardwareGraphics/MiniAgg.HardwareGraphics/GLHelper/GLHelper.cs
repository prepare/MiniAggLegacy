﻿// 2015,2014 ,MIT, WinterDev

using PixelFarm.Drawing;
namespace OpenTK.Graphics.OpenGL
{
    public static class GLHelper
    {
        public static void ClearColor(Color c)
        {
            GL.ClearColor(
                (float)c.R / 255f,
                (float)c.G / 255f,
                (float)c.B / 255f,
                (float)c.A / 255f);
        }
        public static void Color3(Color c)
        {
            GL.Color3(
                 (float)c.R / 255f,
                 (float)c.G / 255f,
                 (float)c.B / 255f);
        }
        public static PixelFarm.Drawing.Rectangle ConvToRect(Rectangle openTkRect)
        {
            return new PixelFarm.Drawing.Rectangle(
                openTkRect.X,
                openTkRect.Y,
                openTkRect.Width,
                openTkRect.Height);
        }
    }
}
