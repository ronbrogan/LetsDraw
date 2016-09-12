using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Core;
using LetsDraw.Managers;
using LetsDraw.Rendering;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace LetsDraw
{
    static class Program
    {
        //public static GameModels models;
        public static IListener scene;

        public static GameWindow game;

        [STAThread]
        static void Main(string[] args)
        {
            //Setup
            InitializeEngine();

            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));

            

            //Render
            game.Run(60, 60);

            game.Dispose();
        }

        public static void InitializeEngine()
        {
            game = new GameWindow(720, 480, GraphicsMode.Default);
            game.Title = "Test Engine v0.1";
            //game.CursorVisible = false;

            //models = new GameModels();
            scene = new SceneManager(game.Size);

            game.Load += (sender, e) =>
            {
                game.VSync = VSyncMode.Off;

                GL.Enable(EnableCap.DebugOutput);
                GL.Enable(EnableCap.Blend);
                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.Lighting);
                GL.Enable(EnableCap.Light0);
                GL.Light(LightName.Light0, LightParameter.Diffuse, Color4.Red);
                GL.Light(LightName.Light0, LightParameter.Position, new [] { 3f, 3f, 3f });

            };

            game.Resize += (sender, e) =>
            {
                GL.Viewport(0, 0, game.Width, game.Height);
                scene.NotifyResize(game.Width, game.Height, 0, 0);
            };

            game.UpdateFrame += Update;

            game.RenderFrame += Render;
        }

        public static void Update(object sender, FrameEventArgs e)
        {
            if (game.Keyboard[Key.Escape])
                CloseGame();

            game.KeyDown += InputManager.NotifyKeyDown;
            game.KeyUp += InputManager.NotifyKeyUp;
            game.MouseMove += InputManager.NotifyMouse;
            game.MouseDown += InputManager.NotifyMouseDown;
            game.MouseUp += InputManager.NotifyMouseUp;
        }

        public static void Render(object sender, FrameEventArgs e)
        {
            scene.NotifyBeginFrame(e.Time);
            // render graphics
            scene.NotifyDisplayFrame();

            scene.NotifyEndFrame(game);
            //Console.Write("\r " + game.RenderFrequency.ToString("0.0") + " fps     ");
        }

        public static void CloseGame()
        {
            game.Exit();
            scene.Dispose();
        }

    }
}
