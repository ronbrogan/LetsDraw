using System.Collections.Generic;
using Core.Primitives;
using Core;
using System.Numerics;
using Core.Rendering.Enums;

namespace Foundation.Rendering.Models
{
    public static class MeshFactory
    {
        public static Mesh Sphere(float radius)
        {
            var mesh = new Mesh();

            // TODO all of it

            return mesh;
        }

        public static Mesh Cube(float sideLength)
        {
            var cube = new Mesh();

            var verts = new Vector3[9] {
                    new Vector3(),
                    new Vector3(-sideLength, -sideLength, -sideLength),
                    new Vector3(-sideLength, -sideLength, sideLength),
                    new Vector3(sideLength, -sideLength, -sideLength),
                    new Vector3(sideLength, -sideLength, sideLength),
                    new Vector3(sideLength, sideLength, sideLength),
                    new Vector3(-sideLength, sideLength, sideLength),
                    new Vector3(sideLength, sideLength, -sideLength),
                    new Vector3(-sideLength, sideLength, -sideLength),
            };

            var norms = new Vector3[7] {
                    new Vector3(),
                    new Vector3(0f, 1.0f, 0f),
                    new Vector3(0f, -1.0f, 0f),
                    new Vector3(0f, 0f, -1.0f),
                    new Vector3(-1.0f, 0f, 0f),
                    new Vector3(0f, 0f, 1.0f),
                    new Vector3(1.0f, 0f, 0f),
            };

            var tex = new Vector2[15] {
                    new Vector2(),
                    new Vector2(0.5f, 1f),
                    new Vector2(0.25f, 1f),
                    new Vector2(0.5f, 0.666666f),
                    new Vector2(0.25f, 0.666666f),
                    new Vector2(0.5f, 0.333333f),
                    new Vector2(0.5f, 0f),
                    new Vector2(0.25f, 0.333333f),
                    new Vector2(0.25f, 0f),
                    new Vector2(0.75f, 0.666666f),
                    new Vector2(0.75f, 0.333333f),
                    new Vector2(0f, 0.666666f),
                    new Vector2(0f, 0.333333f),
                    new Vector2(1f, 0.666666f),
                    new Vector2(1f, 0.333333f),
            };

            cube.Verticies = new List<VertexFormat>()
            {
                // 1
                new VertexFormat(verts[1], tex[1], norms[1]),
                new VertexFormat(verts[2], tex[2], norms[1]),
                new VertexFormat(verts[3], tex[3], norms[1]),

                new VertexFormat(verts[4], tex[4], norms[1]),
                new VertexFormat(verts[3], tex[3], norms[1]),
                new VertexFormat(verts[2], tex[2], norms[1]),

                // 3
                new VertexFormat(verts[5], tex[5], norms[2]),
                new VertexFormat(verts[6], tex[6], norms[2]),
                new VertexFormat(verts[7], tex[7], norms[2]),

                new VertexFormat(verts[8], tex[8], norms[2]),
                new VertexFormat(verts[7], tex[7], norms[2]),
                new VertexFormat(verts[6], tex[6], norms[2]),

                // 5
                new VertexFormat(verts[4], tex[3], norms[3]),
                new VertexFormat(verts[2], tex[9], norms[3]),
                new VertexFormat(verts[5], tex[5], norms[3]),

                new VertexFormat(verts[6], tex[10], norms[3]),
                new VertexFormat(verts[5], tex[5], norms[3]),
                new VertexFormat(verts[2], tex[9], norms[3]),

                // 7
                new VertexFormat(verts[3], tex[4], norms[4]),
                new VertexFormat(verts[4], tex[3], norms[4]),
                new VertexFormat(verts[7], tex[7], norms[4]),
                                                         
                new VertexFormat(verts[5], tex[5], norms[4]),
                new VertexFormat(verts[7], tex[7], norms[4]),
                new VertexFormat(verts[4], tex[3], norms[4]),
                
                // 9
                new VertexFormat(verts[1], tex[11], norms[5]),
                new VertexFormat(verts[3], tex[4], norms[5]),
                new VertexFormat(verts[8], tex[12], norms[5]),
                                                         
                new VertexFormat(verts[7], tex[7], norms[5]),
                new VertexFormat(verts[8], tex[12], norms[5]),
                new VertexFormat(verts[3], tex[4], norms[5]),

                // 11
                new VertexFormat(verts[2], tex[9], norms[6]),
                new VertexFormat(verts[1], tex[13], norms[6]),
                new VertexFormat(verts[6], tex[10], norms[6]),
                                                         
                new VertexFormat(verts[8], tex[14], norms[6]),
                new VertexFormat(verts[6], tex[10], norms[6]),
                new VertexFormat(verts[1], tex[13], norms[6]),
            };

            cube.Indicies = new List<uint>()
            {
                0,1,2,3,4,5,6,7,8,9,10,
                11,12,13,14,15,16,17,18,
                19,20,21,22,23,24,25,26,27,28,
                29,30,31,32,33,34,35
            };

            cube.Material = new Material("none")
            {
                DiffuseColor = new Vector3(0.5f, 0.5f, 0.8f),
                AmbientColor = new Vector3(0.5f, 0.5f, 0.8f),
                IlluminationModel = IlluminationModel.Color,
                Transparency = 0,
                SpecularExponent = 10
            };

            return cube;
        }

        public static Mesh Octahedron(float diameter)
        {
            var mesh = new Mesh();
            var halfD = diameter / 2f;
            var quarterD = diameter / 4f;

            mesh.Verticies = new List<VertexFormat>
            {
                //1
                new VertexFormat
                {
                    position = new Vector3(0,0,0),
                    normal = new Vector3(0, -0.44721350f, -0.89442730f)
                },
                new VertexFormat
                {
                    position = new Vector3(-quarterD,halfD,-quarterD),
                    normal = new Vector3(0, -0.44721350f, -0.89442730f)
                },
                new VertexFormat
                {
                    position = new Vector3(quarterD,halfD,-quarterD),
                    normal = new Vector3(0, -0.44721350f, -0.89442730f)
                },
                //2
                new VertexFormat
                {
                    position = new Vector3(0,0,0),
                    normal = new Vector3(0.89442724f, -0.44721362f, 0)
                },
                new VertexFormat
                {
                    position = new Vector3(quarterD,halfD,-quarterD),
                    normal = new Vector3(0.89442724f, -0.44721362f, 0)
                },
                new VertexFormat
                {
                    position = new Vector3(quarterD, halfD,quarterD),
                    normal = new Vector3(0.89442724f, -0.44721362f, 0)
                },
                //3
                new VertexFormat
                {
                    position = new Vector3(0,0,0),
                    normal = new Vector3(0, -0.44721371f, 0.89442712f)
                },
                new VertexFormat
                {
                    position = new Vector3(quarterD, halfD,quarterD),
                    normal = new Vector3(0, -0.44721371f, 0.89442712f)
                },
                new VertexFormat
                {
                    position = new Vector3(-quarterD,halfD,quarterD),
                    normal = new Vector3(0, -0.44721371f, 0.89442712f)
                },

                //4
                new VertexFormat
                {
                    position = new Vector3(0,0,0),
                    normal = new Vector3(-0.89442724f, -0.44721362f, 0)
                },
                new VertexFormat
                {
                    position = new Vector3(-quarterD,halfD,quarterD),
                    normal = new Vector3(-0.89442724f, -0.44721362f, 0)

                },
                new VertexFormat
                {
                    position = new Vector3(-quarterD,halfD,-quarterD),
                    normal = new Vector3(-0.89442724f, -0.44721362f, 0)
                },

                // 5
                new VertexFormat
                {
                    position = new Vector3(0,diameter,0),
                    normal = new Vector3(0, 0.44721362f, 0.89442724f)
                },
                new VertexFormat
                {
                    position = new Vector3(-quarterD,halfD,quarterD),
                    normal = new Vector3(0, 0.44721362f, 0.89442724f)
                },
                new VertexFormat
                {
                    position = new Vector3(quarterD, halfD,quarterD),
                    normal = new Vector3(0, 0.44721362f, 0.89442724f)
                },

                // 6
                new VertexFormat
                {
                    position = new Vector3(0,diameter,0),
                    normal = new Vector3(0.89442724f, 0.44721362f, 0)
                },
                new VertexFormat
                {
                    position = new Vector3(quarterD, halfD,quarterD),
                    normal = new Vector3(0.89442724f, 0.44721362f, 0)
                },
                new VertexFormat
                {
                    position = new Vector3(quarterD,halfD,-quarterD),
                    normal = new Vector3(0.89442724f, 0.44721362f, 0)
                },

                // 7
                new VertexFormat
                {
                    position = new Vector3(0,diameter,0),
                    normal = new Vector3(0, 0.44721362f, -0.89442724f)
                },
                new VertexFormat
                {
                    position = new Vector3(quarterD,halfD,-quarterD),
                    normal = new Vector3(0, 0.44721362f, -0.89442724f)
                },
                new VertexFormat
                {
                    position = new Vector3(-quarterD,halfD,-quarterD),
                    normal = new Vector3(0, 0.44721362f, -0.89442724f)
                },

                // 8
                new VertexFormat
                {
                    position = new Vector3(0,diameter,0),
                    normal = new Vector3(-0.89442724f, 0.44721362f, 0)
                },
                new VertexFormat
                {
                    position = new Vector3(-quarterD,halfD,-quarterD),
                    normal = new Vector3(-0.89442724f, 0.44721362f, 0)
                },
                new VertexFormat
                {
                    position = new Vector3(-quarterD,halfD,quarterD),
                    normal = new Vector3(-0.89442724f, 0.44721362f, 0)
                }
            };

            mesh.Indicies = new List<uint>
            {
                0,1,2,3,4,5,6,7,8,9,10,
                11,12,13,14,15,16,17,18,
                19,20,21,22,23
            };

            mesh.Material = new Material("none")
            {
                DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f),
                AmbientColor = new Vector3(0.5f, 0.5f, 0.5f),
                IlluminationModel = IlluminationModel.Color,
                Transparency = 0,
                SpecularExponent = 10
            };

            return mesh;
        }


    }
}
