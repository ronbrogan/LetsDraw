using System;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Serialization
{
    public class Vector2Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector2);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = JObject.Load(reader);
            return new Vector2(((float)temp["X"]), ((float)temp["Y"]));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vec = (Vector2)value;
            serializer.Serialize(writer, new TempVector2 { X = vec.X, Y = vec.Y });
        }

        private class TempVector2
        {
            public float X { get; set; }
            public float Y { get; set; }
        }
    }
}
