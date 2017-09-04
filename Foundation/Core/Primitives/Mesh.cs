using System;
using System.Collections.Generic;
using Foundation.Serialization;
using Newtonsoft.Json;

namespace Foundation.Core.Primitives
{
    [JsonConverter(typeof(MeshConverter))]
    public class Mesh
    {
        public Mesh()
        {
            Parent = Guid.NewGuid();
        }

        public Mesh(Material mat, Guid ObjSource)
        {
            Material = mat;
            Parent = ObjSource;
        }

        public Mesh(Guid parent)
        {
            Parent = parent;
        }

        public Guid Id = Guid.NewGuid();
        public Guid Parent { get; set; }

        public List<uint> Indicies = new List<uint>();
        public List<VertexFormat> Verticies = new List<VertexFormat>();
        public Cuboid BoundingBox = new Cuboid();

        public Material Material { get; set; }
        public int? ShaderOverride { get; set; }

        public int LastGenericUniformHash;
        public uint uniformBufferHandle = 0;
    }
}
