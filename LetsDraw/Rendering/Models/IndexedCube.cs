using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LetsDraw.Rendering.Models
{
    public class IndexedCube : Model
    {
        private Vector3 RotationSpeed;
        private Vector3 Rotation;

        private float pi = (float)Math.PI;

        public void Create()
        {
            uint vao;
            uint vbo;
            uint ibo;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            List<uint> indices = new List<uint>
            {
                0, 1, 2, 0, 2, 3, //front
                4, 5, 6, 4, 6, 7, //right
                8, 9, 10, 8, 10, 11, //back
                12, 13, 14, 12, 14, 15, //left
                16, 17, 18, 16, 18, 19, //upper
                20, 21, 22, 20, 22, 23 //bottom
            };

            List<VertexFormat> vertices = new List<VertexFormat>
            {
                  //front
                  new VertexFormat(new Vector3(-1.0f, -1.0f, 1.0f), new Vector2(0, 0)),
                  new VertexFormat(new Vector3( 1.0f, -1.0f, 1.0f), new Vector2(1, 0)),
                  new VertexFormat(new Vector3( 1.0f, 1.0f, 1.0f), new Vector2(1, 1)),
                  new VertexFormat(new Vector3(-1.0f, 1.0f, 1.0f), new Vector2(0, 1)),
 
                  //right
                  new VertexFormat(new Vector3(1.0f, 1.0f, 1.0f), new Vector2(0, 0)),
                  new VertexFormat(new Vector3(1.0f, 1.0f, -1.0f), new Vector2(1, 0)),
                  new VertexFormat(new Vector3(1.0f, -1.0f, -1.0f), new Vector2(1, 1)),
                  new VertexFormat(new Vector3(1.0f, -1.0f, 1.0f), new Vector2(0, 1)),
 
                  //back
                  new VertexFormat(new Vector3(-1.0f, -1.0f, -1.0f), new Vector2(0, 0)),
                  new VertexFormat(new Vector3( 1.0f, -1.0f, -1.0f), new Vector2(1, 0)),
                  new VertexFormat(new Vector3( 1.0f, 1.0f, -1.0f), new Vector2(1, 1)),
                  new VertexFormat(new Vector3(-1.0f, 1.0f, -1.0f), new Vector2(0, 1)),
 
                  //left
                  new VertexFormat(new Vector3(-1.0f, -1.0f, -1.0f), new Vector2(0, 0)),
                  new VertexFormat(new Vector3(-1.0f, -1.0f, 1.0f), new Vector2(1, 0)),
                  new VertexFormat(new Vector3(-1.0f, 1.0f, 1.0f), new Vector2(1, 1)),
                  new VertexFormat(new Vector3(-1.0f, 1.0f, -1.0f), new Vector2(0, 1)),
 
                  //upper
                  new VertexFormat(new Vector3( 1.0f, 1.0f, 1.0f), new Vector2(0, 0)),
                  new VertexFormat(new Vector3(-1.0f, 1.0f, 1.0f), new Vector2(1, 0)),
                  new VertexFormat(new Vector3(-1.0f, 1.0f, -1.0f), new Vector2(1, 1)),
                  new VertexFormat(new Vector3( 1.0f, 1.0f, -1.0f), new Vector2(0, 1)),
 
                  //bottom
                  new VertexFormat(new Vector3(-1.0f, -1.0f, -1.0f), new Vector2(0, 0)),
                  new VertexFormat(new Vector3( 1.0f, -1.0f, -1.0f), new Vector2(1, 0)),
                  new VertexFormat(new Vector3( 1.0f, -1.0f, 1.0f), new Vector2(1, 1)),
                  new VertexFormat(new Vector3(-1.0f, -1.0f, 1.0f), new Vector2(0, 1))
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

            base.Vao = vao;
            base.Vbos.Add(vbo);
            base.Vbos.Add(ibo);

            RotationSpeed = new Vector3(90f, 90f, 90f);
            Rotation = new Vector3(0f, 0f, 0f);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(Matrix4 Projection, Matrix4 View)
        {
            var modelTransforms = Matrix4.Identity;

            Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), (float)Math.PI / 3, out modelTransforms);

            GL.UseProgram(base.Program);
            GL.BindVertexArray(base.Vao);

            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, base.Textures["crate"]);

            GL.Uniform1(GL.GetUniformLocation(base.Program, "texture1"), 0);

            GL.UniformMatrix4(GL.GetUniformLocation(base.Program, "model"), false, ref modelTransforms);

            GL.UniformMatrix4(GL.GetUniformLocation(base.Program, "view_matrix"), false, ref View);

            GL.UniformMatrix4(GL.GetUniformLocation(base.Program, "projection_matrix"), false, ref Projection);

            
            GL.DrawElements(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, 0);
        }
    }
}
