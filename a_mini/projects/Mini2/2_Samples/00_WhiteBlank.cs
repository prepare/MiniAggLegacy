﻿using PixelFarm.DrawingGL;
namespace Mini2
{
    [Info(OrderCode = "00")]
    [Info("Drawing")]
    public class WhiteBlankDemo : DemoBase
    {
        public override void Load()
        {
            //draw 1
            FormTestWinGLControl form = new FormTestWinGLControl();
            CanvasGL2d canvas = new CanvasGL2d(this.Width, this.Height);
            form.SetGLPaintHandler((o, s) =>
            {
                canvas.Clear(PixelFarm.Drawing.Color.White);
            });
            form.Show();
        }
    }
}