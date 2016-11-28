using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core.Rendering;
using LetsDraw.Data.Shaders;
using LetsDraw.Data.Shaders.Generic;
using LetsDraw.Loaders;
using LetsDraw.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Vector3 = OpenTK.Vector3;

namespace LetsDraw.Rendering
{
    public static class Renderer
    {
        private static Dictionary<Guid, uint> VertexArrayObjects = new Dictionary<Guid, uint>();
        private static Dictionary<Guid, uint> VertexBufferObjects = new Dictionary<Guid, uint>();
        private static Dictionary<Guid, uint> IndexBufferObjects = new Dictionary<Guid, uint>();

        private static uint MatriciesUniformHandle = 0;
        private static uint PointLightContainerHandle = 0;

        public static void CompileMesh(Mesh mesh)
        {
            uint vao, vbo, ibo;

            var vboExists = VertexBufferObjects.ContainsKey(mesh.Parent);
                

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            if(!vboExists)
                GL.GenBuffers(1, out vbo);
            else
                vbo = VertexBufferObjects[mesh.Parent];

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            if(!vboExists)
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
            if(!vboExists)
                VertexBufferObjects.Add(mesh.Parent, vbo);
            IndexBufferObjects.Add(mesh.Id, ibo);

        }

        public static void RenderMesh(Mesh mesh, Matrix4x4 RelativeTransformation, int? ShaderOverride = null)
        {
            var material = mesh.Material;

            if (material.Transparency == 1f)
                return;

            var shader = ShaderOverride ?? ShaderManager.GetShaderForMaterial(material);
            var unifs = ShaderManager.UniformCatalog[shader];

            ShaderManager.SetShader(shader);
            GL.BindVertexArray(VertexArrayObjects[mesh.Id]);

            var data = new GenericUniform();

            var needToInitBuffer = mesh.uniformBufferHandle == default(uint);

            if (needToInitBuffer)
            {
                // Convert to numerics to take advantage of SIMD operations
                Matrix4x4 invertedNormal;
                Matrix4x4.Invert(RelativeTransformation, out invertedNormal);
                var NormalMatrix = Matrix4x4.Transpose(invertedNormal).ToGl();

                data.ModelMatrix = RelativeTransformation.ToGl();
                data.NormalMatrix = NormalMatrix;
                data.Alpha = 1f - material.Transparency;
                data.SpecularExponent = material.SpecularExponent;
                data.SpecularColor = new OpenTK.Vector4(material.SpecularColor, 1);

                SetMaterialProperties(material, ref data, unifs);

                GL.GenBuffers(1, out mesh.uniformBufferHandle);
                GL.BindBuffer(BufferTarget.UniformBuffer, mesh.uniformBufferHandle);
                GL.BufferData(BufferTarget.UniformBuffer, GenericUniform.Size, ref data, BufferUsageHint.StaticDraw);
            }
            else
            {
                SetMaterialProperties(material, ref data, unifs);
            }

            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 1, mesh.uniformBufferHandle);
            GL.DrawElements(PrimitiveType.Triangles, mesh.Indicies.Count, DrawElementsType.UnsignedInt, 0);
        }

        private static void SetMaterialProperties(Material material, ref GenericUniform data, ShaderUniformCatalog unifs)
        {
            if (material.DiffuseMap != null)
            {
                TextureManager.SetActiveTexture(material.DiffuseMap.TextureBinding, TextureUnit.Texture0);
                data.UseDiffuseMap = 1;
            }
            else
            {
                data.DiffuseColor = new OpenTK.Vector4(material.DiffuseColor, 1);
                data.UseDiffuseMap = 0;
            }
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

                sortedMeshes[grouping.Key] = sortedMeshes[grouping.Key].OrderBy(m => m.Material?.DiffuseMap?.TextureBinding).ToList();
            }
        }

        public static void DrawSortedMeshes(Dictionary<int, List<Mesh>> meshes, Matrix4x4 RelativeTransformation)
        {
            foreach(var group in meshes)
            {
                foreach(var mesh in group.Value)
                {
                    RenderMesh(mesh, RelativeTransformation);
                }
            }

        }

        public static void SetMatricies(Vector3 position, Matrix4 view, Matrix4 proj)
        {
            MatriciesUniform MatriciesUniform = new MatriciesUniform
            {
                ViewMatrix = view,
                ProjectionMatrix = proj,
                DetranslatedViewMatrix = view.ClearTranslation(),
                ViewPosition = position
            };

            var needToInitBuffer = MatriciesUniformHandle == default(uint);

            if (needToInitBuffer)
                GL.GenBuffers(1, out MatriciesUniformHandle);

            GL.BindBuffer(BufferTarget.UniformBuffer, MatriciesUniformHandle);

            if (needToInitBuffer)
                GL.BufferData(BufferTarget.UniformBuffer, MatriciesUniform.Size, ref MatriciesUniform, BufferUsageHint.DynamicDraw);
            else
                GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, MatriciesUniform.Size, ref MatriciesUniform);

            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 0, MatriciesUniformHandle);
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        public static void AddPointLights(List<PointLight> pointLights)
        {
            var lights = pointLights.ToArray();

            var lightsSize = PointLight.Size * lights.Length;

            var needToInitBuffer = PointLightContainerHandle == default(uint);

            if (needToInitBuffer)
                GL.GenBuffers(1, out PointLightContainerHandle);

            GL.BindBuffer(BufferTarget.UniformBuffer, PointLightContainerHandle);

            if (needToInitBuffer)
                GL.BufferData(BufferTarget.UniformBuffer, lightsSize, lights, BufferUsageHint.StaticDraw);
            else
                GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, lightsSize, lights);

            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 2, PointLightContainerHandle);

            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        public static void DrawLightPoints(List<PointLight> pointLights)
        {
            GL.PointSize(50);
            GL.Begin(PrimitiveType.Points);
           
            foreach (var light in pointLights)
            {
                GL.Vertex3(light.Position.Xyz);
                GL.Color3(light.Color.Xyz);
            }

            GL.End();
        }
    }
}
