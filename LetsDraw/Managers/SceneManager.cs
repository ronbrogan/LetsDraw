using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Rendering;
using LetsDraw.World;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace LetsDraw.Managers
{
    public class SceneManager : IListener
    {
        private HudManager hudManager;

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

        public void NotifyBeginFrame(double deltaTime)
        {
            scene.Update(deltaTime);

            hudManager.Update(deltaTime);
        }

        public void NotifyDisplayFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            scene.Draw();
            hudManager.Draw();
        }

        public void NotifyEndFrame(GameWindow game)
        {
            game.SwapBuffers();
        }

        public void NotifyResize(int width, int height, int prevWidth, int prevHeight)
        {
            scene?.Camera.UpdateProjectionMatrix(width, height);
        }


        public void Dispose()
        {
            ShaderManager.Dispose();
            scene.Dispose();
        }
    }
}
