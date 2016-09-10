using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDrawATriangle.Core;
using LetsDrawATriangle.Rendering;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace LetsDrawATriangle.Managers
{
    public class SceneManager : IListener
    {
        private ShaderManager shaderManager;
        private ModelManager modelManager;
        private TextureLoader textureLoader;

        private FpCamera firstPersonCamera;
        private TextElement hudText;


        public SceneManager(Size screenSize)
        {
            shaderManager = new ShaderManager();
            shaderManager.CreateShader("CrateShader", "Rendering/Shaders/Textured/vertexShader.glsl", "Rendering/Shaders/Textured/fragmentShader.glsl");
            shaderManager.CreateShader("SphereShader", "Rendering/Shaders/Sphere/vertexShader.glsl", "Rendering/Shaders/Sphere/fragmentShader.glsl");
            shaderManager.CreateShader("HudShader", "Rendering/Shaders/HUD/hudVertex.glsl", "Rendering/Shaders/HUD/hudFragment.glsl");

            textureLoader = new TextureLoader();

            firstPersonCamera = new FpCamera(new Vector3(0, 2, 10));

            modelManager = new ModelManager(shaderManager, textureLoader);
        }

        public void NotifyBeginFrame()
        {
            firstPersonCamera.UpdateCamera();
            modelManager.Update();
            
        }

        public void NotifyDisplayFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.LightSlateGray);

            modelManager.Draw(firstPersonCamera.GetProjectionMatrix(), firstPersonCamera.GetViewMatrix());
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
