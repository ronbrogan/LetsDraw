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

        private Matrix4 ProjectionMatrix;
        private Matrix4 ViewMatrix;

        public SceneManager()
        {
            ViewMatrix = new Matrix4
            (
                new Vector4(1, 0, 0, 0),
                new Vector4(0, 1, 0, 0),
                new Vector4(0, 0, -1, 0),
                new Vector4(0, 0, 10, 1)
            );

            shaderManager = new ShaderManager();
            shaderManager.CreateShader("CrateShader", "Rendering/Shaders/Textured/vertexShader.glsl", "Rendering/Shaders/Textured/fragmentShader.glsl");
            shaderManager.CreateShader("SphereShader", "Rendering/Shaders/Sphere/vertexShader.glsl", "Rendering/Shaders/Sphere/fragmentShader.glsl");

            textureLoader = new TextureLoader();

            firstPersonCamera = new FpCamera(new Vector3(0, 0, 10));

            modelManager = new ModelManager(shaderManager, textureLoader);
        }

        public void NotifyBeginFrame()
        {
            modelManager.Update();
        }

        public void NotifyDisplayFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.LightSlateGray);

            modelManager.Draw(firstPersonCamera.GetProjectionMatrix(), firstPersonCamera.GetViewMatrix());
        }

        public void NotifyEndFrame()
        {
            throw new NotImplementedException();
        }

        public void NotifyResize(int width, int height, int prevWidth, int prevHeight)
        {
            firstPersonCamera.UpdateProjectionMatrix(width, height);
        }

        public void NotifyKey(object sender, KeyboardKeyEventArgs e)
        {
            firstPersonCamera.KeyPressed(e.Key);
        }

        public void NotifyMouse(object sender, MouseMoveEventArgs e)
        {
            firstPersonCamera.MouseMove(e.Position.X, e.Position.Y);
        }

        public void NotifyMouseDown(object sender, MouseButtonEventArgs e)
        {
            firstPersonCamera.MousePressed(e.Position.X, e.Position.Y);
        }

        public void NotifyMouseUp(object sender, MouseButtonEventArgs e)
        {
            firstPersonCamera.MouseReleased();
        }


    public void Dispose()
        {
            shaderManager.Dispose();
            modelManager.Dispose();
        }
    }
}
