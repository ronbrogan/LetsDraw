using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneComposer.MenuServices
{
    public class FileService : INotifyPropertyChanged
    {
        public List<string> RecentFiles { get; set; }
        private string recentFileStoragePath { get; }
        private FileSystemWatcher recentWatcher { get; }
        private static object _recentFileLock = new object();

        public event FileEventHandler OnFileOpen;
        public event PropertyChangedEventHandler PropertyChanged;

        public FileService(string recentFileStoragePath)
        {
            Path.GetFullPath(recentFileStoragePath);

            this.recentFileStoragePath = Path.GetFullPath(recentFileStoragePath); ;

            recentWatcher = new FileSystemWatcher(Path.GetDirectoryName(this.recentFileStoragePath), Path.GetFileName(this.recentFileStoragePath));
            recentWatcher.Changed += (sender, e) =>
            {
                LoadRecentFiles();
            };

            LoadRecentFiles();

            recentWatcher.EnableRaisingEvents = true;
        }

        public void OpenFile(string path)
        {
            var eventArgs = new FileEventArgs(path);

            OnFileOpen?.Invoke(this, eventArgs);

            AddRecentFile(path);
        }

        private void AddRecentFile(string path)
        {
            if (RecentFiles.Contains(path))
                return;

            RecentFiles.Insert(0, path);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecentFiles)));

            if (RecentFiles.Count > 6)
                RecentFiles.RemoveRange(5, RecentFiles.Count - 6);

            FlushRecentFiles();
        }

        private void LoadRecentFiles()
        {
            lock(_recentFileLock)
            {
                recentWatcher.EnableRaisingEvents = false;

                RecentFiles = new List<string>();

                using (var recentFs = new FileStream(recentFileStoragePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                using (var recentReader = new StreamReader(recentFs))
                {
                    string readLine = null;
                    while((readLine = recentReader.ReadLine()) != null)
                    {
                        RecentFiles.Add(readLine);
                    }
                }

                recentWatcher.EnableRaisingEvents = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecentFiles)));
            }
        }

        private void FlushRecentFiles()
        {
            lock (_recentFileLock)
            {
                recentWatcher.EnableRaisingEvents = false;
                File.WriteAllLines(recentFileStoragePath, RecentFiles);
                recentWatcher.EnableRaisingEvents = true;
            }
        }
    }

    public delegate void FileEventHandler(object sender, FileEventArgs e);

    public class FileEventArgs : EventArgs
    {
        public string Path { get; }

        public FileEventArgs(string Path)
        {
            this.Path = Path;
        }

    }
}
