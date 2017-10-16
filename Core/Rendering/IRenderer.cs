using Core.Primitives;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Core.Rendering
{
    public interface IRenderer
    {
        bool MeshCompiled(Mesh mesh);
        bool BoundingBoxCompiled(Mesh mesh);
        void CompileMesh(Mesh mesh);
        void CompileBoundingBox(Mesh mesh);

        void RenderMesh(Mesh mesh, Matrix4x4 RelativeTransformation, int? ShaderOverride = null);
        void AddAndSortMeshes(Dictionary<int, List<Mesh>> sortedMeshes, List<Mesh> rawMeshes);

        void DrawSortedMeshes(Dictionary<int, List<Mesh>> meshes, Dictionary<Guid, Matrix4x4> transformLookup);
        void DrawRenderQueue(IRenderQueue queue);

        void SetMatricies(Vector3 position, Matrix4x4 view, Matrix4x4 proj);

        void DeleteMesh(int vao, int ibo, int? vbo);
    }
}
