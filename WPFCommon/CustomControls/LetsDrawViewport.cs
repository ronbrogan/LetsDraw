using Foundation.Core;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Threading;
using InputManager = Foundation.Managers.InputManager;

namespace WPFCommon.CustomControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPFCommon"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPFCommon;assembly=WPFCommon"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:LetsDrawViewport/>
    ///
    /// </summary>
    public class LetsDrawViewport : WindowsFormsHost
    {
        private GLControl glControl;
        public Engine engine;

        private DateTime lastMeasure;
        private int msaaSamples = 8;

        public  LetsDrawViewport()
        {
            lastMeasure = DateTime.Now;
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LetsDrawViewport), new FrameworkPropertyMetadata(typeof(LetsDrawViewport)));

            glControl = new GLControl(new GraphicsMode(32, 24, 0, msaaSamples));
            glControl.CreateControl();
            glControl.MakeCurrent();

            glControl.Dock = DockStyle.Fill;
            
            this.Child = glControl;
        }

        public Engine CreateEngine() 
        {
            engine = new Engine(glControl.Size);

            glControl.VSync = false;

            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.CullFace);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.LineWidth(1);

            glControl.Resize += engine.Resize;
            engine.SwapBuffers += Engine_SwapBuffers;

            glControl.KeyDown += InputManager.NotifyKeyDown;
            glControl.KeyUp += InputManager.NotifyKeyUp;
            glControl.MouseMove += InputManager.NotifyMouse;
            glControl.MouseDown += InputManager.NotifyMouseDown;
            glControl.MouseUp += InputManager.NotifyMouseUp;

            glControl.Paint += glControl_Paint;

            engine.Start();

            return engine;
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            // Rendering in dispatch queue to allow UI updates
            this.Dispatcher.InvokeAsync(() =>
            {
                var now = DateTime.Now;
                var elapsed = (now - lastMeasure).TotalSeconds;
                if (elapsed == 0d)
                    elapsed = 0.000001;

                engine.Render(sender, new FrameEventArgs(elapsed));

                // Immediately invalidate state, force repaint ASAP
                ((GLControl)sender).Invalidate();

                lastMeasure = now;

            }, DispatcherPriority.Render);
        }

        private void Engine_SwapBuffers(object sender, EventArgs e)
        {
            glControl.SwapBuffers();
        }
    }
}
