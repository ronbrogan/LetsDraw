using Foundation.Core;
using Foundation.World.Cameras;
using OpenTK;

namespace Foundation.World
{
    public static class SceneFactory
    {
        public static Scene BuildDefaultScene()
        {
            var SpawnPoint = new Vector3(80, 290, 30);
            var scene = new Scene()
            {
                SpawnPoint = SpawnPoint,

                Skybox = new Skybox("Rendering/Skyboxes/Skybox01/texture.png"),

                Camera = new FpCamera(SpawnPoint),

                Terrain = new Terrain("Data/Objects/powerhouse.obj"),
            };

            var powerthing = StaticScenery.FromObj(@"Data\Objects\powerthing.obj");
            powerthing.Transform.Position = new Vector3(380, 20, -300);
            powerthing.Transform.Scale = 0.7f;

            scene.Scenery.Add(powerthing);

            return scene;
        }
    }
}
