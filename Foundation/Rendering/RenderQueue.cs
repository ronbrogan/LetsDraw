using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.Managers;
using Core.Rendering;
using Core.Primitives;

namespace Foundation.Rendering
{
    public class RenderQueue : IRenderQueue, IDisposable
    {
        private readonly IRenderer renderer;

        public RenderQueue(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        // Meshes grouped by shader
        // Eventually by texture as well
        public Dictionary<int, List<Mesh>> MeshRegistry { get; set; } = new Dictionary<int, List<Mesh>>();

        protected internal Dictionary<Guid, WorldTransform> TransformRegistry { get; set; } = new Dictionary<Guid, WorldTransform>();

        public void Add(Mesh mesh, WorldTransform MeshTransform = null)
        {
            if (!renderer.MeshCompiled(mesh))
                renderer.CompileMesh(mesh);

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

            if(MeshTransform != null && !TransformRegistry.ContainsKey(mesh.Id))
            {
                TransformRegistry[mesh.Id] = MeshTransform;
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

        public WorldTransform GetTransform(Mesh mesh)
        {
            if (TransformRegistry.ContainsKey(mesh.Id))
                return TransformRegistry[mesh.Id];
            else if (TransformRegistry.ContainsKey(mesh.Parent))
                return TransformRegistry[mesh.Parent];

            return new WorldTransform(mesh.Id);
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
            renderer.DrawRenderQueue(this);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).

                    foreach(var meshGroup in MeshRegistry.Values)
                    {
                        foreach(var mesh in meshGroup)
                        {
                            mesh.Dispose();
                        }
                    }


                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // OpenGL buffers?
                

                // TODO: set large fields to null.
                MeshRegistry = null;
                TransformRegistry = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RenderQueue() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
