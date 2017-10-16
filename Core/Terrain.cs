using Core.Physics;
using Core.Primitives;
using Core.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Terrain : IRenderableComponent, ICollidableComponent, IDisposable
    {
        public Guid Id { get; private set; }

        public WorldTransform Transform { get; set; }

        public List<Mesh> Meshes { get; set; }

        public List<Mesh> CollisionMeshes
        {
            get { return Meshes; }
            set { Meshes = value; }
        }

        public Terrain()
        {
            Id = Guid.NewGuid();
        }

        public Terrain(string objPath)
        {
            Transform = new WorldTransform(Id);
            // TODO invert this
            //var obj = new ObjLoader(objPath);
            //Id = obj.Id;
            //Meshes = obj.Meshes.Values.ToList();
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
                    foreach (var mesh in Meshes)
                        mesh.Dispose();
                }

                // TODO: set large fields to null.
                Meshes = null;
                CollisionMeshes = null;
                Transform = null;

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
