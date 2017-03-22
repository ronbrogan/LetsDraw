using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Core.Core.Rendering;
using Core.Loaders;

namespace Core.Core
{
    public class Terrain : IRenderableComponent
    {
        public Guid Id { get; private set; }

        public Matrix4x4 Transform { get; set; }

        public List<Mesh> Meshes { get; set; }

        public Terrain(string objPath)
        {
            Transform = Matrix4x4.Identity;
            var obj = new ObjLoader(objPath);
            Id = obj.Id;
            Meshes = obj.Meshes.Values.ToList();
        }
    }
}
