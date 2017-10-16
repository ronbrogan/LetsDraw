using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Core.Primitives;
using System.Numerics;

namespace Core.Serialization
{
    public class MeshConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Mesh);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var mesh = (Mesh)value;
            serializer.Serialize(writer, new TempMesh()
            {
                Id = mesh.Id,
                Parent = mesh.Parent,
                Indicies = mesh.Indicies,
                Verticies = mesh.Verticies,
                Material = mesh.Material
            });
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = JObject.Load(reader);

            var verticies = temp.SelectToken("Verticies").Children();

            var mesh = new Mesh()
            {
                Id = temp.SelectToken("Id").ToObject<Guid>(),
                Parent = temp.SelectToken("Parent").ToObject<Guid>(),
                Indicies = temp.SelectToken("Indicies").ToObject<List<uint>>(),
                Verticies = new List<VertexFormat>(verticies.Count()),
                Material = temp.SelectToken("Material").ToObject<Material>()
            };

            foreach(var vert in verticies)
            {
                var position = vert.SelectToken("p");
                var texture = vert.SelectToken("t");
                var normal = vert.SelectToken("n");
                var tangent = vert.SelectToken("a");
                var bitangent = vert.SelectToken("b");

                mesh.Verticies.Add(new VertexFormat()
                {
                    position = new Vector3(((float)position["X"]), ((float)position["Y"]), ((float)position["Z"])),
                    texture = new Vector2(((float)texture["X"]), ((float)texture["Y"])),
                    normal = new Vector3(((float)normal["X"]), ((float)normal["Y"]), ((float)normal["Z"])),
                    tangent = new Vector3(((float)tangent["X"]), ((float)tangent["Y"]), ((float)tangent["Z"])),
                    bitangent = new Vector3(((float)bitangent["X"]), ((float)bitangent["Y"]), ((float)bitangent["Z"])),
                });
            }

            return mesh;
        }

        private class TempMesh
        {
            public Guid Id = Guid.NewGuid();
            public Guid Parent { get; set; }

            public List<uint> Indicies = new List<uint>();
            public List<VertexFormat> Verticies = new List<VertexFormat>();

            public Material Material { get; set; }
        }
        
    }
}
