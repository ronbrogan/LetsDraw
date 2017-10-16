using System;
using Foundation.Managers;
using Foundation.World;
using LetsDraw.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Foundation;

namespace LetsDraw
{
    class Program
    {
        private static GameWindow game;

        [STAThread]
        static void Main(string[] args)
        {
            var readout = new ReadoutSubscriber();

            var msaaSamples = 8;

            game = new GameWindow(1600, 900, new GraphicsMode(32, 24, 0, msaaSamples))
            {
                Title = "LetsDrawEngine"
            };

            var engine = new Engine(game.Size);

            AttachGameWindow(engine, game);

            engine.SubscribeToSceneChanges(readout);

            var scene = SceneFactory.FromFile(@"Scenes\powerhouse.json");

            engine.LoadScene(scene);

            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));

            engine.Start();
        }

        private static void AttachGameWindow(Engine engine, GameWindow gameWindow)
        {
            var callback = (DebugProc)Delegate.CreateDelegate(typeof(DebugProc), engine.GetType().GetMethod(nameof(Engine.DebugCallbackF)));

            gameWindow.Load += (sender, e) =>
            {
                gameWindow.VSync = VSyncMode.Off;

                GL.DebugMessageCallback(callback, (IntPtr.Zero));

                GL.Enable(EnableCap.DebugOutput);
                GL.Enable(EnableCap.Blend);
                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.Multisample);
                GL.Enable(EnableCap.CullFace);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            };

            engine.StartCallback += Engine_StartCallback;
            engine.SwapBuffers += Engine_SwapBuffers;

            gameWindow.Resize += engine.Resize;
            gameWindow.UpdateFrame += engine.Update;
            gameWindow.RenderFrame += engine.Render;


            gameWindow.KeyDown += InputManager.NotifyKeyDown;
            gameWindow.KeyUp += InputManager.NotifyKeyUp;
            gameWindow.MouseMove += InputManager.NotifyMouse;
            gameWindow.MouseDown += InputManager.NotifyMouseDown;
            gameWindow.MouseUp += InputManager.NotifyMouseUp;
        }

        private static void Engine_StartCallback(object sender, EventArgs e)
        {
            game.Run();

            game.Dispose();
        }

        private static void Engine_SwapBuffers(object sender, EventArgs e)
        {
            game.SwapBuffers();
        }
    }
}
