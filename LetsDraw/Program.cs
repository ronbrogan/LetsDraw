﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Core;
using Foundation.Managers;
using LetsDraw.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

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

            Console.WriteLine(GL.GetString(StringName.Version));
            Console.WriteLine(GL.GetString(StringName.Renderer));
            Console.WriteLine(GL.GetString(StringName.Vendor));
            Console.WriteLine(GL.GetString(StringName.ShadingLanguageVersion));

            engine.Start();
        }

        private static void AttachGameWindow(Engine engine, GameWindow game)
        {
            var callback = (DebugProc)Delegate.CreateDelegate(typeof(DebugProc), engine.GetType().GetMethod(nameof(Engine.DebugCallbackF)));

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

            engine.StartCallback += Engine_StartCallback;
            engine.SwapBuffers += Engine_SwapBuffers;

            game.Resize += engine.Resize;
            game.UpdateFrame += engine.Update;
            game.RenderFrame += engine.Render;


            game.KeyDown += InputManager.NotifyKeyDown;
            game.KeyUp += InputManager.NotifyKeyUp;
            game.MouseMove += InputManager.NotifyMouse;
            game.MouseDown += InputManager.NotifyMouseDown;
            game.MouseUp += InputManager.NotifyMouseUp;
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
