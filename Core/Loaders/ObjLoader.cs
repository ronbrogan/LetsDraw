using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Core.Core;
using Core.Core.Rendering;

namespace Core.Loaders
{
    public class ObjLoader
    {
        public Guid Id = Guid.NewGuid();
        public List<Vector3> RawVerts = new List<Vector3>();
        public List<Vector2> TextureCoords = new List<Vector2>();
        public List<Vector3> Normals = new List<Vector3>();

        public Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();

        public ObjLoader(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Select(l => l.Trim()).Where(l => !l.StartsWith("#"));

            var vertexDict = new IndexedDictionary<string, VertexFormat>(900000);
            var currentMeshKey = "";

            foreach (var parts in lines.Select(rawline => rawline.ReduceWhitespace()).Select(line => line.Split(' ')))
            {
                switch (parts[0])
                {
                    case "mtllib":
                        var mats = new MtlLoader(Path.Combine(Path.GetDirectoryName(filePath), parts[1]));
                        foreach(var mat in mats.Materials)
                        {
                            Meshes.Add(mat.MaterialName, new Mesh(mat, Id));
                        }
                        break;

                    case "v":
                        RawVerts.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;

                    case "vt":
                        TextureCoords.Add(new Vector2(float.Parse(parts[1]), float.Parse(parts[2])));
                        break;

                    case "vn":
                        Normals.Add(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;

                    case "usemtl":
                        if (!string.IsNullOrWhiteSpace(currentMeshKey))
                        {
                            Meshes[currentMeshKey].Verticies = vertexDict.Values;
                        }
                        currentMeshKey = parts[1];
                        break;

                    case "f":

                        var indicies0 = parts[1].Split('/');
                        var vert0position = RawVerts[int.Parse(indicies0[0]) - 1];
                        var vert0texture = TextureCoords[int.Parse(indicies0[1]) - 1];
                        var vert0normal = Normals[int.Parse(indicies0[2]) - 1];
                            
                        var indicies1 = parts[2].Split('/');
                        var vert1position = RawVerts[int.Parse(indicies1[0]) - 1];
                        var vert1texture = TextureCoords[int.Parse(indicies1[1]) - 1];
                        var vert1normal = Normals[int.Parse(indicies1[2]) - 1];
                            
                        var indicies2 = parts[3].Split('/');
                        var vert2position = RawVerts[int.Parse(indicies2[0]) - 1];
                        var vert2texture = TextureCoords[int.Parse(indicies2[1]) - 1];
                        var vert2normal = Normals[int.Parse(indicies2[2]) - 1];

                        var deltaPos1 = vert1position - vert0position;
                        var deltaPos2 = vert2position - vert0position;
                        var deltaUv1 = vert1texture - vert0texture;
                        var deltaUv2 = vert2texture - vert0texture;
                        var r = 1.0f / (deltaUv1.X * deltaUv2.Y - deltaUv1.Y * deltaUv2.X);
                        var tangent = ((deltaPos1 * deltaUv2.Y - deltaPos2 * deltaUv1.Y) * r).ToGl();
                        var bitangent = ((deltaPos2 * deltaUv1.X - deltaPos1 * deltaUv2.X) * r).ToGl();

                        var vert0 = new VertexFormat(vert0position.ToGl(), vert0texture.ToGl(), vert0normal.ToGl(), tangent, bitangent);
                        var vert1 = new VertexFormat(vert1position.ToGl(), vert1texture.ToGl(), vert1normal.ToGl(), tangent, bitangent);
                        var vert2 = new VertexFormat(vert2position.ToGl(), vert2texture.ToGl(), vert2normal.ToGl(), tangent, bitangent);

                        var index0 = vertexDict.Add(parts[1], vert0);
                        Meshes[currentMeshKey].Indicies.Add((uint)index0);
                        var index1 = vertexDict.Add(parts[2], vert1);
                        Meshes[currentMeshKey].Indicies.Add((uint)index1);
                        var index2 = vertexDict.Add(parts[3], vert2);
                        Meshes[currentMeshKey].Indicies.Add((uint)index2);
                        break;
                }
            }
            Meshes[currentMeshKey].Verticies = vertexDict.Values;
        }
    }
}
