using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Core;
using Foundation.World;
using Foundation.World.Cameras;
using OpenTK;

namespace LetsDraw
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
            powerthing.WorldTransform.Position = new Vector3(380, 20, -300);
            powerthing.WorldTransform.Scale = 0.7f;

            scene.Scenery.Add(powerthing);

            return scene;
        }
    }
}
