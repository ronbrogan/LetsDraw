using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Core.Rendering;
using LetsDraw.Loaders;
using LetsDraw.Managers;
using LetsDraw.Rendering.Models;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Numerics;
using Vector3 = OpenTK.Vector3;
using Vector4 = OpenTK.Vector4;

namespace LetsDraw.Rendering.Skyboxes
{
    public class Skybox : Model
    {
        private float rotationAngle = 0;
        public Vector3 WorldPosition = new Vector3(0, 15, 0);
        public Vector3 Scale = new Vector3(1, 1, 1);

        private uint Texture;

        private Matrix4 RelativeTransformation = Matrix4.Identity;

        private Mesh mesh { get; set; }


        public Skybox(string VertexShaderPath, string FragmentShaderPath, string TexturePath)
        {
            uint vao;
            uint vbo;
            uint ibo;

            base.ShaderProgram = ShaderManager.CreateShader("Skybox01Shader", VertexShaderPath, FragmentShaderPath);
            Texture = TextureLoader.LoadTexture(TexturePath);

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            var obj = new ObjLoader("Data/Objects/mappedcube.obj");
            mesh = obj.Meshes.First(m => m.Value.Verticies.Count > 0).Value;

            var vertexFormatSize = BlittableValueType.StrideOf(new VertexFormat());

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
                ModelMatrix = GL.GetUniformLocation(base.ShaderProgram, "model_matrix"),
                ViewMatrix = GL.GetUniformLocation(base.ShaderProgram, "view_matrix"),
                ProjectionMatrix = GL.GetUniformLocation(base.ShaderProgram, "projection_matrix")
            };

            ShaderManager.UniformCatalog.Add(base.ShaderProgram, cat);
        }

        public override void Update(double deltaTime = 0)
        {
            base.Update(deltaTime);
        }

        public override void Draw(Matrix4 Projection, Matrix4 View)
        {
            GL.DepthMask(false);
            GL.UseProgram(base.ShaderProgram);
            GL.BindVertexArray(base.Vao);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Texture);

            GL.Uniform1(GL.GetUniformLocation(base.ShaderProgram, "texture1"), 0);

            var lookup = ShaderManager.UniformCatalog[base.ShaderProgram];

            GL.UniformMatrix4(lookup.ModelMatrix, false, ref RelativeTransformation);

            var detranslatedView = View.ClearTranslation();
            
            GL.UniformMatrix4(lookup.ViewMatrix, false, ref detranslatedView);

            GL.UniformMatrix4(lookup.ProjectionMatrix, false, ref Projection);

            GL.DrawElements(PrimitiveType.Triangles, mesh.Indicies.Count, DrawElementsType.UnsignedInt, 0);
            GL.DepthMask(true);
        }
    }
}
