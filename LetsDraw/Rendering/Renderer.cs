using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core.Rendering;
using LetsDraw.Data.Shaders.Generic;
using LetsDraw.Loaders;
using LetsDraw.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LetsDraw.Rendering
{
    public static class Renderer
    {
        private static Dictionary<Guid, uint> VertexArrayObjects = new Dictionary<Guid, uint>();
        private static Dictionary<Guid, uint> VertexBufferObjects = new Dictionary<Guid, uint>();
        private static Dictionary<Guid, uint> IndexBufferObjects = new Dictionary<Guid, uint>();

        private static int LastShader = -1;

        public static void CompileMesh(Mesh mesh)
        {
            uint vao, vbo, ibo;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mesh.Verticies.Count * VertexFormat.Size), mesh.Verticies.ToArray(), BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out ibo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mesh.Indicies.Count * sizeof(uint)), mesh.Indicies.ToArray(), BufferUsageHint.StaticDraw);

            // Enables binding to location 0 in vertex shader
            GL.EnableVertexAttribArray(0);
            // At location 0, there'll be 3 floats
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, VertexFormat.Size, 0);

            // Enables binding to location 1 in vertex shader
            GL.EnableVertexAttribArray(1);
            // At location 1 there'll be two floats, and FYI, that's 12 bytes (3 * 4) in to the format
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, VertexFormat.Size, 12);

            // Enables binding to location 2 in vertex shader
            GL.EnableVertexAttribArray(2);
            // At location 2 there'll be three floats, 20 bytes (3 * 4) + (2 * 4) in to the format
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, VertexFormat.Size, 20);

            VertexArrayObjects.Add(mesh.Id, vao);
            VertexBufferObjects.Add(mesh.Id, vbo);
            IndexBufferObjects.Add(mesh.Id, ibo);

        }

        public static void RenderMesh(Mesh mesh, Matrix4 RelativeTransformation, Matrix4 View, Matrix4 Projection, int? ShaderOverride = null)
        {
            var material = mesh.Material;

            if (material.Transparency == 1f)
                return;

            var shader = ShaderOverride ?? ShaderManager.GetShaderForMaterial(material);
            var unifs = ShaderManager.UniformCatalog[shader];

            ShaderManager.SetShader(shader);
            GL.BindVertexArray(VertexArrayObjects[mesh.Id]);

            // Convert to numerics to take advantage of SIMD operations
            Matrix4x4 invertedNormal;
            Matrix4x4.Invert(RelativeTransformation.ToNumerics(), out invertedNormal);
            var NormalMatrix = Matrix4x4.Transpose(invertedNormal).ToGl();

            var data = new GenericUniform
            {
                ModelMatrix = RelativeTransformation,
                ViewMatrix = View,
                ProjectionMatrix = Projection,
                NormalMatrix = NormalMatrix,
                Alpha = 1f - material.Transparency
            };

            if (material.DiffuseMap != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, material.DiffuseMap.TextureBinding);
                GL.Uniform1(unifs.DiffuseMap, 0);
                data.UseDiffuseMap = 1;
            }
            else
            {
                data.DiffuseColor = material.DiffuseColor;
                data.UseDiffuseMap = 0;
            }

            var needToInitBuffer = mesh.uniformBufferHandle == default(uint);

            if (needToInitBuffer)
                GL.GenBuffers(1, out mesh.uniformBufferHandle);

            GL.BindBuffer(BufferTarget.UniformBuffer, mesh.uniformBufferHandle);

            if (needToInitBuffer)
                GL.BufferData(BufferTarget.UniformBuffer, GenericUniform.Size, ref data, BufferUsageHint.DynamicDraw);
            else
                GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, GenericUniform.Size, ref data);

            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 1, mesh.uniformBufferHandle);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);

            GL.DrawElements(PrimitiveType.Triangles, mesh.Indicies.Count, DrawElementsType.UnsignedInt, 0);
        }

        public static void AddAndSortMeshes(Dictionary<int, List<Mesh>> sortedMeshes, List<Mesh> rawMeshes)
        {
            var shaderAndMesh = rawMeshes.Select(m => new KeyValuePair<int, Mesh>(ShaderManager.GetShaderForMaterial(m.Material), m));

            var meshesByShader = shaderAndMesh.GroupBy(m => m.Key, m => m.Value);

            foreach(var grouping in meshesByShader)
            {
                var meshes = grouping.ToList();

                if (sortedMeshes.ContainsKey(grouping.Key))
                {
                    sortedMeshes[grouping.Key].AddRange(meshes);
                }
                else
                {
                    sortedMeshes.Add(grouping.Key, meshes);
                }
            }
        }

        public static void DrawSortedMeshes(Dictionary<int, List<Mesh>> meshes, Matrix4 RelativeTransformation, Matrix4 View, Matrix4 Projection)
        {
            foreach(var group in meshes)
            {
                foreach(var mesh in group.Value)
                {
                    RenderMesh(mesh, RelativeTransformation, View, Projection, group.Key);
                }
            }
        }
    }
}
