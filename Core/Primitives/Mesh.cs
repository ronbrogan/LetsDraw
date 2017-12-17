using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Primitives
{
    [JsonConverter(typeof(MeshConverter))]
    public class Mesh : IDisposable
    {
        public Mesh()
        {
            Parent = Guid.NewGuid();
        }

        public Mesh(Material mat, Guid ObjSource)
        {
            Material = mat;
            Parent = ObjSource;
        }

        public Mesh(Guid parent)
        {
            Parent = parent;
        }

        public Guid Id = Guid.NewGuid();
        public Guid Parent { get; set; }

        public List<uint> Indicies = new List<uint>();
        public List<VertexFormat> Verticies = new List<VertexFormat>();
        public Cuboid BoundingBox = new Cuboid();

        public Material Material { get; set; }
        public int? ShaderOverride { get; set; }

        public int LastGenericUniformHash;
        public uint uniformBufferHandle = 0;


        public bool Colliding = false;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.

                Material.Dispose();
                Material = null;

                Indicies = null;
                Verticies = null;

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
