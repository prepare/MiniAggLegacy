﻿//2014 BSD,WinterDev

//MatterHackers: BSD
// Much of the ui to the drawing functions still needs to be C#'ed and cleaned up.  A lot of
// it still follows the originall agg function names.  I have been cleaning these up over time
// and intend to do much more refactoring of these things over the long term.

using System;
using PixelFarm.Agg.Fonts;
using Mini;
namespace PixelFarm.Agg.SimplePainter
{
    [Info(OrderCode = "01")]
    [Info("SimplePainterGlyph")]
    public class SimplePainterGlyphSample : DemoBase
    {
        string fontfile = "c:\\Windows\\Fonts\\tahoma.ttf";
        PixelFarm.Agg.Fonts.Font font1;
        PixelFarm.Agg.Fonts.Font font2;
        public override void Init()
        {
            //load font ?
            font1 = NativeFontStore.LoadFont(fontfile, 72);
            font2 = NativeFontStore.LoadFont(fontfile, 10);
        }
        public override void Draw(CanvasPainter p)
        { //1.
            // clear the image to white 
            p.Clear(ColorRGBA.White);
            p.FillColor = new ColorRGBA(ColorRGBA.Blue, 80);
            //M414 -20q-163 0 -245 86t-82 236z
            //<path d="M414 20q-163 40 -245 126t-82 276z"/> 
            //PathWriter path = new PathWriter();
            //path.MoveTo(414, 20);
            //path.Curve3Rel(-163, 40, -245, 126);
            //path.SmoothCurve3Rel(-82, 246);
            //path.CloseFigure();

            //M414 -20q-163 0 -245 86t-82 236z
            //PathWriter path = new PathWriter();
            //path.MoveTo(414, -20);
            //path.Curve3Rel(-163, 0, -245, 86);
            //path.SmoothCurve3Rel(-82, 236);
            //path.CloseFigure(); 

            //p.Fill(p.FlattenCurves(path.Vxs));

            //p.DrawBezierCurve(120, 500 - 160, 220, 500 - 40, 35, 500 - 200, 220, 500 - 260);
            //--------------------------------------------------- 
            var fontGlyph = font1.GetGlyph('{');
            //outline version
            var flat_v = p.FlattenCurves(fontGlyph.flattenVxs);
            p.Fill(flat_v);
            //bitmap version

            p.DrawImage(fontGlyph.glyphImage32, 20, 30);
            p.CurrentFont = font1;
            p.FillColor = ColorRGBA.Black;
            // string test_str = "มีมี่ญูดุญคำค่าค่ำป่บ่";
            //string test_str = "abcde";
            string test_str = "บ่ป่มีมี่";
            p.UseSubPixelRendering = true;
            p.DrawString(test_str, 5, 200);
            //p.DrawString("12345", 50, 200); 
            p.StrokeColor = ColorRGBA.Black;
            p.Line(0, 200, 800, 200);
            p.FillColor = ColorRGBA.Black;
            p.CurrentFont = font2;
            p.DrawString(test_str, 80, 100);
            p.UseSubPixelRendering = false;
            //--------------------------------------------------- 
            //p.Fill(fontGlyph.vxs);
#if DEBUG
            //p.Fill(fontGlyph.vxs); 


            // dbugVxsDrawPoints.DrawVxsPoints(flat_v, g);
            //dbugVxsDrawPoints.DrawVxsPoints(fontGlyph.vxs, g); 
#endif

            //p.Fill(p.FlattenCurves(fontGlyph.vxs));


            //StyledTypeFace typeFaceForLargeA = new StyledTypeFace(LiberationSansFont.Instance, 300, flatenCurves: false);
            //var m_pathVxs = typeFaceForLargeA.GetGlyphForCharacter('a');
            //Affine shape_mtx = Affine.NewMatix(AffinePlan.Translate(150, 50));
            //m_pathVxs = shape_mtx.TransformToVxs(m_pathVxs);

            ////p.FillColor = new ColorRGBA(ColorRGBA.Red, 100);
            ////p.Fill(m_pathVxs);
            //p.FillColor = new ColorRGBA(ColorRGBA.Green, 120);
            //p.Fill(p.FlattenCurves(m_pathVxs));
        }
    }
}