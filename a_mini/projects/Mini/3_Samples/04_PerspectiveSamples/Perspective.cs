﻿//2014 BSD,WinterDev
//MatterHackers

using System;
using PixelFarm.Agg.Transform;
using PixelFarm.Agg.Image;
using PixelFarm.Agg.VertexSource;
using Mini;
namespace PixelFarm.Agg.Sample_Perspective
{
    [Info(OrderCode = "04")]
    [Info("Perspective and bilinear transformations. In general, these classes can transform an arbitrary quadrangle "
            + " to another arbitrary quadrangle (with some restrictions). The example demonstrates how to transform "
            + "a rectangle to a quadrangle defined by 4 vertices. You can drag the 4 corners of the quadrangle, "
            + "as well as its boundaries. Note, that the perspective transformations don't work correctly if "
            + "the destination quadrangle is concave. Bilinear thansformations give a different result, but "
            + "remain valid with any shape of the destination quadrangle.")]
    public class perspective_application : DemoBase
    {
        UI.PolygonEditWidget quadPolygonControl;
        private SpriteShape lionShape;
        public perspective_application()
        {
            lionShape = new SpriteShape();
            lionShape.ParseLion();
            quadPolygonControl = new PixelFarm.Agg.UI.PolygonEditWidget(4, 5.0);
            quadPolygonControl.SetXN(0, lionShape.Bounds.Left);
            quadPolygonControl.SetYN(0, lionShape.Bounds.Top);
            quadPolygonControl.SetXN(1, lionShape.Bounds.Right);
            quadPolygonControl.SetYN(1, lionShape.Bounds.Top);
            quadPolygonControl.SetXN(2, lionShape.Bounds.Right);
            quadPolygonControl.SetYN(2, lionShape.Bounds.Bottom);
            quadPolygonControl.SetXN(3, lionShape.Bounds.Left);
            quadPolygonControl.SetYN(3, lionShape.Bounds.Bottom);
        }
        public override void Init()
        {
            OnInitialize();
            base.Init();
        }

        [DemoConfig]
        public PerspectiveTransformType PerspectiveTransformType
        {
            get;
            set;
        }
        public void OnInitialize()
        {
            double dx = Width / 2.0 - (quadPolygonControl.GetXN(1) - quadPolygonControl.GetXN(0)) / 2.0;
            double dy = Height / 2.0 - (quadPolygonControl.GetYN(0) - quadPolygonControl.GetYN(2)) / 2.0;
            quadPolygonControl.AddXN(0, dx);
            quadPolygonControl.AddYN(0, dy);
            quadPolygonControl.AddXN(1, dx);
            quadPolygonControl.AddYN(1, dy);
            quadPolygonControl.AddXN(2, dx);
            quadPolygonControl.AddYN(2, dy);
            quadPolygonControl.AddXN(3, dx);
            quadPolygonControl.AddYN(3, dy);
        }
        bool didInit = false;
        public override void Draw(CanvasPainter p)
        {
            CanvasPainter painter = p;
            if (!didInit)
            {
                didInit = true;
                OnInitialize();
            }


            //-----------------------------------
            painter.Clear(ColorRGBA.White);
            //IImageReaderWriter backBuffer = ImageHelper.CreateChildImage(gx.DestImage, gx.GetClippingRect());
            //ChildImage image;
            //if (backBuffer.BitDepth == 32)
            //{
            //    image = new ChildImage(backBuffer, new PixelBlenderBGRA());
            //}
            //else
            //{
            //    if (backBuffer.BitDepth != 24)
            //    {
            //        throw new System.NotSupportedException();
            //    }
            //    image = new ChildImage(backBuffer, new PixelBlenderBGR());
            //}
            //ClipProxyImage dest = new ClipProxyImage(image);
            //gx.Clear(ColorRGBA.White);
            //gx.SetClippingRect(new RectInt(0, 0, Width, Height)); 
            //ScanlineRasToDestBitmapRenderer sclineRasToBmp = gx.ScanlineRasToDestBitmap;

            if (this.PerspectiveTransformType == Sample_Perspective.PerspectiveTransformType.Bilinear)
            {
                var bound = lionShape.Bounds;
                Bilinear txBilinear = Bilinear.RectToQuad(bound.Left,
                    bound.Bottom,
                    bound.Right,
                    bound.Top,
                    quadPolygonControl.GetInnerCoords());
                if (txBilinear.IsValid)
                {
                    painter.PaintSeries(txBilinear.TransformToVxs(lionShape.Path.Vxs),
                        lionShape.Colors,
                        lionShape.PathIndexList,
                        lionShape.NumPaths);
                    RectD lionBound = lionShape.Bounds;
                    Ellipse ell = new Ellipse((lionBound.Left + lionBound.Right) * 0.5,
                                     (lionBound.Bottom + lionBound.Top) * 0.5,
                                     (lionBound.Right - lionBound.Left) * 0.5,
                                     (lionBound.Top - lionBound.Bottom) * 0.5,
                                     200);
                    VertexStore trans_ell = txBilinear.TransformToVxs(ell.MakeVxs());
                    painter.FillColor = ColorRGBA.Make(0.5f, 0.3f, 0.0f, 0.3f);
                    painter.Fill(trans_ell);
                    //-------------------------------------------------------------
                    //outline
                    double prevStrokeWidth = painter.StrokeWidth;
                    painter.StrokeWidth = 3;
                    painter.StrokeColor = ColorRGBA.Make(0.0f, 0.3f, 0.2f, 1.0f);
                    painter.Draw(trans_ell);
                    painter.StrokeWidth = prevStrokeWidth;
                }
            }
            else
            {
                var txPerspective = new Perspective(
                    lionShape.Bounds,
                    quadPolygonControl.GetInnerCoords());
                if (txPerspective.IsValid)
                {
                    painter.PaintSeries(txPerspective.TransformToVxs(lionShape.Path.Vxs),
                      lionShape.Colors,
                      lionShape.PathIndexList,
                      lionShape.NumPaths);
                    //--------------------------------------------------------------------------------------
                    //filled Ellipse
                    //1. create original fill ellipse
                    RectD lionBound = lionShape.Bounds;
                    var filledEllipse = new Ellipse((lionBound.Left + lionBound.Right) * 0.5,
                                      (lionBound.Bottom + lionBound.Top) * 0.5,
                                      (lionBound.Right - lionBound.Left) * 0.5,
                                      (lionBound.Top - lionBound.Bottom) * 0.5,
                                      200);
                    VertexStore transformedEll = txPerspective.TransformToVxs(filledEllipse.MakeVxs());
                    painter.FillColor = ColorRGBA.Make(0.5f, 0.3f, 0.0f, 0.3f);
                    painter.Fill(transformedEll);
                    //-------------------------------------------------------- 
                    var prevStrokeW = painter.StrokeWidth;
                    painter.StrokeWidth = 3;
                    painter.StrokeColor = ColorRGBA.Make(0.0f, 0.3f, 0.2f, 1.0f);
                    painter.Draw(transformedEll);
                    painter.StrokeWidth = prevStrokeW;
                }
            }

            //--------------------------
            // Render the "quad" tool and controls
            painter.FillColor = ColorRGBA.Make(0f, 0.3f, 0.5f, 0.6f);
            painter.Fill(quadPolygonControl.MakeVxs());
        }

        public override void MouseDown(int x, int y, bool isRightButton)
        {
            var mouseEvent = new UI.MouseEventArgs(UI.MouseButtons.Left, 1, x, y, 0);
            quadPolygonControl.OnMouseDown(mouseEvent);
        }
        public override void MouseDrag(int x, int y)
        {
            var mouseEvent = new UI.MouseEventArgs(UI.MouseButtons.Left, 1, x, y, 0);
            quadPolygonControl.OnMouseMove(mouseEvent);
            base.MouseDrag(x, y);
        }
        public override void MouseUp(int x, int y)
        {
            var mouseEvent = new UI.MouseEventArgs(UI.MouseButtons.Left, 1, x, y, 0);
            quadPolygonControl.OnMouseUp(mouseEvent);
            base.MouseUp(x, y);
        }
    }


    public enum PerspectiveTransformType
    {
        Bilinear,
        Perspective
    }
}

