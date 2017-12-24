using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Foundation.Loaders;
using Foundation.Managers;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Core.Rendering;
using Core;
using System.Numerics;
using Core.Loaders;
using Core.Dependencies;

namespace Foundation.Rendering.HUD
{
    public class TextDisplay : IHudElement
    {
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (value == text)
                    return;
                RegenTexture = true;
                text = value;
            }
        }

        private string text = "";
        public int FontSize = 40;
        private Size ScreenSize;
        public Brush Color = Brushes.White;

        protected uint Vao;
        protected List<uint> Vbos;
        protected int ShaderProgram;
        protected int TextureLocation;
        protected int Texture;

        protected bool RegenTexture = true;

        public Vector2 Position;

        public SizeF LastSize { get; set; }
        public Size LastScreenSize { get; set; }

        public TextDisplay(Size screenSize, float originX, float originY, string text = "", int fontSize = 36, Brush color = null)
        {
            //Vbos = new List<uint>();
            Position = new Vector2(originX, originY);
            this.text = text;
            FontSize = fontSize;
            ScreenSize = screenSize;
            if (color != null)
                Color = color;
        }

        public TextDisplay(Size screenSize, Vector2 origin, string text = "", int fontSize = 36, Brush color = null)
        {
            //Vbos = new List<uint>();
            Position = origin;
            this.text = text;
            FontSize = fontSize;
            ScreenSize = screenSize;
            if (color != null)
                Color = color;
        }

        public void Resize(int width, int height)
        {
            ScreenSize = new Size(width, height);
        }

        public void Draw()
        {
            //Generate image from string, get height and width, make texture from it
            if (RegenTexture)
                GenerateTexture();

            //Draw it
            ShaderManager.SetShader(ShaderProgram);
            GL.BindVertexArray(Vao);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.Uniform1(TextureLocation, 0);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        public virtual void Update()
        {
            
        }

        public virtual void Update(double deltaTime)
        {
            
        }

        public void SetShader(int ProgramHandle)
        {
            ShaderProgram = ProgramHandle;
            TextureLocation = GL.GetUniformLocation(ShaderProgram, "texture1");
        }

        private void GenerateTexture()
        {
            var binder = DependencyContainer.Resolve<ITextureBinder>();
            var bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            var size = g.MeasureString(text, new Font(FontFamily.GenericMonospace, FontSize, FontStyle.Bold));
            bmp = new Bitmap((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), PixelFormat.Format32bppArgb);
            g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.Transparent, 0, 0, (int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString(text, new Font(FontFamily.GenericMonospace, FontSize), Color, 0, 0);
            g.Flush();
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            using (var ms = new System.IO.MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                Texture = binder.Bind(ms);
            }
                
            RegenTexture = false;

            if(LastSize != size || LastScreenSize != ScreenSize)
                UpdateGeometry(size);
        }

        private void UpdateGeometry(SizeF size)
        {
            uint vao;
            uint vbo = 0;
            uint ibo = 0;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            var width = size.Width * FontSize / (size.Height * ScreenSize.Width);
            var height = FontSize / (float)ScreenSize.Height;

            List<uint> indices = new List<uint>
            {
                0, 1, 2, 0, 2, 3 //front
            };

            List<VertexFormat> vertices = new List<VertexFormat>
            {
                  new VertexFormat(new Vector3( Position.X, Position.Y - height, 0.0f), new Vector2(0, 1), Vector3.One),
                  new VertexFormat(new Vector3( Position.X + width, Position.Y - height, 0.0f), new Vector2(1, 1), Vector3.One),
                  new VertexFormat(new Vector3( Position.X + width, Position.Y, 0.0f), new Vector2(1, 0), Vector3.One),
                  new VertexFormat(new Vector3( Position.X, Position.Y, 0.0f), new Vector2(0, 0), Vector3.One),
            };

            var vertexFormatSize = VertexFormat.Size;

            var generateBuffer = vbo == 0;
            var generatedBuffer = false;

            if (generateBuffer)
            {
                GL.GenBuffers(1, out vbo);
                generatedBuffer = true;
            }
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            if (generatedBuffer)
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * vertexFormatSize), vertices.ToArray(), BufferUsageHint.StaticDraw);
            else
                GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, (vertices.Count * vertexFormatSize), vertices.ToArray());

            generateBuffer = ibo == 0;
            generatedBuffer = false;

            if (generateBuffer)
            {
                GL.GenBuffers(1, out ibo);
                generatedBuffer = true;
            }
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);

            if(generatedBuffer)
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Count * sizeof(uint)), indices.ToArray(), BufferUsageHint.StaticDraw);
            else
                GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)0, (indices.Count * sizeof(uint)), indices.ToArray());

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexFormatSize, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, vertexFormatSize, 12);

            Vao = vao;
            LastSize = size;
            LastScreenSize = ScreenSize;
        }
    }
}
