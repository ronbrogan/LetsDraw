using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Foundation.Core;
using Foundation.World;
using LetsDraw;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SceneComposer.Properties;
using InputManager = Foundation.Managers.InputManager;

namespace SceneComposer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GLControl glControl;
        private Engine engine;
        private DateTime lastMeasure;

        private ApplicationState appState;

        private Scene defaultScene;

        public MainWindow()
        {
            appState = new ApplicationState();
            this.Resources.Add("ApplicationStateData", appState);

            lastMeasure = DateTime.Now;
            InitializeComponent();

            var msaaSamples = 8;

            glControl = new GLControl(new GraphicsMode(32, 24, 0, msaaSamples));
            glControl.CreateControl();
            glControl.MakeCurrent();

            engine = new Engine(glControl.Size);

            SetupEngine();
            BindEngineToControl();

            glControl.Dock = DockStyle.Fill;
            glControl.Paint += glControl_Paint;

            engine.Start();

            defaultScene = SceneFactory.BuildDefaultScene();

            this.RenderWindow.Child = glControl;
        }

        private void SetupEngine()
        {
            glControl.VSync = false;

            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.CullFace);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        private void BindEngineToControl()
        {
            glControl.Resize += engine.Resize;
            engine.SwapBuffers += Engine_SwapBuffers;

            glControl.KeyDown += InputManager.NotifyKeyDown;
            glControl.KeyUp += InputManager.NotifyKeyUp;
            glControl.MouseMove += InputManager.NotifyMouse;
            glControl.MouseDown += InputManager.NotifyMouseDown;
            glControl.MouseUp += InputManager.NotifyMouseUp;
        }

        private void Engine_SwapBuffers(object sender, EventArgs e)
        {
            glControl.SwapBuffers();
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            // Rendering in dispatch queue to allow UI updates
            RenderWindow.Dispatcher.InvokeAsync(() =>
            {
                var now = DateTime.Now;
                var elapsed = (now - lastMeasure).TotalSeconds;
                if (elapsed == 0d)
                    elapsed = 0.000001;

                engine.Render(sender, new FrameEventArgs(elapsed));
                ((GLControl)sender).Invalidate();
                lastMeasure = now;
            }, DispatcherPriority.Render);
        }

        #region File Menu

        private void NewScene_Click(object sender, RoutedEventArgs e)
        {
            engine.LoadScene(new Scene());
        }

        private async void LoadScene_Click(object sender, RoutedEventArgs e)
        {
            // Replace this with File dialog, and load scene from selection

            appState.IsLoading = true;
            appState.StatusBarText = "Loading Default Scene";

            await ForceUiUpdate();

            await RenderWindow.Dispatcher.InvokeAsync(() =>
            {
                engine.LoadScene(defaultScene);
            });

            appState.IsLoading = false;
            appState.StatusBarText = "Ready";
        }

        public void SaveScene_Click(object sender, RoutedEventArgs e)
        {
            // Serialize and save current scene to file
            
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        private async Task ForceUiUpdate()
        {
            var frame = new DispatcherFrame();
            await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                frame.Continue = false;
            }, DispatcherPriority.Render);
            Dispatcher.PushFrame(frame);
        }
    }
}
