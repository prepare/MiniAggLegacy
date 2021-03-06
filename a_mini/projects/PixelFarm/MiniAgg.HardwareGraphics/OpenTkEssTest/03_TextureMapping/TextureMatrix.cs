﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Mini;
namespace OpenTkEssTest
{
    //[Example("Texture Matrix Wormhole", ExampleCategory.OpenGL, "1.x", Documentation = "TextureMatrix")]
    [Info(OrderCode = "02")]
    [Info("Texture Matrix Wormhole")]
    public class T05_TextureMatrix : DemoBase
    {
        public override void Init()
        {
            using (var example = new TextureMatrix())
            {
                example.Run(30.0, 0.0);
            }
        }
    }
    class TextureMatrix : GameWindow
    {
        public TextureMatrix()
            : base(800, 600, new GraphicsMode(32, 16, 0, 4))
        {
            VSync = VSyncMode.On;
        }

        int Texture;
        int list;
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0f, 0f, 0f, 0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            Texture = LoadTexture("../../Data/Textures/logo-dark.jpg");
            GL.Enable(EnableCap.Texture2D);
            list = GL.GenLists(1);
            GL.NewList(list, ListMode.Compile);
            {
                const int slices = 32;
                const float distance = 0.25f;
                GL.Begin(BeginMode.Quads);
                for (float scale = 0.26f; scale < 5f; scale += distance)
                    for (int i = 0; i < slices; i++)
                    {
                        Vector3 MiddleCenter = new Vector3((float)(Math.Sin((double)i / slices * 2 * Math.PI) * scale),
                                                           (float)(Math.Cos((double)i / slices * 2 * Math.PI) * scale),
                                                           (float)(1f / scale));
                        Vector3 MiddleRight = new Vector3((float)(Math.Sin((double)(i + 1) / slices * 2 * Math.PI) * scale),
                                                          (float)(Math.Cos((double)(i + 1) / slices * 2 * Math.PI) * scale),
                                                          (float)(1f / scale));
                        Vector3 BottomRight = new Vector3((float)(Math.Sin((double)(i + 1) / slices * 2 * Math.PI) * (scale - distance)),
                                                          (float)(Math.Cos((double)(i + 1) / slices * 2 * Math.PI) * (scale - distance)),
                                                          (float)(1f / (scale - distance)));
                        Vector3 BottomCenter = new Vector3((float)(Math.Sin((double)i / slices * 2 * Math.PI) * (scale - distance)),
                                                           (float)(Math.Cos((double)i / slices * 2 * Math.PI) * (scale - distance)),
                                                           (float)(1f / (scale - distance)));
                        GL.TexCoord2(1f, 0f);
                        GL.Vertex3(MiddleCenter);
                        GL.TexCoord2(0f, 0f);
                        GL.Vertex3(MiddleRight);
                        GL.TexCoord2(0f, 1f);
                        GL.Vertex3(BottomRight);
                        GL.TexCoord2(1f, 1f);
                        GL.Vertex3(BottomCenter);
                    }

                GL.End();
            }
            GL.EndList();
        }

        protected override void OnResize(EventArgs e)
        {
            var clientRect = this.ClientRectangle;
            GL.Viewport(
                clientRect.X,
                clientRect.Y,
                clientRect.Width,
                clientRect.Height);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Width / (float)Height, 1.0f, 50.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[Key.Escape])
                Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit |
                     ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Texture);
            GL.Translate(e.Time / 2, -e.Time, 0f);
            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Translate(0f, 0f, 1.5f);
            GL.Rotate(Mouse.X, Vector3.UnitY);
            GL.Rotate(Mouse.Y, Vector3.UnitX);
            GL.CallList(list);
            SwapBuffers();
        }

        public static int LoadTexture(string filename)
        {
            TextureTarget Target = TextureTarget.Texture2D;
            int texture;
            GL.GenTextures(1, out texture);
            GL.BindTexture(Target, texture);
            float version = Single.Parse(GL.GetString(StringName.Version).Substring(0, 3), System.Globalization.CultureInfo.InvariantCulture);
            if (version >= 1.4)
            {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, (int)All.True);
                GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            }
            else
            {
                GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            }
            GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            Bitmap bitmap = new Bitmap(filename);
            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.Finish();
            bitmap.UnlockBits(data);
            if (GL.GetError() != ErrorCode.NoError)
                throw new Exception("Error loading texture " + filename);
            return texture;
        }
    }
}