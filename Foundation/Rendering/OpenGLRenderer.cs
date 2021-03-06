﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Foundation.Data.Shaders;
using Foundation.Data.Shaders.Generic;
using Foundation.Managers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Vector3 = OpenTK.Vector3;
using OpenTK.Input;
using Core.Primitives;
using Core;
using Core.Rendering;
using Core.Extensions;

namespace Foundation.Rendering
{
    public class OpenGLRenderer : IRenderer
    {
        private Dictionary<Guid, uint> MeshVAOs = new Dictionary<Guid, uint>();
        private Dictionary<Guid, uint> BoundingBoxVAOs = new Dictionary<Guid, uint>();
        //private Dictionary<Guid, uint> VertexBufferObjects = new Dictionary<Guid, uint>();
        //private Dictionary<Guid, uint> IndexBufferObjects = new Dictionary<Guid, uint>();

        private uint MatriciesUniformHandle = 0;
        private uint PointLightContainerHandle = 0;

        public bool RenderBoundingBoxes = false;

        public bool MeshCompiled(Mesh mesh)
        {
            return MeshVAOs.ContainsKey(mesh.Id);
        }

        public bool BoundingBoxCompiled(Mesh mesh)
        {
            return BoundingBoxVAOs.ContainsKey(mesh.Id);
        }

        public void CompileMesh(Mesh mesh)
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
            // At location 1 there'll be two floats, that's 12 bytes in to the format
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, VertexFormat.Size, 12);

            // Enables binding to location 2 in vertex shader
            GL.EnableVertexAttribArray(2);
            // At location 2 there'll be three floats, 20 bytes in to the format
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, VertexFormat.Size, 20);

            // Enables binding to location 3 in vertex shader
            GL.EnableVertexAttribArray(3);
            // At location 3 there'll be three floats, 32 bytes in to the format
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, VertexFormat.Size, 32);

            // Enables binding to location 4 in vertex shader
            GL.EnableVertexAttribArray(4);
            // At location 4 there'll be three floats, 20 bytes in to the format
            GL.VertexAttribPointer(4, 3, VertexAttribPointerType.Float, false, VertexFormat.Size, 44);

            MeshVAOs.Add(mesh.Id, vao);
        }

        public void CompileBoundingBox(Mesh mesh)
        {
            uint vao, vbo, ibo;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            var vec3Size = BlittableValueType.StrideOf(new Vector3());
            
            var verts = mesh.BoundingBox.GetVerticies();
            var lineIndicies = mesh.BoundingBox.GetIndicies();

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Count * vec3Size), verts.ToArray(), BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out ibo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(lineIndicies.Length * sizeof(uint)), lineIndicies.ToArray(), BufferUsageHint.StaticDraw);

            // Enables binding to location 0 in vertex shader
            GL.EnableVertexAttribArray(0);
            // At location 0, there'll be 3 floats
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vec3Size, 0);

            BoundingBoxVAOs.Add(mesh.Id, vao);
        }

        public void RenderMesh(Mesh mesh, Matrix4x4 RelativeTransformation, int? ShaderOverride = null)
        {
            var material = mesh.Material;

            if (material.Transparency == 1f)
                return;

            var shader = ShaderOverride ?? mesh.ShaderOverride ?? ShaderManager.GetShaderForMaterial(material);

            ShaderManager.SetShader(shader);
            GL.BindVertexArray(MeshVAOs[mesh.Id]);

            Matrix4x4 invertedNormal;
            Matrix4x4.Invert(RelativeTransformation, out invertedNormal);
            var NormalMatrix = Matrix4x4.Transpose(invertedNormal).ToGl();

            var data = new GenericUniform()
            {
                ModelMatrix = RelativeTransformation.ToGl(),
                NormalMatrix = NormalMatrix,
                Alpha = 1f - material.Transparency,
                SpecularExponent = material.SpecularExponent,
                SpecularColor = new OpenTK.Vector4(material.SpecularColor.ToGl(), 1)
            };

            SetMaterialProperties(material, ref data);

            var needToBufferData = data.GetHashCode() != mesh.LastGenericUniformHash;
            var needToInitBuffer = mesh.uniformBufferHandle == default(uint);

            if (needToInitBuffer)
            {
                GL.GenBuffers(1, out mesh.uniformBufferHandle);
                GL.BindBuffer(BufferTarget.UniformBuffer, mesh.uniformBufferHandle);
                GL.BufferData(BufferTarget.UniformBuffer, GenericUniform.Size, ref data, BufferUsageHint.DynamicDraw);
            }
            else if (needToBufferData)
            {
                mesh.LastGenericUniformHash = data.GetHashCode();
                GL.BindBuffer(BufferTarget.UniformBuffer, mesh.uniformBufferHandle);
                GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, GenericUniform.Size, ref data);
            }

            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 1, mesh.uniformBufferHandle);
            GL.DrawElements(PrimitiveType.Triangles, mesh.Indicies.Count, DrawElementsType.UnsignedInt, 0);

            if(RenderBoundingBoxes && mesh.BoundingBox != null)
            {
                if (!BoundingBoxCompiled(mesh))
                    CompileBoundingBox(mesh);

                var bbShader = ShaderManager.GetShader("FlatWhite");
                ShaderManager.SetShader(bbShader);
                GL.BindVertexArray(BoundingBoxVAOs[mesh.Id]);

                var bbColor = new Vector3(1, 1, 1);

                if (mesh.Colliding)
                    bbColor = new Vector3(1, 0, 0);

                GL.Uniform3(GL.GetUniformLocation(bbShader, "color"), ref bbColor);
                GL.DrawElements(PrimitiveType.Lines, 26, DrawElementsType.UnsignedInt, 0);
            }
        }

        private void SetMaterialProperties(Material material, ref GenericUniform data)
        {
            data.UseNormalMap = 0;
            data.UseDiffuseMap = 0;
            data.UseSpecularMap = 0;

            if (material.DiffuseMap != null)
            {
                TextureManager.SetActiveTexture(material.DiffuseMap.TextureBinding, TextureUnit.Texture0);
                data.UseDiffuseMap = 1;
            }
            else
            {
                data.DiffuseColor = new OpenTK.Vector4(material.DiffuseColor.ToGl(), 1);
            }

            if(material.BumpMap != null)
            {
                TextureManager.SetActiveTexture(material.BumpMap.TextureBinding, TextureUnit.Texture1);
                data.UseNormalMap = 1;
            }

            if(material.SpecularMap != null)
            {
                TextureManager.SetActiveTexture(material.SpecularMap.TextureBinding, TextureUnit.Texture2);
                data.UseSpecularMap = 1;
            }
        }

        public void AddAndSortMeshes(Dictionary<int, List<Mesh>> sortedMeshes, List<Mesh> rawMeshes)
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

        public void DrawSortedMeshes(Dictionary<int, List<Mesh>> meshes, Dictionary<Guid, Matrix4x4> transformLookup)
        {
            foreach(var group in meshes)
            {
                foreach(var mesh in group.Value)
                {
                    var RelativeTransformation = Matrix4x4.Identity;

                    if (transformLookup.ContainsKey(mesh.Id))
                        RelativeTransformation = transformLookup[mesh.Id];

                    RenderMesh(mesh, RelativeTransformation);
                }
            }

        }

        public void DrawRenderQueue(IRenderQueue queue)
        {
            foreach (var key in InputManager.PressedKeys)
            {
                switch (key)
                {
                    case Key.F4:
                        // This is a terminal press
                        InputManager.ProcessKey(key);
                        RenderBoundingBoxes = !RenderBoundingBoxes;
                        break;
                }
            }

            foreach (var group in queue.MeshRegistry)
            {
                foreach (var mesh in group.Value)
                {
                    var xform = queue.GetTransform(mesh);

                    RenderMesh(mesh, xform.GetTransformationMatrix());
                }
            }
        }

        public void SetMatricies(System.Numerics.Vector3 position, Matrix4x4 view, Matrix4x4 proj)
        {
            MatriciesUniform MatriciesUniform = new MatriciesUniform
            {
                ViewMatrix = view.ToGl(),
                ProjectionMatrix = proj.ToGl(),
                DetranslatedViewMatrix = view.ToGl().ClearTranslation(),
                ViewPosition = position.ToGl()
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

        public void AddPointLights(List<PointLight> pointLights)
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

        public void DrawLightPoints(List<PointLight> pointLights, uint uniformHandle)
        {
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, 1, uniformHandle);
            GL.PointSize(50);
            GL.Begin(PrimitiveType.Points);
           
            foreach (var light in pointLights)
            {
                GL.Vertex3(light.Position.Xyz().ToGl());
                GL.Color3(light.Color.Xyz().ToGl());
            }

            GL.End();
        }

        public void DeleteMesh(int vao, int ibo, int? vbo)
        {
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(ibo);

            if (vbo.HasValue)
                GL.DeleteBuffer(vbo.Value);
        }
    }
}
