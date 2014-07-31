using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using LiveMarkdownPreview.Events;
using LiveMarkdownPreview.Infrastructure.Contracts;
using NiceTry;

namespace LiveMarkdownPreview.ViewModels
{
    [Export(typeof (PreviewViewModel))]
    public sealed class PreviewViewModel : Screen,
                                           IHandleWithTask<FileSelected>,
                                           IHandleWithTask<FileChanged>
    {
        private readonly IParseMarkdown _parseMarkdown;
        private readonly IReadFiles _readFile;
        private string _html;

        [ImportingConstructor]
        public PreviewViewModel(FooterViewModel footer, IReadFiles readFile, IParseMarkdown parseMarkdown,
                                IEventAggregator pubSub)
        {
            pubSub.Subscribe(this);

            _readFile = readFile;
            _parseMarkdown = parseMarkdown;

            Footer = footer;
        }

        public FooterViewModel Footer { get; private set; }

        public string Html
        {
            get { return _html; }
            set
            {
                _html = value;
                NotifyOfPropertyChange(() => Html);
            }
        }

        public async Task Handle(FileChanged message)
        {
            var getMarkdown = await _readFile.From(message.FullPath);

            getMarkdown.Map(md => _parseMarkdown.AsHtml(md))
                       .Apply(html => Html = html);
        }

        public async Task Handle(FileSelected message)
        {
            var getMarkdown = await _readFile.From(message.FullPath);

            getMarkdown.Map(md => _parseMarkdown.AsHtml(md))
                       .Apply(html => Html = html);
        }
    }
}