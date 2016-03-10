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

namespace LetsDrawATriangle.Managers
{
    public class SceneManager : IListener
    {
        private ShaderManager shaderManager;
        private ModelManager modelManager;
        private TextureLoader textureLoader;

        private Matrix4 ViewMatrix;
        private Matrix4 ProjectionMatrix;

        public SceneManager()
        {
            GL.Enable(EnableCap.DepthTest);

            shaderManager = new ShaderManager();
            shaderManager.CreateShader("CrateShader", "Rendering/Shaders/Textured/vertexShader.glsl", "Rendering/Shaders/Textured/fragmentShader.glsl");
            shaderManager.CreateShader("SphereShader", "Rendering/Shaders/Sphere/vertexShader.glsl", "Rendering/Shaders/Sphere/fragmentShader.glsl");

            textureLoader = new TextureLoader();

            ViewMatrix = new Matrix4
            (
                new Vector4(1, 0, 0, 0),
                new Vector4(0, 1, 0, 0),
                new Vector4(0, 0, -1, 0),
                new Vector4(0, 0, 10, 1)
            );

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

            modelManager.Draw(ProjectionMatrix, ViewMatrix);
        }

        public void NotifyEndFrame()
        {
            
        }

        public void NotifyResize(int width, int height, int prevWidth, int prevHeight)
        {
            float ar = (width / (float)height);
            var angle = 45.0f;
            var near1 = 0.1f;
            var far1 = 2000.0f;

            ProjectionMatrix[0,0] = 1.0f / (ar * (float)Math.Tan(angle / 2.0f));
            ProjectionMatrix[1,1] = 1.0f / (float)Math.Tan(angle / 2.0f);
            ProjectionMatrix[2,2] = (-near1 - far1) / (near1 - far1);
            ProjectionMatrix[2,3] = 1.0f;
            ProjectionMatrix[3,2] = 2.0f * near1 * far1 / (near1 - far1);
        }

        public void Dispose()
        {
            shaderManager.Dispose();
            modelManager.Dispose();
        }
    }
}
