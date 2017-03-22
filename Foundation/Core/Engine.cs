using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Core.Managers;
using Core.World;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Core.Core
{
    public class Engine
    {
        private ShaderManager shaderManager;
        private SceneManager sceneManager;
        private List<ISceneChangeSubscriber> sceneChangeSubscribers = new List<ISceneChangeSubscriber>();

        private GameWindow game;

        public static void DebugCallbackF(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            if (severity == DebugSeverity.DebugSeverityNotification)
                return;

            string msg = Marshal.PtrToStringAnsi(message, length);
            Console.WriteLine(msg);
        }

        public Engine()
        {
            var callback = (DebugProc)Delegate.CreateDelegate(typeof(DebugProc), GetType().GetMethod(nameof(DebugCallbackF)));

            var msaaSamples = 8;

            game = new GameWindow(1600, 900, new GraphicsMode(32, 24, 0, msaaSamples))
            {
                Title = "LetsDrawEngine"
            };

            shaderManager = new ShaderManager();
            sceneManager = new SceneManager(game.Size);

            game.Load += (sender, e) =>
            {
                game.VSync = VSyncMode.Off;

                GL.DebugMessageCallback(callback, (IntPtr.Zero));

                GL.Enable(EnableCap.DebugOutput);
                GL.Enable(EnableCap.Blend);
                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.Multisample);
                GL.Enable(EnableCap.CullFace);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                
            };

            game.Resize += (sender, e) =>
            {
                GL.Viewport(0, 0, game.Width, game.Height);
                sceneManager.NotifyResize(game.Width, game.Height, 0, 0);
            };

            game.UpdateFrame += Update;

            game.RenderFrame += Render;

            game.KeyDown += InputManager.NotifyKeyDown;
            game.KeyUp += InputManager.NotifyKeyUp;
            game.MouseMove += InputManager.NotifyMouse;
            game.MouseDown += InputManager.NotifyMouseDown;
            game.MouseUp += InputManager.NotifyMouseUp;
        }

        public void SubscribeToSceneChanges(ISceneChangeSubscriber sub)
        {
            sceneChangeSubscribers.Add(sub);
        }

        public void Start()
        {
            sceneManager.Load(new Scene());

            game.Run();

            game.Dispose();
        }

        private void Update(object sender, FrameEventArgs e)
        {
            // need to move this to input handler.
            if (game.Keyboard[Key.Escape])
                CloseGame();
        }

        private void Render(object sender, FrameEventArgs e)
        {
            sceneManager.NotifyBeginFrame(e.Time);

            foreach(var sub in sceneChangeSubscribers)
            {
                sub.Update(sceneManager.scene);
            }

            // render graphics
            sceneManager.NotifyDisplayFrame();

            sceneManager.NotifyEndFrame(game);
        }

        private void CloseGame()
        {
            game.Exit();
            sceneManager.Dispose();
        }
    }
}
