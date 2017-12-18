using Core;
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
            using (var sceneFile = new FileStream(Path.Combine(outputPath, $"{scene.Name}.json"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            using (var sceneBinary = new FileStream(Path.Combine(outputPath, $"{scene.Name}.{Constants.FileExtensions.SceneBinary}"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            using (StreamWriter sw = new StreamWriter(sceneFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.ContractResolver = new LetsDrawContractResolver(sceneBinary);

                serializer.Serialize(writer, scene);
            }
        }

        public Scene DeserializeScene(string filePath)
        {
            Scene scene;

            var folder = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            var fileBase = Path.Combine(folder, fileName);

            using (var sceneFile = new FileStream($"{fileBase}.json", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var sceneBinary = new FileStream($"{fileBase}.{Constants.FileExtensions.SceneBinary}", FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader sr = new StreamReader(sceneFile))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.ContractResolver = new LetsDrawContractResolver(sceneBinary);

                scene = serializer.Deserialize<Scene>(reader);
            }

            return scene;
        }
    }
}
