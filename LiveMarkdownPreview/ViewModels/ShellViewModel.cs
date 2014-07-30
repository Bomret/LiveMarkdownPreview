using System.ComponentModel.Composition;
using Caliburn.Micro;
using LiveMarkdownPreview.Events;

namespace LiveMarkdownPreview.ViewModels
{
    [Export(typeof (ShellViewModel))]
    public sealed class ShellViewModel : Conductor<IScreen>, IHandle<FileSelected>

    {
        private readonly PreviewViewModel _preview;

        [ImportingConstructor]
        public ShellViewModel(SelectFileViewModel selectFile, PreviewViewModel preview, IEventAggregator pubSub)
        {
            pubSub.Subscribe(this);
            _preview = preview;

            ActivateItem(selectFile);
        }

        public void Handle(FileSelected message)
        {
            ActivateItem(_preview);
        }
    }
}