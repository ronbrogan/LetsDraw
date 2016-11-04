using System.Collections.Generic;
using LetsDraw.Loaders;

namespace LetsDraw.Core.Rendering
{
    public class RenderMesh
    {
        public string Name { get; set; }
        public int Faces = 0;
        public List<uint> Indicies = new List<uint>();
        public List<VertexFormat> Verticies = new List<VertexFormat>();

        public RenderMesh(string name)
        {
            Name = name;
        }
    }
}
