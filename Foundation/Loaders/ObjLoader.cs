using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Foundation.Core;
using Foundation.Core.Rendering;
using Foundation.Core.Primitives;

namespace Foundation.Loaders
{
    public class ObjLoader
    {
        public Guid Id = Guid.NewGuid();
        public List<Vector3> RawVerts = new List<Vector3>();
        public List<Vector2> TextureCoords = new List<Vector2>();
        public List<Vector3> Normals = new List<Vector3>();

        public Dictionary<string, Mesh> Meshes = new Dictionary<string, Mesh>();
        public Dictionary<string, IndexedDictionary<string, VertexFormat>> MeshDicts = new Dictionary<string, IndexedDictionary<string, VertexFormat>>();

        public ObjLoader(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Select(l => l.Trim()).Where(l => !l.StartsWith("#"));

            var currentMeshKey = "";

            foreach (var parts in lines.Select(rawline => rawline.ReduceWhitespace()).Select(line => line.Split(' ')))
            {
                switch (parts[0])
                {
                    case "mtllib":
                        var mats = new MtlLoader(Path.Combine(Path.GetDirectoryName(filePath), parts[1]));
                        foreach (var mat in mats.Materials)
                        {
                            Meshes.Add(mat.MaterialName, new Mesh(mat, Id));
                            MeshDicts.Add(mat.MaterialName, new IndexedDictionary<string, VertexFormat>(65536));
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
                            Meshes[currentMeshKey].Verticies = MeshDicts[currentMeshKey].Values;
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

                        var mesh = Meshes[currentMeshKey];
                        var dict = MeshDicts[currentMeshKey];

                        var index0 = dict.Add(parts[1], vert0);
                        mesh.Indicies.Add((uint)index0);
                        var index1 = dict.Add(parts[2], vert1);
                        mesh.Indicies.Add((uint)index1);
                        var index2 = dict.Add(parts[3], vert2);
                        mesh.Indicies.Add((uint)index2);

                        mesh.BoundingBox.LowerX = MathExtensions.Min(mesh.BoundingBox.LowerX, vert0position.X, vert1position.X, vert2position.X);
                        mesh.BoundingBox.UpperX = MathExtensions.Max(mesh.BoundingBox.UpperX, vert0position.X, vert1position.X, vert2position.X);
                        mesh.BoundingBox.LowerY = MathExtensions.Min(mesh.BoundingBox.LowerY, vert0position.Y, vert1position.Y, vert2position.Y);
                        mesh.BoundingBox.UpperY = MathExtensions.Max(mesh.BoundingBox.UpperY, vert0position.Y, vert1position.Y, vert2position.Y);
                        mesh.BoundingBox.LowerZ = MathExtensions.Min(mesh.BoundingBox.LowerZ, vert0position.Z, vert1position.Z, vert2position.Z);
                        mesh.BoundingBox.UpperZ = MathExtensions.Max(mesh.BoundingBox.UpperZ, vert0position.Z, vert1position.Z, vert2position.Z);

                        break;
                }
            }
            Meshes[currentMeshKey].Verticies = MeshDicts[currentMeshKey].Values;
        }
    }
}
