using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation.Core;
using Foundation.Rendering;
using Foundation.Rendering.HUD;
using Foundation.World;
using OpenTK.Graphics.OpenGL;

namespace Foundation.Managers
{
    public class SceneManager : IListener
    {
        private Scene scene { get; set; }
        private HudManager hudManager { get; set; }
        private List<ISceneChangeSubscriber> sceneChangeSubscribers = new List<ISceneChangeSubscriber>();
        private Size size { get; set; }

        public bool HasScene = false;

        public SceneManager(Size screenSize)
        {
            hudManager = new HudManager(screenSize);
            size = screenSize;
        }

        public void Load(Scene newScene)
        {
            scene?.Unload();
            HasScene = false;
            scene = newScene;
            scene.Load(size);
            HasScene = scene.Loaded;
        }

        public Scene GetScene()
        {
            return scene;
        }

        internal void SubscribeToSceneChanges(ISceneChangeSubscriber sub)
        {
            sceneChangeSubscribers.Add(sub);
        }

        public void NotifyBeginFrame(double deltaTime)
        {
            UpdateScene(deltaTime);

            hudManager.Update(deltaTime);

            foreach (var sub in sceneChangeSubscribers)
            {
                sub.Update(scene);
            }
        }

        public void NotifyDisplayFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            DrawScene();

            hudManager.Draw();
        }

        public void NotifyEndFrame(EventHandler ev)
        {
            ev?.Invoke(this, null);
        }

        public void NotifyResize(int width, int height)
        {
            scene?.Camera.UpdateProjectionMatrix(width, height);
            hudManager.Resize(width, height);
            size = new Size(width, height);
        }

        public void UpdateScene(double deltaTime)
        {
            scene.Skybox?.Update(deltaTime);

            if(scene.Camera != null)
            {
                scene.Camera.UpdateCamera(deltaTime);

                var proj = scene.Camera.GetProjectionMatrix();
                var view = scene.Camera.GetViewMatrix();

                //Renderer.AddPointLights(scene.PointLights);
                Renderer.SetMatricies(scene.Camera.Position, view, proj);
            }

            scene.Update(deltaTime);
        }

        public void DrawScene()
        {
            scene.Skybox?.Draw();
            scene.RenderQueue.Render();
            scene.Draw();
        }

        public void AttempHudUpdate(string textToDisplay)
        {
            var hudElem = hudManager.GetElement("FpsReadout") as FramesDisplay;

            hudElem.Update(textToDisplay);
            
        }

        public void Dispose()
        {
            ShaderManager.Dispose();
            scene.Dispose();
        }
    }
}
