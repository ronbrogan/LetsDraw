using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Foundation.Serialization
{
    public class QuaternionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OpenTK.Quaternion);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = JObject.Load(reader);
            return new OpenTK.Quaternion(
                ((float)temp["X"]), ((float)temp["Y"]), ((float)temp["Z"]), ((float)temp["W"])
            );
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var quat = (OpenTK.Quaternion)value;
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
