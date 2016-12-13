using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core.Rendering;
using LetsDraw.Data.Enums;
using LetsDraw.Loaders;
using OpenTK;

namespace LetsDraw.Rendering.Models
{
    public static class PrimitiveGenerator
    {
        public static Mesh GenerateSphere(float radius)
        {
            var mesh = new Mesh();


            return mesh;
        }

        public static Mesh GenerateOctahedron(float diameter)
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
                //0, 3, 4,
                //0, 4, 5,
                //0, 5, 2,
                //0, 2, 3,

                //1, 2, 5,
                //1, 5, 4,
                //1, 4, 3,
                //1, 3, 2
                0,1,2,3,4,5,6,7,8,9,10,
                11,12,13,14,15,16,17,18,
                19,20,21,22,23
            };

            mesh.Material = new Material("none")
            {
                DiffuseColor = new Vector3(0f, 1f, 0f),
                IlluminationModel = IlluminationModel.Highlight,
                Transparency = 0
            };

            return mesh;
        }


    }
}
