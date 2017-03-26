using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Foundation.Serialization
{
    public class Vector2Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OpenTK.Vector2);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = JObject.Load(reader);
            return new OpenTK.Vector2(((float?)temp["X"]).GetValueOrDefault(), ((float?)temp["Y"]).GetValueOrDefault());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vec = (OpenTK.Vector2)value;
            serializer.Serialize(writer, new TempVector2 { X = vec.X, Y = vec.Y });
        }

        private class TempVector2
        {
            public float X { get; set; }
            public float Y { get; set; }
        }
    }
}
