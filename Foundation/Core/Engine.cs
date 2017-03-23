using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Foundation.Managers;
using Foundation.World;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Foundation.Core
{
    public class Engine
    {
        private ShaderManager shaderManager;
        private SceneManager sceneManager;
        

        public event EventHandler StartCallback;
        public event EventHandler SwapBuffers;

        public static void DebugCallbackF(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            if (severity == DebugSeverity.DebugSeverityNotification)
                return;

            string msg = Marshal.PtrToStringAnsi(message, length);
            Console.WriteLine(msg);
        }

        public Engine(Size windowSize)
        {
            shaderManager = new ShaderManager();
            sceneManager = new SceneManager(windowSize);
        }

        public void SubscribeToSceneChanges(ISceneChangeSubscriber sub)
        {
            sceneManager.SubscribeToSceneChanges(sub);
        }

        public void Start()
        {
            sceneManager.Load(new Scene());

            StartCallback?.Invoke(this, null);
        }

        public void Update(object sender, FrameEventArgs e)
        {
            
        }

        public void Resize(object sender, EventArgs e)
        {
            // TODO refactor this to not use reflection
            if(sender.GetType().GetProperty("ClientSize") == null)
            {
                throw new Exception("Size property expected on event sender.");
            }

            var size = (Size)sender.GetType().GetProperty("ClientSize").GetValue(sender);

            GL.Viewport(size);
            sceneManager.NotifyResize(size.Width, size.Height);
        }

        public void Render(object sender, FrameEventArgs e)
        {
            sceneManager.NotifyBeginFrame(e.Time);

            // render graphics
            sceneManager.NotifyDisplayFrame();

            sceneManager.NotifyEndFrame(SwapBuffers);
        }
    }
}
