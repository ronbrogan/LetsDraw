using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Serialization
{
    public class StreamConverter : JsonConverter
    {
        private readonly Stream binaryContents;

        public StreamConverter(Stream binaryContents)
        {
            this.binaryContents = binaryContents;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Stream).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var temp = JObject.Load(reader);

            var position = ((long)temp[nameof(StreamMetadata.Position)]);
            var length = ((long)temp[nameof(StreamMetadata.Length)]);
            var bytes = new byte[length];
            this.binaryContents.Seek(position, SeekOrigin.Begin);
            this.binaryContents.Read(bytes, 0, (int)length);
            var stream = new MemoryStream(bytes);

            bytes = null;

            return stream;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var stream = (Stream)value;

            var pos = this.binaryContents.Position;
            var inputPos = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(this.binaryContents);
            stream.Seek(inputPos, SeekOrigin.Begin);

            serializer.Serialize(writer, new StreamMetadata { Position = pos, Length = stream.Length });
        }

        private class StreamMetadata
        {
            public long Position { get; set; }
            public long Length { get; set; }
        }
    }
}
