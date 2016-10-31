using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Rendering;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace LetsDraw.Managers
{
    public class SceneManager : IListener, IDisposable
    {
        private ShaderManager shaderManager;
        private ModelManager modelManager;
        private HudManager hudManager;

        private FpCamera firstPersonCamera;

        public SceneManager(Size screenSize)
        {
            shaderManager = new ShaderManager();
            shaderManager.CreateShader("TexturedShader", "Rendering/Shaders/Textured/vertexShader.glsl", "Rendering/Shaders/Textured/fragmentShader.glsl");
            shaderManager.CreateShader("SphereShader", "Rendering/Shaders/Sphere/vertexShader.glsl", "Rendering/Shaders/Sphere/fragmentShader.glsl");
            shaderManager.CreateShader("HudShader", "Rendering/Shaders/HUD/hudVertex.glsl", "Rendering/Shaders/HUD/hudFragment.glsl");

            firstPersonCamera = new FpCamera(new Vector3(0, 60, 10));

            hudManager = new HudManager(screenSize, shaderManager);
            Console.Write("Initializing Models...");
            modelManager = new ModelManager(shaderManager);
            Console.WriteLine("Done.");
        }

        public void NotifyBeginFrame(double deltaTime)
        {
            firstPersonCamera.UpdateCamera(deltaTime);
            modelManager.Update(deltaTime);
            hudManager.Update(deltaTime);
        }

        public void NotifyDisplayFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.LightSlateGray);

            modelManager.Draw(firstPersonCamera.GetProjectionMatrix(), firstPersonCamera.GetViewMatrix());
            hudManager.Draw();
        }

        public void NotifyEndFrame(GameWindow game)
        {
            game.SwapBuffers();
        }

        public void NotifyResize(int width, int height, int prevWidth, int prevHeight)
        {
            firstPersonCamera.UpdateProjectionMatrix(width, height);
        }


        public void Dispose()
        {
            shaderManager.Dispose();
            modelManager.Dispose();
        }
    }
}
