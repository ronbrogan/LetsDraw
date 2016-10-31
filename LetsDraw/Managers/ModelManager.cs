﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Rendering;
using LetsDraw.Rendering.Models;
using OpenTK;
using Model = LetsDraw.Rendering.Models.Model;

namespace LetsDraw.Managers
{
    public class ModelManager : IDisposable
    {
        private Dictionary<string, Model> GameModels;
        private ShaderManager shaderManager;

        public ModelManager(ShaderManager shaderManager)
        {
            this.shaderManager = shaderManager;
            GameModels = new Dictionary<string, Model>();

            var loaded = new LoadedModel();
            loaded.SetShader(shaderManager.GetShader("TexturedShader"));
            loaded.SetTexture("diffuse", TextureLoader.LoadTexture("Objects/TexturesCom_Grass0139_1_seamless_S.jpg"));
            loaded.Create();
            GameModels.Add("Loaded1", loaded);
        }

        public void Draw(Matrix4 ProjectionMatrix, Matrix4 ViewMatrix)
        {
            foreach(var model in GameModels.Values)
            {
                model.Draw(ProjectionMatrix, ViewMatrix);
            }
        }

        public void Update(double deltaTime)
        {
            foreach(var model in GameModels.Values)
            {
                model.Update(deltaTime);
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
