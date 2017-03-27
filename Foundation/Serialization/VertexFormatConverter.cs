using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK;

namespace Foundation.Serialization
{
    public class VertexFormatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(VertexFormat);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vert = (VertexFormat)value;
            serializer.Serialize(writer, new TempVertexFormat { p = vert.position, t = vert.texture, n = vert.normal, a = vert.tangent, b = vert.bitangent });
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = JObject.Load(reader);
            return new VertexFormat()
            {
                position = temp.SelectToken("p").ToObject<Vector3>(),
                texture = temp.SelectToken("t").ToObject<Vector2>(),
                normal = temp.SelectToken("n").ToObject<Vector3>(),
                tangent = temp.SelectToken("a").ToObject<Vector3>(),
                bitangent = temp.SelectToken("b").ToObject<Vector3>(),
            };
        }

        private struct TempVertexFormat
        {
            public Vector3 p;
            public Vector2 t;
            public Vector3 n;
            public Vector3 a;
            public Vector3 b;
        }
        
    }
}
