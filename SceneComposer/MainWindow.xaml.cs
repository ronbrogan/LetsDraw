﻿using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Foundation.World;
using Newtonsoft.Json;
using SceneComposer.MenuServices;
using System.Configuration;
using Foundation;
using Core.Serialization;
using Foundation.World.Serialization;
using System.Windows.Input;

namespace SceneComposer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Engine engine;

        private ApplicationState appState;

        private FileService fileService;

        public MainWindow()
        {
            fileService = new FileService(ConfigurationManager.AppSettings[Constants.Configuration.RecentFilesStoragePath]);
            appState = new ApplicationState();

            Resources.Add("ApplicationStateData", appState);
            Resources.Add("FileService", fileService);

            InitializeComponent();

            engine = this.RenderWindow.CreateEngine();

            var emptyScene = new Scene();

            engine.LoadScene(emptyScene);
            UpdateSceneDataContexts(emptyScene);

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

        private void UpdateSceneDataContexts(Scene scene)
        {
            editTabControl.DataContext = scene;
        }

        #region File Menu

        private void NewScene_Click(object sender, RoutedEventArgs e)
        {
            var emptyScene = new Scene();

            engine.LoadScene(emptyScene);
            UpdateSceneDataContexts(emptyScene);
        }

        // Method for loading a default scene for debugging purposes
        private async void LoadDefaultScene_Click(object sender, RoutedEventArgs e)
        {
            var defaultScene = SceneFactory.BuildDefaultScene();

            appState.IsLoading = true;
            appState.StatusBarText = "Loading Default Scene";

            await RenderWindow.Dispatcher.InvokeAsync(() =>
            {
                engine.LoadScene(defaultScene);
            });

            appState.IsLoading = false;
            appState.StatusBarText = "Ready";

            UpdateSceneDataContexts(defaultScene);
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

            UpdateSceneDataContexts(engine.GetScene());
        }

        private void loadScene_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            appState.StatusBarText = (string)e.UserState;
        }

        private void loadScene_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            worker.ReportProgress(0, "Parsing File");
            
            var serializer = new SceneSerializer();
            var scene = serializer.DeserializeScene((string)e.Argument);

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

            var serializer = new SceneSerializer();
            serializer.SerializeScene(scene, Environment.CurrentDirectory);

            
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion


        private void LoadSceneryFromFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}
