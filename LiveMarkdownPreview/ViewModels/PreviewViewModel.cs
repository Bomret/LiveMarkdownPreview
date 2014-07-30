using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Caliburn.Micro;
using LiveMarkdownPreview.Events;
using MarkdownSharp;

namespace LiveMarkdownPreview.ViewModels
{
    [Export(typeof (PreviewViewModel))]
    public sealed class PreviewViewModel : Screen,
        IHandle<FileChanged>,
        IHandle<FileSelected>
    {
        private readonly Markdown _markdown;
        private string _html;

        [ImportingConstructor]
        public PreviewViewModel(IEventAggregator pubSub)
        {
            pubSub.Subscribe(this);

            _markdown = new Markdown();
        }


        public string Html
        {
            get { return _html; }
            set
            {
                _html = value;
                NotifyOfPropertyChange(() => Html);
            }
        }

        public async void Handle(FileChanged message)
        {
            var content = await ReadFile(message.FullPath);

            Html = _markdown.Transform(content);
        }

        public async void Handle(FileSelected message)
        {
            var content = await ReadFile(message.FullPath);

            Html = _markdown.Transform(content);
        }

        private static async Task<string> ReadFile(string fullPath)
        {
            using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(fs))
                return await reader.ReadToEndAsync();
        }
    }
}