using System;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Serialization
{
    public class Matrix4Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Matrix4x4);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = JObject.Load(reader);
            return new Matrix4x4(
                ((float)temp["M11"]), ((float)temp["M12"]), ((float)temp["M13"]), ((float)temp["M14"]),
                ((float)temp["M21"]), ((float)temp["M22"]), ((float)temp["M23"]), ((float)temp["M24"]),
                ((float)temp["M31"]), ((float)temp["M32"]), ((float)temp["M33"]), ((float)temp["M34"]),
                ((float)temp["M41"]), ((float)temp["M42"]), ((float)temp["M43"]), ((float)temp["M44"])
            );
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var mat = (Matrix4x4)value;
            serializer.Serialize(writer, new TempMatrix4
            {
                M11 = mat.M11, M12 = mat.M12, M13 = mat.M13, M14 = mat.M14,
                M21 = mat.M21, M22 = mat.M22, M23 = mat.M23, M24 = mat.M24,
                M31 = mat.M31, M32 = mat.M32, M33 = mat.M33, M34 = mat.M34,
                M41 = mat.M41, M42 = mat.M42, M43 = mat.M43, M44 = mat.M44
            });
        }

        private class TempMatrix4
        {
            public float M11 { get; set; }
            public float M12 { get; set; }
            public float M13 { get; set; }
            public float M14 { get; set; }
            public float M21 { get; set; }
            public float M22 { get; set; }
            public float M23 { get; set; }
            public float M24 { get; set; }
            public float M31 { get; set; }
            public float M32 { get; set; }
            public float M33 { get; set; }
            public float M34 { get; set; }
            public float M41 { get; set; }
            public float M42 { get; set; }
            public float M43 { get; set; }
            public float M44 { get; set; }
        }
    }
}
