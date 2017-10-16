using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.Loaders;
using Core.Primitives;
using Core.Rendering;
using Core.Physics;

namespace Foundation.World
{
    public class StaticScenery : IRenderableComponent, ICollidableComponent
    {
        public Guid Id { get; set; }

        public WorldTransform Transform { get; set; }

        public List<Mesh> Meshes { get; set; }

        public List<Mesh> CollisionMeshes
        {
            get { return Meshes;  }
            set { Meshes = value; }
        }
        
        public StaticScenery()
        {
            Id = Guid.NewGuid();
            Transform = new WorldTransform(Id);
            Meshes = new List<Mesh>();
        }

        public static StaticScenery FromObj(string objPath)
        {
            var scenery = new StaticScenery();

            var objLoader = new ObjLoader(objPath);

            scenery.Meshes = objLoader.Meshes.Values.ToList();
            scenery.Id = objLoader.Id;

            return scenery;
        }
    }
}
