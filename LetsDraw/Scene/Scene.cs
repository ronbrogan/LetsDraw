using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Core.Rendering;
using LetsDraw.Managers;
using LetsDraw.Rendering.Models;
using LetsDraw.Rendering.Skyboxes;
using OpenTK;
using Vector3 = OpenTK.Vector3;
using Newtonsoft.Json;

namespace LetsDraw.Rendering
{
    public class Scene
    {
        public Vector3 SpawnPoint = new Vector3(80, 290, 30);

        private Dictionary<int, List<Mesh>> MeshRegistry = new Dictionary<int, List<Mesh>>();
        private Dictionary<Guid, Matrix4> MeshTransformations = new Dictionary<Guid, Matrix4>();

        public ICamera Camera { get; set; }

        public Terrain Terrain { get; set; }

        public Skybox Skybox { get; set; }

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
            },
        };
        public Matrix4 PointLightTransform { get; set; }

        public List<Mesh> Models = new List<Mesh>
        {
            { PrimitiveGenerator.GenerateTetrahedron(100f) }

        };

        private float tetraRotation = 0f;

        public Scene()
        {
            
        }

        public void Load()
        {
            Skybox = new Skybox("Rendering/Skyboxes/Skybox01/texture.png");

            Camera = new FpCamera(SpawnPoint);

            Terrain = new Terrain();

            foreach (var mesh in Models)
                Renderer.CompileMesh(mesh);

            Renderer.AddAndSortMeshes(MeshRegistry, Terrain.Meshes);
            Renderer.AddAndSortMeshes(MeshRegistry, Models);
        }

        public void Unload()
        {
            Skybox.Destroy();
        }

        public void Update(double time)
        {
            Skybox.Update(time);
            Camera.UpdateCamera(time);

            tetraRotation += .001f;
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

            var accumulatedRotation = Matrix4.CreateRotationY(tetraRotation);
            var position = new Vector3(25, 15, 25);
            var translation = Matrix4.CreateTranslation(-position);
            var scale = Matrix4.CreateScale(0.5f);
            var backTranslate = Matrix4.CreateTranslation(position);

            var proj = Camera.GetProjectionMatrix();
            var view = Camera.GetViewMatrix();

            Renderer.AddPointLights(PointLights);
            Renderer.SetMatricies(Camera.Position, view, proj);

            var meshIdToRotate = Models[0].Id;

            if (MeshTransformations.ContainsKey(meshIdToRotate))
                MeshTransformations[meshIdToRotate] = translation * accumulatedRotation * scale * backTranslate;
            else
                MeshTransformations.Add(meshIdToRotate, translation * accumulatedRotation * scale * backTranslate);
        }

        public void Draw()
        {
            Skybox.Draw();
            Renderer.DrawSortedMeshes(MeshRegistry, MeshTransformations);

            Renderer.DrawLightPoints(PointLights);
        }

        public void Resize()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
