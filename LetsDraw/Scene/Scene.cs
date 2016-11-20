using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Core.Rendering;
using LetsDraw.Managers;
using LetsDraw.Rendering.Skyboxes;
using Vector3 = OpenTK.Vector3;

namespace LetsDraw.Rendering
{
    public class Scene
    {
        public Vector3 SpawnPoint = new Vector3(80, 290, 30);

        private Dictionary<int, List<Mesh>> MeshRegistry = new Dictionary<int, List<Mesh>>();

        public ICamera Camera { get; set; }

        public Terrain Terrain { get; set; }

        public Skybox Skybox { get; set; }

        public Scene()
        {
            
        }

        public void Load()
        {
            Skybox = new Skybox("Rendering/Skyboxes/Skybox01/texture.png");

            Camera = new FpCamera(SpawnPoint);

            Terrain = new Terrain();

            Renderer.AddAndSortMeshes(MeshRegistry, Terrain.Meshes);
            
        }

        public void Unload()
        {
            Skybox.Destroy();
        }

        public void Update(double time)
        {
            Skybox.Update(time);
            Camera.UpdateCamera(time);

            var proj = Camera.GetProjectionMatrix();
            var view = Camera.GetViewMatrix();

            Renderer.SetMatricies(view, proj);
        }

        public void Draw()
        {
            Skybox.Draw();
            Renderer.DrawSortedMeshes(MeshRegistry, Matrix4x4.Identity);
        }

        public void Resize()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
