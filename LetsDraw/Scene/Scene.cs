using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Core.Rendering;
using LetsDraw.Rendering.Skyboxes;
using OpenTK;

namespace LetsDraw.Rendering
{
    public class Scene
    {
        public ICamera Camera { get; set; }

        public Terrain Terrain { get; set; }

        public Skybox Skybox { get; set; }

        public Scene()
        {
            Skybox = new Skybox("Data/Shaders/Skybox/vertexShader.glsl", "Data/Shaders/Skybox/fragmentShader.glsl", "Rendering/Skyboxes/Skybox01/texture.png");

            Camera = new FpCamera(new Vector3(80, 290, 30));

            Terrain = new Terrain();
        }

        public void Load()
        {
            
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
            Terrain.Draw(proj, view);


        }

        public void Resize()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
