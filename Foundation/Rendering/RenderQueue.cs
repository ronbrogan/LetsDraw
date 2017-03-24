using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Foundation.Core.Rendering;
using Foundation.Managers;

namespace Foundation.Rendering
{
    public class RenderQueue
    {
        // Meshes grouped by shader
        // Eventually by texture as well
        public Dictionary<int, List<Mesh>> MeshRegistry = new Dictionary<int, List<Mesh>>();

        public Dictionary<Guid, Matrix4x4> TransformRegistry = new Dictionary<Guid, Matrix4x4>();

        public void Add(Mesh mesh, Matrix4x4? MeshTransform = null)
        {
            if (!Renderer.MeshCompiled(mesh))
                Renderer.CompileMesh(mesh);

            var shader = ShaderManager.GetShaderForMaterial(mesh.Material);

            if (MeshRegistry.ContainsKey(shader))
            {
                MeshRegistry[shader].Add(mesh);
            }
            else
            {
                MeshRegistry.Add(shader, new List<Mesh>
                {
                    mesh
                });
            }

            if(MeshTransform.HasValue && !TransformRegistry.ContainsKey(mesh.Id))
            {
                TransformRegistry[mesh.Id] = MeshTransform.Value;
            }
        }

        public void Add(IRenderableComponent component)
        {
            TransformRegistry.Add(component.Id, component.Transform);

            foreach(var mesh in component.Meshes)
            {
                Add(mesh);
            }
        }

        public void UpdateTransform(IRenderableComponent component)
        {
            if (TransformRegistry.ContainsKey(component.Id))
                TransformRegistry[component.Id] = component.Transform;
        }

        public void Remove(Mesh mesh, bool SkipParentCheck = false)
        {
            var shader = ShaderManager.GetShaderForMaterial(mesh.Material);

            if (MeshRegistry.ContainsKey(shader))
            {
                var meshes = MeshRegistry[shader];
                var existingMesh = meshes.FirstOrDefault(m => m.Id == mesh.Id);
                meshes.Remove(existingMesh);
            }
        }

        public void Render()
        {
            Renderer.DrawRenderQueue(this, TransformRegistry);
        }

        public void Destroy()
        {
            var meshes = MeshRegistry.Values.SelectMany(m => m).ToArray();

            var meshCount = meshes.Length;

            for(var i = 0; i < meshes.Length; i++)
            {
                Remove(meshes[i]);
            }
        }

    }
}
