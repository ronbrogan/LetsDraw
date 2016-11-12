using System;
using System.Collections.Generic;
using LetsDraw.Loaders;

namespace LetsDraw.Core.Rendering
{
    public class Mesh
    {
        public Guid Id = Guid.NewGuid();

        public List<uint> Indicies = new List<uint>();
        public List<VertexFormat> Verticies = new List<VertexFormat>();

        public Material Material { get; set; }

        public Mesh()
        {
        }

        public Mesh(Material mat)
        {
            Material = mat;
        }
    }
}
