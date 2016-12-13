using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Core.Rendering;
using LetsDraw.Loaders;
using LetsDraw.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LetsDraw.Core
{
    public class Terrain : IRenderableComponent
    {
        public Guid Id { get; private set; }

        public Matrix4x4 Transform { get; set; }

        public List<Mesh> Meshes { get; set; }

        public Terrain()
        {
            Transform = Matrix4x4.Identity;
            var obj = new ObjLoader("Data/Objects/powerhouse.obj");
            Id = obj.Id;
            Meshes = obj.Meshes.Values.ToList();
        }
    }
}
