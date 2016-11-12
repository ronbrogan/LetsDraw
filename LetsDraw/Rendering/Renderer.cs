﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core.Rendering;
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

        public static void CompileMesh(Mesh mesh)
        {
            uint vao, vbo, ibo;

            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

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

            VertexArrayObjects.Add(mesh.Id, vao);
            VertexBufferObjects.Add(mesh.Id, vbo);
            IndexBufferObjects.Add(mesh.Id, ibo);

        }

        public static void RenderMesh(Mesh mesh, Matrix4 RelativeTransformation, Matrix4 View, Matrix4 Projection)
        {
            var material = mesh.Material;

            if (material.Transparency == 1f)
                return;

            var shader = ShaderManager.GetShaderForMaterial(material);
            var unifs = ShaderManager.UniformCatalog[shader];

            GL.UseProgram(shader);
            GL.BindVertexArray(VertexArrayObjects[mesh.Id]);

            var NormalMatrix = Matrix3.Transpose(Matrix3.Invert(new Matrix3(RelativeTransformation)));

            if (material.DiffuseMap != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, material.DiffuseMap.TextureBinding);
                GL.Uniform1(unifs.DiffuseMap, 0);
                GL.Uniform1(unifs.UseDiffuseMap, 1);
            }
            else
            {
                GL.Uniform3(unifs.DiffuseColor, material.DiffuseColor);
                GL.Uniform1(unifs.UseDiffuseMap, 0);
            }

            GL.Uniform1(unifs.Alpha, 1f - material.Transparency);

            GL.UniformMatrix3(unifs.NormalMatrix, false, ref NormalMatrix);

            GL.UniformMatrix4(unifs.ModelMatrix, false, ref RelativeTransformation);

            GL.UniformMatrix4(unifs.ViewMatrix, false, ref View);

            GL.UniformMatrix4(unifs.ProjectionMatrix, false, ref Projection);

            GL.DrawElements(PrimitiveType.Triangles, mesh.Indicies.Count, DrawElementsType.UnsignedInt, 0);
        }

    }
}