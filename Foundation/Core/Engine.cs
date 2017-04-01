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
        public bool IsPaused { get; private set; }
        private bool Running = false;

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

        public void Start()
        {
            if (Running)
                return;

            if(!sceneManager.HasScene)
                sceneManager.Load(new Scene());

            StartCallback?.Invoke(this, null);

            Running = true;
            IsPaused = false;
        }

        public void LoadScene(Scene scene)
        {
            sceneManager.Load(scene);
        }

        public Scene GetScene()
        {
            return sceneManager.GetScene();
        }

        public void SubscribeToSceneChanges(ISceneChangeSubscriber sub)
        {
            if (IsPaused)
                throw new Exception("Cannot subscribe to scene changes while Engine is paused.");

            sceneManager.SubscribeToSceneChanges(sub);
        }

        public void Update(object sender, FrameEventArgs e)
        {
            if (IsPaused)
                return;

        }

        public void Pause()
        {
            IsPaused = true;

            sceneManager.AttempHudUpdate("Paused");

            sceneManager.NotifyDisplayFrame();

            sceneManager.NotifyEndFrame(SwapBuffers);
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void Render(object sender, FrameEventArgs e)
        {
            if (IsPaused)
                return;

            sceneManager.NotifyBeginFrame(e.Time);

            sceneManager.NotifyDisplayFrame();

            sceneManager.NotifyEndFrame(SwapBuffers);
        }

        public void Resize(object sender, EventArgs e)
        {
            if (IsPaused)
                return;

            // TODO refactor this to not use reflection
            if (sender.GetType().GetProperty("ClientSize") == null)
            {
                throw new Exception("Size property expected on event sender.");
            }

            var size = (Size)sender.GetType().GetProperty("ClientSize").GetValue(sender);

            GL.Viewport(size);
            sceneManager.NotifyResize(size.Width, size.Height);
        }
    }
}
