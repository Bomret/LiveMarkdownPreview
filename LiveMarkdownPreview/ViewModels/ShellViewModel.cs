using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LiveMarkdownPreview.Events;
using LiveMarkdownPreview.Infrastructure.Contracts;

namespace LiveMarkdownPreview.ViewModels
{
    [Export(typeof (ShellViewModel))]
    public sealed class ShellViewModel : Conductor<IScreen>,
                                         IHandle<FileSelected>

    {
        private readonly INotifyOnFileChanges _notifyOnFileChanges;
        private readonly PreviewViewModel _preview;
        private readonly IEventAggregator _pubSub;
        private IDisposable _notification;

        [ImportingConstructor]
        public ShellViewModel(FooterViewModel footer, SelectFileViewModel selectFile, PreviewViewModel preview,
                              INotifyOnFileChanges notifyOnFileChanges, IEventAggregator pubSub)
        {
            _pubSub = pubSub;
            pubSub.Subscribe(this);
            _preview = preview;
            _notifyOnFileChanges = notifyOnFileChanges;

            Footer = footer;

            ActivateItem(selectFile);
        }

        public FooterViewModel Footer { get; private set; }

        public void Handle(FileSelected message)
        {
            if (_notification != null)
                _notification.Dispose();

            _pubSub.PublishOnUIThread(new FileChanged(message.FullPath));

            _notification = _notifyOnFileChanges.For(message.FullPath, "*.md")
                                                .Subscribe(
                                                    ev => _pubSub.PublishOnUIThread(new FileChanged(ev.FullPath)),
                                                    err => Console.WriteLine(err.Message),
                                                    () => Console.WriteLine("File watcher disposed"));

            ActivateItem(_preview);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            if (close && _notification != null)
                _notification.Dispose();
        }
    }
}