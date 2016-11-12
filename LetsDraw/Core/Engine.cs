using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsDraw.Managers;
using LetsDraw.Rendering;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace LetsDraw.Core
{
    public class Engine
    {
        public ShaderManager shaderManager;
        public SceneManager scene;

        public GameWindow game;

        public Engine()
        {
            var msaaSamples = 8;

            game = new GameWindow(1600, 900, new GraphicsMode(32, 24, 0, msaaSamples))
            {
                Title = "LetsDrawEngine"
            };

            shaderManager = new ShaderManager();
            scene = new SceneManager(game.Size);
            
            game.Load += (sender, e) =>
            {
                game.VSync = VSyncMode.Off;

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
            scene.Load(new Scene());

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
        }

        private void CloseGame()
        {
            game.Exit();
            scene.Dispose();
        }
    }
}
