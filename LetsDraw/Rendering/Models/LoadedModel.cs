using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Formats;
using LetsDraw.Formats.Obj;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LetsDraw.Rendering.Models
{
    public class LoadedModel : Model
    {
        private Vector3 RotationSpeed;
        private Vector3 Rotation;

        private float pi = (float)Math.PI;
        private ObjMesh mesh { get; set; }

        public void Create()
        {
            uint vao;
            uint vbo;
            uint ibo;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            var obj = new ObjLoader("Objects/test.obj");
            mesh = obj.Meshes.First();

            var vertexFormatSize = BlittableValueType.StrideOf<VertexFormat>(new VertexFormat());

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mesh.Verticies.Count * vertexFormatSize), mesh.Verticies.ToArray(), BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out ibo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mesh.Indicies.Count * sizeof(uint)), mesh.Indicies.ToArray(), BufferUsageHint.StaticDraw);

            // Enables binding to location 0 in vertex shader
            GL.EnableVertexAttribArray(0);
            // At location 0, there'll be 3 floats
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexFormatSize, 0);

            // Enables binding to location 1 in vertex shader
            GL.EnableVertexAttribArray(1);
            // At location 1 there'll be two floats, and FYI, that's 12 bytes (3 * 4) in
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


            GL.DrawElements(PrimitiveType.Triangles, mesh.Indicies.Count, DrawElementsType.UnsignedInt, 0);
        }
    }
}
