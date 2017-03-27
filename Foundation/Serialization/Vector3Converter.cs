using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Foundation.Serialization
{
    public class Vector3Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OpenTK.Vector3);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = JObject.Load(reader);
            return new OpenTK.Vector3(((float)temp["X"]), ((float)temp["Y"]), ((float)temp["Z"]));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vec = (OpenTK.Vector3)value;
            serializer.Serialize(writer, new TempVector3 { X = vec.X, Y = vec.Y, Z = vec.Z });
        }

        private class TempVector3
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
        }
    }
}
