using System.ComponentModel.Composition;
using System.IO;
using Caliburn.Micro;
using LiveMarkdownPreview.Events;
using Microsoft.Win32;

namespace LiveMarkdownPreview.ViewModels
{
    [Export(typeof (SelectFileViewModel))]
    public sealed class SelectFileViewModel : Screen
    {
        private readonly IEventAggregator _pubSub;
        private string _file;
        private FileSystemWatcher _watcher;

        [ImportingConstructor]
        public SelectFileViewModel(IEventAggregator pubSub)
        {
            _pubSub = pubSub;
        }


        public void LoadFile()
        {
            var loadFileDialog = new OpenFileDialog
            {
                Filter = "Markdown|*.md",
                Multiselect = false
            };

            var result = loadFileDialog.ShowDialog();
            if (!result.HasValue) return;

            _file = Path.GetFileName(loadFileDialog.FileName);
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(loadFileDialog.FileName));
            _watcher.Changed += FileChanged;
            _watcher.Created += FileCreated;
            _watcher.Renamed += FileRenamed;
            _watcher.EnableRaisingEvents = true;


            _pubSub.PublishOnUIThread(new FileSelected(loadFileDialog.FileName));
        }

        private void FileRenamed(object sender, RenamedEventArgs e)
        {
            if (e.Name != _file) return;

            _pubSub.PublishOnUIThread(new FileChanged(e.FullPath));
        }

        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            if (e.Name != _file) return;

            _pubSub.PublishOnUIThread(new FileChanged(e.FullPath));
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.Name != _file) return;

            _pubSub.PublishOnUIThread(new FileChanged(e.FullPath));
        }
    }
}