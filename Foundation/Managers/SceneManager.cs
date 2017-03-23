using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation.Core;
using Foundation.World;
using OpenTK.Graphics.OpenGL;

namespace Foundation.Managers
{
    public class SceneManager : IListener
    {
        private HudManager hudManager;
        private List<ISceneChangeSubscriber> sceneChangeSubscribers = new List<ISceneChangeSubscriber>();

        public Scene scene { get; private set; }

        public SceneManager(Size screenSize)
        {
            hudManager = new HudManager(screenSize);
        }

        public void Load(Scene newScene)
        {
            scene?.Unload();

            scene = newScene;
            scene.Load();
        }

        public void SubscribeToSceneChanges(ISceneChangeSubscriber sub)
        {
            sceneChangeSubscribers.Add(sub);
        }

        public void NotifyBeginFrame(double deltaTime)
        {
            scene.Update(deltaTime);

            hudManager.Update(deltaTime);

            foreach (var sub in sceneChangeSubscribers)
            {
                sub.Update(scene);
            }
        }

        public void NotifyDisplayFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            scene.Draw();
            hudManager.Draw();
        }

        public void NotifyEndFrame(EventHandler ev)
        {
            ev?.Invoke(this, null);
        }

        public void NotifyResize(int width, int height, int prevWidth, int prevHeight)
        {
            scene?.Camera.UpdateProjectionMatrix(width, height);
            hudManager.Resize(width, height);
        }


        public void Dispose()
        {
            ShaderManager.Dispose();
            scene.Dispose();
        }
    }
}
