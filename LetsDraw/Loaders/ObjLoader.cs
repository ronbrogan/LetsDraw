using System.Collections.Generic;
using System.IO;
using System.Linq;
using LetsDraw.Core;
using LetsDraw.Core.Rendering;
using OpenTK;

namespace LetsDraw.Loaders
{
    public class ObjLoader
    {
        public List<Vector3> RawVerts = new List<Vector3>();
        public List<Vector2> TextureCoords = new List<Vector2>();
        public List<Vector3> Normals = new List<Vector3>();

        public List<RenderMesh> Meshes = new List<RenderMesh>();

        public ObjLoader(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Select(l => l.Trim()).Where(l => !l.StartsWith("#"));
            RenderMesh currMesh = null;

            var vertexDict = new IndexedDictionary<string, VertexFormat>();

            foreach(var rawline in lines)
            {
                var line = rawline.ReduceWhitespace();

                var parts = line.Split(' ');

                switch (parts[0])
                {
                    case "v":
                        RawVerts.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;

                    case "vt":
                        TextureCoords.Add(new Vector2(float.Parse(parts[1]), float.Parse(parts[2])));
                        break;

                    case "vn":
                        Normals.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;

                    case "g":
                        if (currMesh != null)
                        {
                            currMesh.Verticies = vertexDict.Values;
                            Meshes.Add(currMesh);
                        }
                        currMesh = new RenderMesh(parts[1]);
                        break;

                    case "f":
                        for (int i = 1; i < 4; i++)
                        {
                            var indicies = parts[i].Split('/');

                            var index = vertexDict.Add(parts[i], new VertexFormat(RawVerts[int.Parse(indicies[0]) - 1], TextureCoords[int.Parse(indicies[1]) - 1], Normals[int.Parse(indicies[2]) - 1]));
                            currMesh.Indicies.Add((uint)index);
                        }
                        currMesh.Faces++;


                        break;

                }

            }
            currMesh.Verticies = vertexDict.Values;
            Meshes.Add(currMesh);

        }


    }
}
