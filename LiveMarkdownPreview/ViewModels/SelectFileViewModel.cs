using System.ComponentModel.Composition;
using Caliburn.Micro;
using LiveMarkdownPreview.Events;
using Microsoft.Win32;

namespace LiveMarkdownPreview.ViewModels
{
    [Export(typeof (SelectFileViewModel))]
    public sealed class SelectFileViewModel : Screen
    {
        private readonly IEventAggregator _pubSub;

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
            if (!result.HasValue || !result.Value) return;

            _pubSub.PublishOnUIThread(new FileSelected(loadFileDialog.FileName));
        }
    }
}