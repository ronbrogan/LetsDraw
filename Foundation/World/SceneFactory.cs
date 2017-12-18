using Core;
using Core.Serialization;
using Foundation.Loaders;
using Foundation.Rendering.Cameras;
using Newtonsoft.Json;
using System.IO;
using System.Numerics;

namespace Foundation.World
{
    public static class SceneFactory
    {
        public static Scene BuildDefaultScene()
        {
            var SpawnPoint = new Vector3(80, 290, 30);
            var terrainLoader = new ObjLoader("Data/Objects/powerhouse.obj");
            var scene = new Scene()
            {
                Name = "Default",
                SpawnPoint = SpawnPoint,

                Skybox = new Skybox("Rendering/Skyboxes/Skybox01/texture.png"),

                Camera = new FpCamera(SpawnPoint),

                
                Terrain = new Terrain(terrainLoader),
            };

            var powerthing = StaticScenery.FromObj(@"Data\Objects\powerthing.obj");
            powerthing.Transform.Position = new Vector3(380, 20, -300);
            powerthing.Transform.Scale = 0.7f;

            scene.Scenery.Add(powerthing);

            return scene;
        }
    }
}
