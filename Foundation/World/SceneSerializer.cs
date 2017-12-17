using Core.Rendering;
using Core.Serialization;
using Foundation.Rendering.Cameras;
using Foundation.World;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;

namespace Foundation.World.Serialization
{
    public class SceneSerializer
    {
        public void SerializeScene(Scene scene, string outputPath)
        {
            using (var sceneFile = new FileStream(Path.Combine(outputPath, scene.Name + ".json"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            using (var sceneBinary = new FileStream(Path.Combine(outputPath, scene.Name + ".bin"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            using (StreamWriter sw = new StreamWriter(sceneFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.ContractResolver = new LetsDrawContractResolver(sceneBinary);

                serializer.Serialize(writer, scene);
            }
        }

        public Scene DeserializeScene()
        {
            throw new NotImplementedException();
        }

        private void serializeCamera(SceneSerializationContext ctx, ICamera camera)
        {
            ctx.WritePropertyName("camera");
            ctx.StartObject();

            ctx.WritePropertyName("position");
            ctx.WriteObject(camera.Position);

            ctx.EndObject();
        }

        private void serializeSkybox(SceneSerializationContext ctx, Skybox skybox)
        {
            ctx.WritePropertyName("skybox");
            ctx.StartObject();

            ctx.WritePropertyName("texture");
            ctx.StartObject();
            ctx.WriteStream(skybox.TextureStream);
            ctx.EndObject();


            ctx.EndObject();
        }


    }

    public class SceneSerializationContext : IDisposable
    {
        public readonly JsonSerializer serializer;
        public readonly JsonWriter writer;
        private readonly TextWriter textWriter;
        private readonly Stream binaryStream;

        public SceneSerializationContext()
        {
            binaryStream = new MemoryStream();
            serializer = new JsonSerializer();
            textWriter = new StringWriter();
            writer = new JsonTextWriter(textWriter);
            writer.AutoCompleteOnClose = true;
            writer.CloseOutput = false;
            StartObject();
        }

        public void WritePropertyName(string propertyName)
        {
            writer.WritePropertyName(propertyName, true);
        }

        public void StartObject()
        {
            writer.WriteStartObject();
        }

        public void EndObject()
        {
            writer.WriteEndObject();
        }

        public void WriteObject(object value)
        {
            serializer.Serialize(writer, value);
        }

        public string GetJson()
        {
            writer.Close();
            return textWriter.ToString();
        }

        public Stream GetBinary()
        {
            binaryStream.Seek(0, SeekOrigin.Begin);
            return binaryStream;
        }

        public void WriteStream(Stream textureStream)
        {
            var start = binaryStream.Position;
            var length = textureStream.Length;

            textureStream.Seek(0, SeekOrigin.Begin);

            textureStream.CopyTo(binaryStream);

            WritePropertyName("binary");
            StartObject();
            WritePropertyName("start");
            WriteObject(start);
            WritePropertyName("length");
            WriteObject(length);
            EndObject();
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    textWriter.Dispose();
                }
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}
