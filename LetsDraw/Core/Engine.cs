using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace LetsDraw.Core
{
    public class Engine
    {
        //public static GameModels models;
        public IListener scene;

        public GameWindow game;

        public Engine()
        {
            game = new GameWindow(720, 480, new GraphicsMode(32, 24, 0, 8))
            {
                Title = "Test Engine v0.1"
            };

            scene = new SceneManager(game.Size);

            game.Load += (sender, e) =>
            {
                game.VSync = VSyncMode.Off;

                GL.Enable(EnableCap.DebugOutput);
                GL.Enable(EnableCap.Blend);
                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.Lighting);
                GL.Enable(EnableCap.Light0);
                GL.Enable(EnableCap.Multisample);
            };

            game.Resize += (sender, e) =>
            {
                GL.Viewport(0, 0, game.Width, game.Height);
                scene.NotifyResize(game.Width, game.Height, 0, 0);
            };

            game.UpdateFrame += Update;

            game.RenderFrame += Render;

            game.KeyDown += InputManager.NotifyKeyDown;
            game.KeyUp += InputManager.NotifyKeyUp;
            game.MouseMove += InputManager.NotifyMouse;
            game.MouseDown += InputManager.NotifyMouseDown;
            game.MouseUp += InputManager.NotifyMouseUp;
        }


        public void Start()
        {
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
            scene.NotifyBeginFrame(e.Time);
            // render graphics
            scene.NotifyDisplayFrame();

            scene.NotifyEndFrame(game);
            Console.Write("\r " + game.RenderFrequency.ToString("0.0") + " fps     ");
        }

        private void CloseGame()
        {
            game.Exit();
            scene.Dispose();
        }
    }
}
