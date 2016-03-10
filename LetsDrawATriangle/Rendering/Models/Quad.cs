using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LetsDrawATriangle.Rendering.Models
{
    public class Quad : Model
    {
        public void Create()
        {
            uint vao;
            uint vbo;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            List<VertexFormat> vertices = new List<VertexFormat>
            {
                //new VertexFormat(new Vector3(-0.25f, 0.5f, 0f), new Vector4(1, 0, 0, 1)),
                //new VertexFormat(new Vector3(-0.25f, 0.75f, 0f), new Vector4(0, 1, 0, 1)),
                //new VertexFormat(new Vector3(0.25f, 0.5f, 0f), new Vector4(0, 0, 1, 1)),
                //new VertexFormat(new Vector3(0.25f, 0.75f, 0f), new Vector4(0, 0, 0, 1))
            };

            var vertexFormatSize = BlittableValueType.StrideOf<VertexFormat>(new VertexFormat());

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Count * vertexFormatSize), vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexFormatSize, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, vertexFormatSize, 12);

            base.Vao = vao;
            base.Vbos.Add(vbo);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(Matrix4 Projection, Matrix4 View)
        {
            GL.UseProgram(base.Program);
            GL.BindVertexArray(base.Vao);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        }
    }
}
