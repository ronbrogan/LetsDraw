using System.Collections.Generic;
using LetsDraw.Loaders;

namespace LetsDraw.Core.Rendering
{
    public class Mesh
    {
        public List<uint> Indicies = new List<uint>();
        public List<VertexFormat> Verticies = new List<VertexFormat>();

        public Mesh()
        {
        }
    }
}
