using System;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Serialization
{
    public class QuaternionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Quaternion);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = JObject.Load(reader);
            return new Quaternion(
                ((float)temp["X"]), ((float)temp["Y"]), ((float)temp["Z"]), ((float)temp["W"])
            );
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var quat = (Quaternion)value;
            serializer.Serialize(writer, new TempQuaternion
            {
                X = quat.X, Y = quat.Y, Z = quat.Z, W = quat.W
            });
        }

        private class TempQuaternion
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public float W { get; set; }
        }
    }
}
