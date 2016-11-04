using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using LetsDraw.Core.Rendering;
using LetsDraw.Loaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace LetsDraw.Rendering.HUD
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
        public int FontSize = 36;
        private Size ScreenSize;
        public Brush Color = Brushes.Yellow;

        protected uint Vao;
        protected List<uint> Vbos;
        protected int ShaderProgram;
        protected uint Texture;

        protected bool RegenTexture = true;

        public Vector2 Position;

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

        public void Draw()
        {
            //Generate image from string, get height and width, make texture from it
            if (RegenTexture)
                GenerateTexture();

            //Draw it
            GL.UseProgram(ShaderProgram);
            GL.BindVertexArray(Vao);
            GL.BlendFuncSeparate(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha, BlendingFactorSrc.One, BlendingFactorDest.Zero);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Texture);

            GL.Uniform1(GL.GetUniformLocation(ShaderProgram, "texture1"), 0);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.Zero);
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
        }

        private void GenerateTexture()
        {
            
            var bmp = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            var size = g.MeasureString(text, new Font(FontFamily.GenericMonospace, FontSize));
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
            Texture = TextureLoader.LoadTexture(data, bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
            RegenTexture = false;

            UpdateGeometry(size);
        }

        private void UpdateGeometry(SizeF size)
        {
            uint vao;
            uint vbo;
            uint ibo;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            var width = (size.Width / ScreenSize.Width);
            var height = (size.Height / ScreenSize.Height);

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

            var vertexFormatSize = BlittableValueType.StrideOf<VertexFormat>(new VertexFormat());

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * vertexFormatSize), vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out ibo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Count * sizeof(uint)), indices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexFormatSize, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, vertexFormatSize, 12);

            Vao = vao;
        }
    }
}
