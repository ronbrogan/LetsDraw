using System.Collections.Generic;
using System.Linq;
using Foundation.Core;
using Foundation.Core.Rendering;
using Foundation.Rendering;
using Foundation.Rendering.Models;
using Foundation.World.Cameras;
using OpenTK;
using Vector3 = OpenTK.Vector3;

namespace Foundation.World
{
    public class Scene
    {
        public bool Loaded = false;

        public ICamera Camera { get; set; }
        public Vector3 SpawnPoint { get; set; }

        public bool Textureless { get; set; }

        public Skybox Skybox { get; set; }
        public Terrain Terrain { get; set; }
        public List<StaticScenery> Scenery = new List<StaticScenery>();
        //public List<Mesh> Meshes = new List<Mesh>
        //{
        //    { PrimitiveGenerator.GenerateOctahedron(100f) }
        //};
        public RenderQueue RenderQueue { get; set; }

        //public List<PointLight> PointLights = new List<PointLight>
        //{
        //    new PointLight
        //    {
        //        Color = new OpenTK.Vector4(1f, 0f, 0f, 1f),
        //        Intensity = 200,
        //        Position = new OpenTK.Vector4(-200, 200, 100, 0),
        //        Range = 200
        //    },
        //    new PointLight
        //    {
        //        Color = new OpenTK.Vector4(0f, 1f, 0f, 1f),
        //        Intensity = 200,
        //        Position = new OpenTK.Vector4(200, 200, 100, 0),
        //        Range = 250
        //    },
        //    new PointLight
        //    {
        //        Color = new OpenTK.Vector4(0f, 0f, 1f, 1f),
        //        Intensity = 200,
        //        Position = new OpenTK.Vector4(0, 200, -200, 0),
        //        Range = 200
        //    }
        //};
        //public Matrix4 PointLightTransform { get; set; }

        public Scene()
        {
            SpawnPoint = new Vector3(0);
            Camera = new FpCamera(SpawnPoint);
        }

        public void Load()
        {
            Loaded = false;

            RenderQueue = new RenderQueue();

            if(Terrain != null)
                RenderQueue.Add(Terrain);

            //foreach (var mesh in Meshes)
            //    RenderQueue.Add(mesh);

            foreach (var item in Scenery)
                RenderQueue.Add(item);

            Loaded = true;
        }

        public void Unload()
        {
            Loaded = false;

            Skybox.Destroy();
            RenderQueue.Destroy();

        }

        public void Update(double time)
        {
            if (!Loaded)
                return;

            //PointLightTransform = Matrix4.CreateRotationY(0.001f);
            //for (int l = 0; l < PointLights.Count; l++)
            //{
            //    var newLight = new PointLight
            //    {
            //        Position = PointLights[l].Position * PointLightTransform,
            //        Color = PointLights[l].Color,
            //        Intensity = PointLights[l].Intensity,
            //        Range = PointLights[l].Range
            //    };

            //    PointLights.RemoveAt(l);
            //    PointLights.Insert(l, newLight);
            //}

            
        }

        public void Draw()
        {
            if (!Loaded)
                return;

            //Renderer.DrawLightPoints(PointLights, Terrain.Meshes.First().uniformBufferHandle);
        }

        public void Dispose()
        {
            
        }
    }
}
