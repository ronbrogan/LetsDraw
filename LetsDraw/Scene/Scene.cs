using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Core.Rendering;
using LetsDraw.Managers;
using LetsDraw.Rendering.Skyboxes;
using OpenTK;

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
        }

        public void Draw()
        {
            var proj = Camera.GetProjectionMatrix();
            var view = Camera.GetViewMatrix();

            Skybox.Draw(proj, view);
            //Terrain.Draw(proj, view);
            Renderer.DrawSortedMeshes(MeshRegistry, Matrix4.Identity, view, proj);
        }

        public void Resize()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
