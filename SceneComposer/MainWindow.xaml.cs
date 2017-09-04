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
        private Engine engine;
        private DateTime lastMeasure;

        private ApplicationState appState;

        private Scene defaultScene;

        private FileService fileService;

        public MainWindow()
        {
            fileService = new FileService(ConfigurationManager.AppSettings[Constants.Configuration.RecentFilesStoragePath]);
            appState = new ApplicationState();

            Resources.Add("ApplicationStateData", appState);
            Resources.Add("FileService", fileService);

            InitializeComponent();

            engine = this.RenderWindow.CreateEngine();


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

            await RenderWindow.Dispatcher.InvokeAsync(() =>
            {
                engine.LoadScene(defaultScene);
            });

            appState.IsLoading = false;
            appState.StatusBarText = "Ready";

            editTabControl.DataContext = defaultScene;
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


        private void LoadSceneryFromFile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
