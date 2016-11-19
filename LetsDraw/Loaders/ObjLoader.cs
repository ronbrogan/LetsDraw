using System;
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
        public Guid Id = Guid.NewGuid();
        public List<Vector3> RawVerts = new List<Vector3>();
        public List<Vector2> TextureCoords = new List<Vector2>();
        public List<Vector3> Normals = new List<Vector3>();

        public Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();

        public ObjLoader(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Select(l => l.Trim()).Where(l => !l.StartsWith("#"));

            var vertexDict = new IndexedDictionary<string, VertexFormat>();
            var currentMeshKey = "";

            foreach(var rawline in lines)
            {
                var line = rawline.ReduceWhitespace();

                var parts = line.Split(' ');

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
                        for (int i = 1; i < 4; i++)
                        {
                            var indicies = parts[i].Split('/');

                            var index = vertexDict.Add(parts[i], new VertexFormat(RawVerts[int.Parse(indicies[0]) - 1], TextureCoords[int.Parse(indicies[1]) - 1], Normals[int.Parse(indicies[2]) - 1]));
                            Meshes[currentMeshKey].Indicies.Add((uint)index);
                        }

                        break;
                }
            }
            Meshes[currentMeshKey].Verticies = vertexDict.Values;
        }
    }
}
