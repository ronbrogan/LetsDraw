using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDrawATriangle.Core;
using LetsDrawATriangle.Rendering;
using LetsDrawATriangle.Rendering.Models;
using OpenTK;
using Model = LetsDrawATriangle.Rendering.Models.Model;

namespace LetsDrawATriangle.Managers
{
    public class ModelManager : IDisposable
    {
        private Dictionary<string, Model> GameModels;
        private ShaderManager shaderManager;

        private TextureLoader textureLoader;

        public ModelManager(ShaderManager shaderManager, TextureLoader textureLoader)
        {
            this.shaderManager = shaderManager;
            this.textureLoader = textureLoader;
            GameModels = new Dictionary<string, Model>();

            //Test IndexedCube
            var cube = new IndexedCube();
            cube.SetShader(shaderManager.GetShader("CrateShader"));
            cube.SetTexture("crate", textureLoader.LoadTexture("Rendering/Textures/Crate.bmp"));
            cube.Create();
            GameModels.Add("Cube1", cube);

            //Sphere
            var sph = new Sphere();
            sph.SetShader(shaderManager.GetShader("SphereShader"));
            sph.SetTexture("BaseTexture", textureLoader.LoadTexture("Rendering/Textures/nebula1.png"));
            sph.SetTexture("SecondTexture", textureLoader.LoadTexture("Rendering/Textures/nebula2.png"));
            sph.SetTexture("ThirdTexture", textureLoader.LoadTexture("Rendering/Textures/nebula3.png"));
            sph.SetTexture("AlphaChanTexture", textureLoader.LoadTexture("Rendering/Textures/alphaChan.png"));
            sph.SetTexture("RampTexture", textureLoader.LoadTexture("Rendering/Textures/ramp.png"));
            sph.Create(1.2f, 24, 48);
            GameModels.Add("Sphere", sph);
        }

        public void Draw(Matrix4 ProjectionMatrix, Matrix4 ViewMatrix)
        {
            foreach(var model in GameModels.Values)
            {
                model.Draw(ProjectionMatrix, ViewMatrix);
            }
        }

        public void Update()
        {
            foreach(var model in GameModels.Values)
            {
                model.Update();
            }
        }

        public void DeleteModel(string ModelName)
        {
            if (!GameModels.ContainsKey(ModelName))
                return;

            var model = GameModels[ModelName];

            model.Dispose();

            GameModels.Remove(ModelName);
        }

        public Model GetModel(string ModelName)
        {
            if (GameModels.ContainsKey(ModelName))
                return GameModels[ModelName];

            return null;
        }

        public void Dispose()
        {
            foreach(var model in GameModels.Values)
            {
                model.Dispose();
            }
        }
    }
}
