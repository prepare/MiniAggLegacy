﻿//2014 BSD,WinterDev

//MatterHackers: BSD
// Much of the ui to the drawing functions still needs to be C#'ed and cleaned up.  A lot of
// it still follows the originall agg function names.  I have been cleaning these up over time
// and intend to do much more refactoring of these things over the long term.

using System;
using PixelFarm.Agg.VertexSource;
using PixelFarm.VectorMath;
using PixelFarm.Agg.Transform;
using PixelFarm.Agg.Fonts;
using Mini;
namespace PixelFarm.Agg.Sample_Draw
{
    [Info(OrderCode = "01")]
    [Info("from MatterHackers' Agg DrawAndSave")]
    public class DrawSample01 : DemoBase
    {
        public override void Init()
        {
        }

        public override void Draw(CanvasPainter p)
        {
            p.Clear(ColorRGBA.White);
            //g.UseSubPixelRendering = true; 
            string teststr = "ABCDE abcd 1230 Hello!";
            p.DrawString(teststr, 300, 400);
            //g.UseSubPixelRendering = false;
            //g.DrawString(teststr, 300, 422, 22);
        }
    }


    [Info(OrderCode = "01")]
    [Info("from MatterHackers' Agg DrawAndSave")]
    public class DrawSample02 : DemoBase
    {
        public override void Init()
        {
        }
        public override void Draw(CanvasPainter p)
        {
            int width = 800;
            int height = 600;
            //clear the image to white
            //g.Clear(ColorRGBA.White);
            // draw a circle
            p.Clear(ColorRGBA.White);
            Ellipse ellipsePro = new Ellipse(0, 0, 100, 50);
            for (double angleDegrees = 0; angleDegrees < 180; angleDegrees += 22.5)
            {
                var mat = Affine.NewMatix(
                    AffinePlan.Rotate(MathHelper.DegreesToRadians(angleDegrees)),
                    AffinePlan.Translate(width / 2, 150));
                VertexStore sp1 = mat.TransformToVxs(ellipsePro.MakeVxs());
                p.FillColor = ColorRGBA.Yellow;
                p.Fill(sp1);
                //g.Render(sp1, ColorRGBA.Yellow);
                //Stroke ellipseOutline = new Stroke(sp1, 3);
                p.FillColor = ColorRGBA.Blue;
                p.Fill(StrokeHelp.MakeVxs(sp1, 3));
                //g.Render(StrokeHelp.MakeVxs(sp1, 3), ColorRGBA.Blue);
            }

            // and a little polygon
            PathWriter littlePoly = new PathWriter();
            littlePoly.MoveTo(50, 50);
            littlePoly.LineTo(150, 50);
            littlePoly.LineTo(200, 200);
            littlePoly.LineTo(50, 150);
            littlePoly.LineTo(50, 50);
            p.FillColor = ColorRGBA.Cyan;
            p.Fill(littlePoly.MakeVertexSnap());
            //g.Render(littlePoly.MakeVertexSnap(), ColorRGBA.Cyan);
            // draw some text
            // draw some text  

            var textPrinter = new TextPrinter();
            textPrinter.CurrentFont = SvgFontStore.LoadFont(SvgFontStore.DEFAULT_SVG_FONTNAME, 30);
            //new TypeFacePrinter("Printing from a printer", 30, justification: Justification.Center);

            VertexStore vxs = textPrinter.CreateVxs("Printing from a printer".ToCharArray());
            var affTx = Affine.NewTranslation(width / 2, height / 4 * 3);
            VertexStore s1 = affTx.TransformToVxs(vxs);
            p.FillColor = ColorRGBA.Black;
            p.Fill(s1);
            //g.Render(s1, ColorRGBA.Black);
            p.FillColor = ColorRGBA.Red;
            p.Fill(StrokeHelp.MakeVxs(s1, 1));
            //g.Render(StrokeHelp.MakeVxs(s1, 1), ColorRGBA.Red);
            var aff2 = Affine.NewMatix(
                AffinePlan.Rotate(MathHelper.DegreesToRadians(90)),
                AffinePlan.Translate(40, height / 2));
            p.FillColor = ColorRGBA.Black;
            p.Fill(aff2.TransformToVertexSnap(vxs));
            //g.Render(aff2.TransformToVertexSnap(vxs), ColorRGBA.Black);
        }
    }

    //public class SimpleDrawAndSave
    //{
    //    [STAThread]
    //    public static void Main(string[] args)
    //    {
    //        // first we will show how to use the simple drawing functions in graphics 2D
    //        {
    //            ImageBuffer simpleImage = new ImageBuffer(640, 480, 32, new BlenderBGRA());
    //            Graphics2D simpleImageGraphics2D = simpleImage.NewGraphics2D();
    //            // clear the image to white
    //            simpleImageGraphics2D.Clear(RGBA_Bytes.White);
    //            // draw a circle
    //            simpleImageGraphics2D.Circle(50, 50, 30, RGBA_Bytes.Blue);
    //            // draw a line
    //            simpleImageGraphics2D.Line(10, 100, 520, 50, new RGBA_Bytes(20, 200, 200));
    //            // draw a filled box
    //            simpleImageGraphics2D.FillRectangle(60, 260, 200, 280, RGBA_Bytes.Yellow);
    //            // and an outline around it
    //            simpleImageGraphics2D.Rectangle(60, 260, 200, 280, RGBA_Bytes.Magenta);
    //            // draw some text
    //            simpleImageGraphics2D.DrawString("A Simple Example", 300, 400, 20);

    //            // and save this image out
    //            ImageTgaIO.Save(simpleImage, "SimpleDrawAndSave.tga");
    //        }

    //        // now we will we will show how to use the render function to draw more complex things
    //        {
    //            ImageBuffer lessSimpleImage = new ImageBuffer(640, 480, 32, new BlenderBGRA());
    //            Graphics2D lessSimpleImageGraphics2D = lessSimpleImage.NewGraphics2D();
    //            // clear the image to white
    //            lessSimpleImageGraphics2D.Clear(RGBA_Bytes.White);
    //            // draw a circle
    //            Ellipse ellipseTest = new Ellipse(0, 0, 100, 50);
    //            for (double angleDegrees = 0; angleDegrees < 180; angleDegrees += 22.5)
    //            {
    //                VertexSourceApplyTransform rotatedTransform = new VertexSourceApplyTransform(ellipseTest, Affine.NewRotation(MathHelper.DegreesToRadians(angleDegrees)));
    //                VertexSourceApplyTransform rotatedAndTranslatedTransform = new VertexSourceApplyTransform(rotatedTransform, Affine.NewTranslation(lessSimpleImage.Width / 2, 150));
    //                lessSimpleImageGraphics2D.Render(rotatedAndTranslatedTransform, RGBA_Bytes.Yellow);
    //                Stroke ellipseOutline = new Stroke(rotatedAndTranslatedTransform, 3);
    //                lessSimpleImageGraphics2D.Render(ellipseOutline, RGBA_Bytes.Blue);
    //            }

    //            // and a little polygon
    //            PathStorage littlePoly = new PathStorage();
    //            littlePoly.MoveTo(50, 50);
    //            littlePoly.LineTo(150, 50);
    //            littlePoly.LineTo(200, 200);
    //            littlePoly.LineTo(50, 150);
    //            littlePoly.LineTo(50, 50);
    //            lessSimpleImageGraphics2D.Render(littlePoly, RGBA_Bytes.Cyan);

    //            // draw some text
    //            TypeFacePrinter textPrinter = new TypeFacePrinter("Printing from a printer", 30, justification: Justification.Center);
    //            IVertexSource translatedText = new VertexSourceApplyTransform(textPrinter, Affine.NewTranslation(new Vector2(lessSimpleImage.Width / 2, lessSimpleImage.Height / 4 * 3)));
    //            lessSimpleImageGraphics2D.Render(translatedText, RGBA_Bytes.Red);
    //            Stroke strokedText = new Stroke(translatedText);
    //            lessSimpleImageGraphics2D.Render(strokedText, RGBA_Bytes.Black);

    //            IVertexSource rotatedText = new VertexSourceApplyTransform(textPrinter, Affine.NewRotation(MathHelper.DegreesToRadians(90)));
    //            IVertexSource rotatedTranslatedText = new VertexSourceApplyTransform(rotatedText, Affine.NewTranslation(new Vector2(40, lessSimpleImage.Height / 2)));
    //            lessSimpleImageGraphics2D.Render(rotatedTranslatedText, RGBA_Bytes.Black);

    //            // and save this image out
    //            ImageTgaIO.Save(lessSimpleImage, "LessSimpleDrawAndSave.tga");
    //        }
    //    }
    //}
}
