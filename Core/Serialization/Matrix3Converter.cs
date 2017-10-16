namespace Core.Serialization
{
    //public class Matrix3Converter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        return objectType == typeof(OpenTK.Matrix3);
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        var temp = JObject.Load(reader);
    //        return new OpenTK.Matrix3(
    //            ((float)temp["M11"]), ((float)temp["M12"]), ((float)temp["M13"]),
    //            ((float)temp["M21"]), ((float)temp["M22"]), ((float)temp["M23"]),
    //            ((float)temp["M31"]), ((float)temp["M32"]), ((float)temp["M33"])
    //        );
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        var mat = (OpenTK.Matrix3)value;
    //        serializer.Serialize(writer, new TempMatrix3
    //        {
    //            M11 = mat.M11, M12 = mat.M12, M13 = mat.M13,
    //            M21 = mat.M21, M22 = mat.M22, M23 = mat.M23,
    //            M31 = mat.M31, M32 = mat.M32, M33 = mat.M33
    //        });
    //    }

    //    private class TempMatrix3
    //    {
    //        public float M11 { get; set; }
    //        public float M12 { get; set; }
    //        public float M13 { get; set; }
    //        public float M21 { get; set; }
    //        public float M22 { get; set; }
    //        public float M23 { get; set; }
    //        public float M31 { get; set; }
    //        public float M32 { get; set; }
    //        public float M33 { get; set; }
    //    }
    //}
}
