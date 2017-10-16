using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Foundation.Rendering;
using Newtonsoft.Json;
using System;
using Core.Rendering;
using Foundation.Rendering.Cameras;
using Core;
using System.Numerics;

namespace Foundation.World
{
    public class Scene : IDisposable
    {
        public bool Loaded = false;

        public ICamera Camera { get; set; }
        public Vector3 SpawnPoint { get; set; }

        public bool Textureless { get; set; }

        public Skybox Skybox { get; set; }
        public Terrain Terrain { get; set; }
        public List<StaticScenery> Scenery { get; set; }

        [JsonIgnore]
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
            Scenery = new List<StaticScenery>();
            SpawnPoint = new Vector3(0);
            Camera = new FpCamera(SpawnPoint);
        }

        public void Load(Size size, IRenderer renderer)
        {
            Loaded = false;

            RenderQueue = new RenderQueue(renderer);

            Skybox?.Load();

            if(Terrain != null)
                RenderQueue.Add(Terrain);

            foreach (var item in Scenery)
                RenderQueue.Add(item);

            foreach(var mat in RenderQueue.MeshRegistry.Values.SelectMany(m => m).Select(m => m.Material))
            {
                mat.DiffuseMap?.GenerateTexture();
                mat.AmbientMap?.GenerateTexture();
                mat.BumpMap?.GenerateTexture();
                mat.SpecularMap?.GenerateTexture();
                mat.SpecularHighlightMap?.GenerateTexture();
                mat.AlphaMap?.GenerateTexture();

            }

            Camera.UpdateProjectionMatrix(size.Width, size.Height);

            Loaded = true;
        }

        public void Unload()
        {
            
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Loaded = false;

                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    Skybox?.Dispose();
                    RenderQueue.Dispose();
                    Terrain?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                Scenery = null;
                Camera = null;


                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Scene() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
