using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Rendering;
using LetsDraw.Rendering.Skyboxes;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace LetsDraw.Managers
{
    public class SceneManager : IListener, IDisposable
    {
        private ModelManager modelManager;
        private HudManager hudManager;

        private FpCamera firstPersonCamera;
        private Skybox skybox;

        public SceneManager(Size screenSize)
        {
            ShaderManager.CreateShader("TexturedShader", "Rendering/Shaders/Textured/vertexShader.glsl", "Rendering/Shaders/Textured/fragmentShader.glsl");
            ShaderManager.CreateShader("SphereShader", "Rendering/Shaders/Sphere/vertexShader.glsl", "Rendering/Shaders/Sphere/fragmentShader.glsl");
            ShaderManager.CreateShader("HudShader", "Rendering/Shaders/HUD/hudVertex.glsl", "Rendering/Shaders/HUD/hudFragment.glsl");

            skybox = new Skybox("Rendering/Shaders/Skybox/vertexShader.glsl", "Rendering/Shaders/Skybox/fragmentShader.glsl", "Rendering/Skyboxes/Skybox01/texture.png");

            firstPersonCamera = new FpCamera(new Vector3(80, 290, 30));

            hudManager = new HudManager(screenSize);
            Console.Write("Initializing Models...");
            modelManager = new ModelManager();
            Console.WriteLine("Done.");
        }

        public void NotifyBeginFrame(double deltaTime)
        {
            skybox.Update(deltaTime);
            firstPersonCamera.UpdateCamera(deltaTime);
            modelManager.Update(deltaTime);
            hudManager.Update(deltaTime);
        }

        public void NotifyDisplayFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.LightSlateGray);

            skybox.Draw(firstPersonCamera.GetProjectionMatrix(), firstPersonCamera.GetViewMatrix());

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
            ShaderManager.Dispose();
            modelManager.Dispose();
        }
    }
}
