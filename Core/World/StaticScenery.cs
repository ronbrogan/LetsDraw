using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Core.Core.Rendering;
using Core.Loaders;

namespace Core.World
{
    public class StaticScenery : IRenderableComponent
    {
        public Guid Id { get; private set; }
        public Matrix4x4 Transform => WorldTransform.GetTransform();
        public WorldTransform WorldTransform = new WorldTransform();

        public List<Mesh> Meshes { get; set; }

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
