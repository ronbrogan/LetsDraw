using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Rendering;
using LetsDraw.Rendering.Models;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LetsDraw.Core
{
    public class TextDisplay : IHudElement
    {
        public string HexText = "";
        public float Width = 1;
        public float Height = 1;

        protected uint Vao;
        protected List<uint> Vbos;
        protected int ShaderProgram;
        protected uint Texture;

        protected bool RegenTexture = true;

        public Vector2 Position;

        public TextDisplay(float originX, float originY, float width = 1, float height = 1, string text = "")
        {
            Vbos = new List<uint>();
            Position = new Vector2(originX, originY);
            HexText = text;
        }

        public TextDisplay(Vector2 origin, string text = "")
        {
            Vbos = new List<uint>();
            Position = origin;
            HexText = text;
        }

        public void Create()
        {
            // This all needs to happen dynamically based on input, and when anything changes
            uint vao;
            uint vbo;
            uint ibo;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            List<uint> indices = new List<uint>
            {
                0, 1, 2, 0, 2, 3 //front
            };

            List<VertexFormat> vertices = new List<VertexFormat>
            {
                  new VertexFormat(new Vector3(-1.0f, 0.0f, 0.0f), new Vector2(0, 1)),
                  new VertexFormat(new Vector3( 0.0f, 0.0f, 0.0f), new Vector2(1, 1)),
                  new VertexFormat(new Vector3( 0.0f, 1.0f, 0.0f), new Vector2(1, 0)),
                  new VertexFormat(new Vector3(-1.0f, 1.0f, 0.0f), new Vector2(0, 0)),
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
            Vbos.Add(vbo);
            Vbos.Add(ibo);

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

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Update(double deltaTime)
        {
            throw new NotImplementedException();
        }

        public void SetShader(int ProgramHandle)
        {
            ShaderProgram = ProgramHandle;
        }

        private void GenerateTexture()
        {
            var bmp = new Bitmap(500, 500, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.Transparent, 0, 0, 500, 500);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.DrawString("HUD TEXT!!", new Font("Tahoma", 64), Brushes.Yellow, 0, 0);
            g.Flush();
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Texture = TextureLoader.LoadTexture(data, bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            RegenTexture = false;
        }
    }
}
