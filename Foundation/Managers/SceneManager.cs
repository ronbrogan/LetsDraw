using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation.Rendering.HUD;
using Foundation.World;
using OpenTK.Graphics.OpenGL;
using Foundation.Physics;
using Core.Rendering;
using Core;
using Core.Loaders;

namespace Foundation.Managers
{
    public class SceneManager : IListener
    {
        private Scene scene { get; set; }
        private HudManager hudManager { get; set; }
        private List<ISceneChangeSubscriber> sceneChangeSubscribers = new List<ISceneChangeSubscriber>();
        private Size size { get; set; }

        private readonly IRenderer renderer;
        private readonly ITextureBinder textureBinder;

        private PhysicsEngine physics { get; set; }

        public bool HasScene = false;

        public SceneManager(Size screenSize, IRenderer renderer, ITextureBinder textureBinder)
        {
            hudManager = new HudManager(screenSize);
            size = screenSize;

            this.renderer = renderer;
            this.textureBinder = textureBinder;
        }

        public void Load(Scene newScene)
        {
            physics = new PhysicsEngine();

            HasScene = false;
            scene?.Dispose();
            scene = null;

            GC.Collect();
            
            scene = newScene;
            scene.Load(size, renderer, textureBinder);
            HasScene = scene.Loaded;

            foreach (var scenery in scene.Scenery)
                physics.RegisterCollidable(scenery);

            physics.RegisterCollidable(scene.Terrain);
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
            if (scene == null)
                return;

            scene.Skybox?.Update(deltaTime);

            physics.DoBroadPhase();

            if(scene.Camera != null)
            {
                scene.Camera.UpdateCamera(deltaTime);

                var proj = scene.Camera.GetProjectionMatrix();
                var view = scene.Camera.GetViewMatrix();

                //renderer.AddPointLights(scene.PointLights);
                renderer.SetMatricies(scene.Camera.Position, view, proj);
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

            hudElem?.Update(textToDisplay);
            
        }

        public void Dispose()
        {
            ShaderManager.Dispose();
            scene.Dispose();
        }
    }
}
