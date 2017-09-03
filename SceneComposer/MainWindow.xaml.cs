using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Foundation.Core;
using Foundation.World;
using Foundation.Serialization;
using Newtonsoft.Json;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using InputManager = Foundation.Managers.InputManager;
using SceneComposer.MenuServices;
using System.Configuration;

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

        private int msaaSamples = 8;

        private FileService fileService;

        public MainWindow()
        {
            fileService = new FileService(ConfigurationManager.AppSettings[Constants.Configuration.RecentFilesStoragePath]);
            appState = new ApplicationState();

            Resources.Add("ApplicationStateData", appState);
            Resources.Add("Scene", new Scene());
            Resources.Add("FileService", fileService);

            lastMeasure = DateTime.Now;

            InitializeComponent();
            InitializeGlControl();

            fileService.OnFileOpen += (_, fe) =>
            {
                var loadScene = new BackgroundWorker()
                {
                    WorkerReportsProgress = true
                };
                loadScene.DoWork += loadScene_DoWork;
                loadScene.ProgressChanged += loadScene_ProgressChanged;
                loadScene.RunWorkerCompleted += loadScene_RunWorkerCompleted;

                appState.IsLoading = true;

                engine.Pause();

                loadScene.RunWorkerAsync(fe.Path);
            };
        }

        private void InitializeGlControl()
        {
            glControl = new GLControl(new GraphicsMode(32, 24, 0, msaaSamples));
            glControl.CreateControl();
            glControl.MakeCurrent();

            engine = new Engine(glControl.Size);

            SetupEngine();
            BindEngineToControl();

            glControl.Dock = DockStyle.Fill;
            glControl.Paint += glControl_Paint;

            engine.Start();

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

                // Immediately invalidate state, force repaint ASAP
                ((GLControl)sender).Invalidate();

                lastMeasure = now;

            }, DispatcherPriority.Render);
        }

        #region File Menu

        private void NewScene_Click(object sender, RoutedEventArgs e)
        {
            engine.LoadScene(new Scene());
        }

        // Method for loading a default scene for debugging purposes
        private async void LoadDefaultScene_Click(object sender, RoutedEventArgs e)
        {
            defaultScene = SceneFactory.BuildDefaultScene();

            appState.IsLoading = true;
            appState.StatusBarText = "Loading Default Scene";

            await ForceUiUpdate();

            await RenderWindow.Dispatcher.InvokeAsync(() =>
            {
                engine.LoadScene(defaultScene);
            });

            appState.IsLoading = false;
            appState.StatusBarText = "Ready";
            Resources["Scene"] = defaultScene;
        }

        private void LoadScene_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Multiselect = false
            };

            var result = dialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            fileService.OpenFile(dialog.FileName);
        }

        private void LoadRecentScene_Click(object sender, RoutedEventArgs e)
        {
            var path = (string)((System.Windows.Controls.MenuItem)sender).Header;
            fileService.OpenFile(path);
        }

        private void loadScene_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            appState.IsLoading = false;

            engine.Resume();

            editTabControl.DataContext = engine.GetScene();
        }

        private void loadScene_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            appState.StatusBarText = (string)e.UserState;
        }

        private void loadScene_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            worker.ReportProgress(0, "Parsing File");

            Scene scene;

            using (var sceneFile = new FileStream((string)e.Argument, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader sr = new StreamReader(sceneFile))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                serializer.ContractResolver = new LetsDrawContractResolver();

                scene = serializer.Deserialize<Scene>(reader);
            }

            worker.ReportProgress(60, "Initializing Scene");

            RenderWindow.Dispatcher.Invoke(() =>
            {
                // This is currently still done on main thread, due to GLContext presence.
                engine.LoadScene(scene);
            });

            worker.ReportProgress(100, "Scene Loaded");
        }

        public void SaveScene_Click(object sender, RoutedEventArgs e)
        {
            var scene = engine.GetScene();

            var sceneData = JsonConvert.SerializeObject(scene);

            File.WriteAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "sceneoutput.json"), sceneData);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        private async Task ForceUiUpdate()
        {
            // TODO Not working reliably, dont use

            var frame = new DispatcherFrame();
            await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                frame.Continue = false;
            }, DispatcherPriority.Background);
            Dispatcher.PushFrame(frame);
        }
    }
}
