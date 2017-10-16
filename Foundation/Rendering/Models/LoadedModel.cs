using System;
using System.Linq;
using Foundation.Loaders;
using OpenTK.Graphics.OpenGL;
using Core.Primitives;
using Core;
using Core.Rendering;
using System.Numerics;

namespace Foundation.Rendering.Models
{
    public class LoadedModel : Model
    {
        private float rotationAngle = 0;
        public Vector3 WorldPosition = new Vector3(0, 15, 0);
        public Vector3 Scale = new Vector3(1, 1, 1);

        private Matrix4x4 RelativeTransformation = Matrix4x4.Identity;

        private Mesh mesh { get; set; }


        public void Create()
        {
            uint vao;
            uint vbo;
            uint ibo;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            var obj = new ObjLoader("Data/Objects/map.obj");
            mesh = obj.Meshes.First(m => m.Value.Verticies.Count > 0).Value;

            var vertexFormatSize = VertexFormat.Size;

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
            // At location 1 there'll be two floats, and FYI, that's 12 bytes (3 * 4) in to the format
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, vertexFormatSize, 12);

            // Enables binding to location 2 in vertex shader
            GL.EnableVertexAttribArray(2);
            // At location 2 there'll be three floats, 20 bytes (3 * 4) + (2 * 4) in to the format
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, vertexFormatSize, 20);

            base.Vao = vao;
            base.Vbos.Add(vbo);
            base.Vbos.Add(ibo);

            var cat = new ShaderUniformCatalog
            {
                NormalMatrix = GL.GetUniformLocation(base.ShaderProgram, "normal_matrix"),
                ModelMatrix = GL.GetUniformLocation(base.ShaderProgram, "model_matrix"),
                ViewMatrix = GL.GetUniformLocation(base.ShaderProgram, "view_matrix"),
                ProjectionMatrix = GL.GetUniformLocation(base.ShaderProgram, "projection_matrix")
            };

            //ShaderManager.UniformCatalog.Add(base.ShaderProgram, cat);
        }

        public override void Update(double deltaTime = 0)
        {
            //var rotSpeed = .01f;
            //rotationAngle += rotSpeed * (float)deltaTime;

            //var scaleFactor = Math.Abs((float)Math.Sin(rotationAngle));

            //Scale.X = scaleFactor;
            //Scale.Y = scaleFactor;
            //Scale.Z = scaleFactor;

            var scaleMatrix = Matrix4x4.CreateScale(Scale);
            var rotationMatrix = Matrix4x4.CreateFromAxisAngle(new Vector3(0, 1, 0), rotationAngle);
            var translateMatrix = Matrix4x4.CreateTranslation(WorldPosition);
                   
            var scaledRotation = Matrix4x4.Multiply(scaleMatrix, rotationMatrix);
            RelativeTransformation = Matrix4x4.Multiply(scaledRotation, translateMatrix);

            base.Update(deltaTime);
        }

        public override void Draw(Matrix4x4 Projection, Matrix4x4 View)
        {
            //GL.UseProgram(base.ShaderProgram);
            //GL.BindVertexArray(base.Vao);

            //var NormalMatrix = Matrix3.Transpose(Matrix3.Invert(new Matrix3(RelativeTransformation)));

            //GL.ActiveTexture(TextureUnit.Texture0);
            //GL.BindTexture(TextureTarget.Texture2D, base.Textures["diffuse"]);

            //GL.Uniform1(GL.GetUniformLocation(base.ShaderProgram, "texture1"), 0);

            //var lookup = ShaderManager.UniformCatalog[base.ShaderProgram];

            //GL.UniformMatrix3(lookup.NormalMatrix, false, ref NormalMatrix);

            //GL.UniformMatrix4(lookup.ModelMatrix, false, ref RelativeTransformation);

            //GL.UniformMatrix4(lookup.ViewMatrix, false, ref View);

            //GL.UniformMatrix4(lookup.ProjectionMatrix, false, ref Projection);


            //GL.DrawElements(PrimitiveType.Triangles, mesh.Indicies.Count, DrawElementsType.UnsignedInt, 0);
            
        }
    }
}
