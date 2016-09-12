using System.Collections.Generic;
using LetsDraw.Rendering;

namespace LetsDraw.Formats.Obj
{
    public class ObjMesh
    {
        public string Name { get; set; }
        public int Faces = 0;
        public List<uint> Indicies = new List<uint>();
        public List<VertexFormat> Verticies = new List<VertexFormat>();

        public ObjMesh(string name)
        {
            Name = name;
        }
    }
}
