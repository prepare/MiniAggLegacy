﻿using System;
using PixelFarm.Agg.Transform;
namespace PixelFarm.Agg
{
    public class LionFillSprite : BasicSprite
    {
        SpriteShape lionShape;
        VertexStore myvxs;
        byte alpha;
        public LionFillSprite()
        {
            lionShape = new SpriteShape();
            lionShape.ParseLion();
            this.Width = 500;
            this.Height = 500;
            AlphaValue = 255;
        }
        public byte AlphaValue
        {
            get { return this.alpha; }
            set
            {
                this.alpha = value;
                //change alpha value
                int j = lionShape.NumPaths;
                var colorBuffer = lionShape.Colors;
                for (int i = lionShape.NumPaths - 1; i >= 0; --i)
                {
                    colorBuffer[i].alpha = alpha;
                }
            }
        }

        public override bool Move(int mouseX, int mouseY)
        {
            bool result = base.Move(mouseX, mouseY);
            myvxs = null;
            return result;
        }
        public void Draw(PixelFarm.DrawingGL.CanvasGL2d canvas)
        {
            if (myvxs == null)
            {
                var transform = Affine.NewMatix(
                        AffinePlan.Translate(-lionShape.Center.x, -lionShape.Center.y),
                        AffinePlan.Scale(spriteScale, spriteScale),
                        AffinePlan.Rotate(angle + Math.PI),
                        AffinePlan.Skew(skewX / 1000.0, skewY / 1000.0),
                        AffinePlan.Translate(Width / 2, Height / 2)
                );
                myvxs = transform.TransformToVxs(lionShape.Path.Vxs);
            }
            //---------------------------------------------------------------------------------------------
            {
                int j = lionShape.NumPaths;
                int[] pathList = lionShape.PathIndexList;
                ColorRGBA[] colors = lionShape.Colors;
                //graphics2D.UseSubPixelRendering = true;

                for (int i = 0; i < j; ++i)
                {
                    var color = colors[i];
                    //? 
                    canvas.FillVxsSnap(
                        new PixelFarm.Drawing.Color(color.alpha, color.blue, color.green, color.red),
                        new VertexStoreSnap(myvxs, pathList[i]));
                }
            }
        }
        public override void OnDraw(Graphics2D graphics2D)
        {
            if (myvxs == null)
            {
                var transform = Affine.NewMatix(
                        AffinePlan.Translate(-lionShape.Center.x, -lionShape.Center.y),
                        AffinePlan.Scale(spriteScale, spriteScale),
                        AffinePlan.Rotate(angle + Math.PI),
                        AffinePlan.Skew(skewX / 1000.0, skewY / 1000.0),
                        AffinePlan.Translate(Width / 2, Height / 2)
                );
                myvxs = transform.TransformToVxs(lionShape.Path.Vxs);
            }
            //---------------------------------------------------------------------------------------------
            {
                int j = lionShape.NumPaths;
                int[] pathList = lionShape.PathIndexList;
                ColorRGBA[] colors = lionShape.Colors;
                //graphics2D.UseSubPixelRendering = true;

                for (int i = 0; i < j; ++i)
                {
                    graphics2D.Render(new VertexStoreSnap(myvxs, pathList[i]), colors[i]);
                }
            }
            //---------------------------------------------------------------------------------------------


        }
    }
}