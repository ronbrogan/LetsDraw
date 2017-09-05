using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Foundation.Core.Rendering;
using Foundation.Core.Primitives;
using Foundation.Loaders;
using Foundation.World;
using Foundation.Core.Physics;

namespace Foundation.Core
{
    public class Terrain : IRenderableComponent, ICollidableComponent
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
            var obj = new ObjLoader(objPath);
            Id = obj.Id;
            Meshes = obj.Meshes.Values.ToList();
        }
    }
}
