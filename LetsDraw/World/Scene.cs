using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Core.Rendering;
using LetsDraw.Managers;
using LetsDraw.Rendering;
using LetsDraw.Rendering.Models;
using LetsDraw.World.Cameras;
using OpenTK;
using Vector3 = OpenTK.Vector3;
using Newtonsoft.Json;

namespace LetsDraw.World
{
    public class Scene
    {
        public bool Loaded = false;

        public ICamera Camera { get; set; }
        public Vector3 SpawnPoint = new Vector3(80, 290, 30);

        public bool Textureless { get; set; }

        public Skybox Skybox { get; set; }
        public Terrain Terrain { get; set; }
        public List<StaticScenery> Scenery { get; set; }
        public List<Mesh> Meshes = new List<Mesh>
        {
            { PrimitiveGenerator.GenerateOctahedron(100f) }
        };
        public RenderQueue RenderQueue { get; set; }

        public List<PointLight> PointLights = new List<PointLight>
        {
            new PointLight
            {
                Color = new OpenTK.Vector4(1f, 0f, 0f, 1f),
                Intensity = 200,
                Position = new OpenTK.Vector4(-200, 200, 100, 0),
                Range = 200
            },
            new PointLight
            {
                Color = new OpenTK.Vector4(0f, 1f, 0f, 1f),
                Intensity = 200,
                Position = new OpenTK.Vector4(200, 200, 100, 0),
                Range = 250
            },
            new PointLight
            {
                Color = new OpenTK.Vector4(0f, 0f, 1f, 1f),
                Intensity = 200,
                Position = new OpenTK.Vector4(0, 200, -200, 0),
                Range = 200
            }
        };
        public Matrix4 PointLightTransform { get; set; }

        public Scene()
        {
            
        }

        public void Load()
        {
            Loaded = false;

            RenderQueue = new RenderQueue();

            Scenery = new List<StaticScenery>();

            Skybox = new Skybox("Rendering/Skyboxes/Skybox01/texture.png");

            Camera = new FpCamera(SpawnPoint);

            Terrain = new Terrain("Data/Objects/powerhouse.obj");

            RenderQueue.Add(Terrain);


            var powerthing = StaticScenery.FromObj(@"Data\Objects\powerthing.obj");
            powerthing.WorldTransform.Position = new Vector3(380, 20, -300);
            powerthing.WorldTransform.Scale = 0.7f;

            Scenery.Add(powerthing);

            foreach (var mesh in Meshes)
                RenderQueue.Add(mesh);

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

            Skybox.Update(time);
            Camera.UpdateCamera(time);

            PointLightTransform = Matrix4.CreateRotationY(0.001f);
            for (int l = 0; l < PointLights.Count; l++)
            {
                var newLight = new PointLight
                {
                    Position = PointLights[l].Position * PointLightTransform,
                    Color = PointLights[l].Color,
                    Intensity = PointLights[l].Intensity,
                    Range = PointLights[l].Range
                };

                PointLights.RemoveAt(l);
                PointLights.Insert(l, newLight);
            }

            var proj = Camera.GetProjectionMatrix();
            var view = Camera.GetViewMatrix();

            Renderer.AddPointLights(PointLights);
            Renderer.SetMatricies(Camera.Position, view, proj);
        }

        public void Draw()
        {
            if (!Loaded)
                return;

            Skybox.Draw();
            
            RenderQueue.Render();

            Renderer.DrawLightPoints(PointLights, Terrain.Meshes.First().uniformBufferHandle);
        }

        public void Dispose()
        {
            
        }
    }
}
