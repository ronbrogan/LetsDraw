using System;
using System.Linq;
using Foundation.Core;
using Foundation.Core.Rendering;
using Foundation.Core.Primitives;
using Foundation.Loaders;
using Foundation.Managers;
using Foundation.Rendering.Models;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Foundation.World
{
    public class Skybox : Model
    {
        private int Texture;

        public Mesh Mesh { get; set; }

        public string TexturePath { get; set; }

        public Skybox()
        {
            
        }

        public Skybox(string texturePath)
        {
            TexturePath = texturePath;
        }

        public void Load()
        {
            uint vao;
            uint vbo;
            uint ibo;

            base.ShaderProgram = ShaderManager.CreateShader("Skybox01Shader", "Data/Shaders/Skybox/vertexShader.glsl", "Data/Shaders/Skybox/fragmentShader.glsl");
            Texture = TextureLoader.LoadTexture(TexturePath);

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            var obj = new ObjLoader("Data/Objects/mappedcube.obj");
            Mesh = obj.Meshes.First(m => m.Value.Verticies.Count > 0).Value;

            var vertexFormatSize = BlittableValueType.StrideOf(new VertexFormat());

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(Mesh.Verticies.Count * vertexFormatSize), Mesh.Verticies.ToArray(), BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out ibo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(Mesh.Indicies.Count * sizeof(uint)), Mesh.Indicies.ToArray(), BufferUsageHint.StaticDraw);

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

            if (!ShaderManager.UniformCatalog.ContainsKey(base.ShaderProgram))
                ShaderManager.UniformCatalog.Add(base.ShaderProgram, cat);
        }

        public override void Update(double deltaTime = 0)
        {
            base.Update(deltaTime);
        }

        public override void Draw()
        {
            ShaderManager.SetShader(base.ShaderProgram);
            GL.BindVertexArray(base.Vao);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Texture);
            GL.Uniform1(GL.GetUniformLocation(base.ShaderProgram, "texture1"), 0);

            GL.DrawElements(PrimitiveType.Triangles, Mesh.Indicies.Count, DrawElementsType.UnsignedInt, 0);
        }
    }
}
