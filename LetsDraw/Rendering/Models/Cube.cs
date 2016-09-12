using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LetsDraw.Rendering.Models
{
    public class Cube : Model
    {
        private Vector3 RotationSpeed;
        private Vector3 Rotation;

        private float pi = (float)Math.PI;

        public void Create()
        {
            uint vao;
            uint vbo;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            List<VertexFormat> vertices = new List<VertexFormat>
            {
                ////vertices for the front face of the cube
                //new VertexFormat(new Vector3(-1.0f, -1.0f, 1.0f), new Vector4( 0.0f,  0.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, -1.0f, 1.0f), new Vector4(1.0f,  0.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f)),

                //new VertexFormat(new Vector3(-1.0f, 1.0f, 1.0f), new Vector4( 0.0f, 1.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(-1.0f, -1.0f, 1.0f), new Vector4( 0.0f,  0.0f, 1.0f, 1.0f)),

                ////vertices for the right face of the cube
                //new VertexFormat(new Vector3(1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, 1.0f, -1.0f), new Vector4(1.0f, 1.0f,  0.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, -1.0f, -1.0f), new Vector4(1.0f,  0.0f , 0.0f, 1.0f)),

                //new VertexFormat(new Vector3(1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, -1.0f, -1.0f), new Vector4(1.0f,  0.0f, 0.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, -1.0f, 1.0f), new Vector4(1.0f,  0.0f, 1.0f, 1.0f)),

                ////vertices for the back face of the cube
                //new VertexFormat(new Vector3(-1.0f, -1.0f, -1.0f), new Vector4( 0.0f,  0.0f,  0.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, -1.0f, -1.0f), new Vector4(1.0f,  0.0f,  0.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, 1.0f, -1.0f), new Vector4(1.0f, 1.0f,  0.0f, 1.0f)),

                //new VertexFormat(new Vector3(-1.0f, -1.0f, -1.0f), new Vector4( 0.0f,  0.0f,  0.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, 1.0f, -1.0f), new Vector4(1.0f, 1.0f,  0.0f, 1.0f)),
                //new VertexFormat(new Vector3(-1.0f, 1.0f, -1.0f), new Vector4( 0.0f, 1.0f,  0.0f, 1.0f)),

                ////vertices for the left face of the cube
                //new VertexFormat(new Vector3(-1.0f, -1.0f, -1.0f), new Vector4( 0.0f, 0.0f, 0.0f, 1.0f)),
                //new VertexFormat(new Vector3(-1.0f, -1.0f, 1.0f), new Vector4( 0.0f,  0.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(-1.0f, 1.0f, 1.0f), new Vector4( 0.0f, 1.0f, 1.0f, 1.0f)),

                //new VertexFormat(new Vector3(-1.0f, -1.0f, -1.0f), new Vector4( 0.0f,  0.0f,  0.0f, 1.0f)),
                //new VertexFormat(new Vector3(-1.0f, 1.0f, 1.0f), new Vector4( 0.0f, 1.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(-1.0f, 1.0f, -1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f)),

                ////vertices for the upper face of the cube
                //new VertexFormat(new Vector3(1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(-1.0f, 1.0f, 1.0f), new Vector4( 0.0f, 1.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, 1.0f, -1.0f), new Vector4(1.0f, 1.0f,  0.0f, 1.0f)),

                //new VertexFormat(new Vector3(-1.0f, 1.0f, 1.0f), new Vector4( 0.0f, 1.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, 1.0f, -1.0f), new Vector4(1.0f, 1.0f,  0.0f, 1.0f)),
                //new VertexFormat(new Vector3(-1.0f, 1.0f, -1.0f), new Vector4( 0.0f, 1.0f, 0.0f, 1.0f)),

                ////vertices for the bottom face of the cube
                //new VertexFormat(new Vector3(-1.0f, -1.0f, -1.0f), new Vector4( 0.0f,  0.0f, 0.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, -1.0f, -1.0f), new Vector4(1.0f,  0.0f,  0.0f, 1.0f)),
                //new VertexFormat(new Vector3(-1.0f, -1.0f, 1.0f), new Vector4( 0.0f,  0.0f, 1.0f, 1.0f)),

                //new VertexFormat(new Vector3(1.0f, -1.0f, -1.0f), new Vector4(1.0f,  0.0f, 0.0f, 1.0f)),
                //new VertexFormat(new Vector3(-1.0f, -1.0f, 1.0f), new Vector4( 0.0f,  0.0f, 1.0f, 1.0f)),
                //new VertexFormat(new Vector3(1.0f, -1.0f, 1.0f), new Vector4(1.0f,  0.0f, 1.0f, 1.0f))
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

            RotationSpeed = new Vector3(90f, 90f, 90f);
            Rotation = new Vector3(0f, 0f, 0f);
        }

        public override void Draw(Matrix4 Projection, Matrix4 View)
        {
            Rotation = 0.01f * RotationSpeed + Rotation;

            var RotationSin = new Vector3(Rotation.X * pi / 180f, Rotation.X * pi / 180f, Rotation.X * pi / 180f);

            GL.UseProgram(base.ShaderProgram);

            GL.Uniform3(GL.GetUniformLocation(base.ShaderProgram, "rotation"), RotationSin);

            GL.UniformMatrix4(GL.GetUniformLocation(base.ShaderProgram, "view_matrix"), false, ref View);

            GL.UniformMatrix4(GL.GetUniformLocation(base.ShaderProgram, "projection_matrix"), false, ref Projection);

            GL.BindVertexArray(base.Vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
    }
}
