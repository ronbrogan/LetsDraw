using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Core.Rendering;
using LetsDraw.Rendering.Skyboxes;

namespace LetsDraw.Rendering
{
    public class Scene
    {
        public ICamera Camera { get; set; }

        public RenderMesh Geometry { get; set; }

        public Skybox Skybox { get; set; }

        public Scene()
        {
            
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
            Camera.UpdateCamera(time);
        }

        public void Draw()
        {
            var projMat = Camera.GetProjectionMatrix();
            var viewMat = Camera.GetViewMatrix();

            Skybox.Draw(projMat, viewMat);

            

        }

        public void Resize()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
