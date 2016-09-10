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
            game.Run(30);

            game.Dispose();
        }

        public static void InitializeEngine()
        {
            

            game = new GameWindow(720, 480, GraphicsMode.Default);
            game.Title = "Test Engine v0.1";

            //models = new GameModels();
            scene = new SceneManager(game.Size);

            game.Load += (sender, e) =>
            {
                game.VSync = VSyncMode.On;
                //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

                GL.Enable(EnableCap.DebugOutput);
                GL.Enable(EnableCap.Blend);
                GL.Enable(EnableCap.DepthTest);
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
            scene.NotifyBeginFrame();
            // render graphics
            scene.NotifyDisplayFrame();

            scene.NotifyEndFrame(game);
        }

        public static void CloseGame()
        {
            game.Exit();
            //models.Dispose();
            scene.Dispose();
        }

    }
}
