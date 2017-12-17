using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Primitives
{
    public class TextureMapConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TextureMap);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var tMap = (TextureMap)value;
            var resolver = serializer.ContractResolver;
            serializer.Serialize(writer, new TempTextureMap()
            {
                tex = tMap.textureData
            });
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var temp = JObject.Load(reader);

            var tex = temp.SelectToken("tex");
            
            var tMap = new TextureMap()
            {
                textureData =  serializer.Deserialize<MemoryStream>(tex.CreateReader())
            };

            return tMap;
        }

        private class TempTextureMap
        {
            public Stream tex { get; set; }
        }

    }
}
