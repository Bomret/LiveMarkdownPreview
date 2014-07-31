using System;
using System.IO;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using LiveMarkdownPreview.Infrastructure.Contracts;
using LiveMarkdownPreview.Infrastructure.Events;

namespace LiveMarkdownPreview.Infrastructure.Activities
{
    public sealed class NotifyOnFileChanges : INotifyOnFileChanges
    {
        public IObservable<FileEvent> For(string fullFilePath, string extensionFilter)
        {
            return Observable.Create<FileEvent>(obs =>
            {
                var fileName = Path.GetFileName(fullFilePath);
                var dir = Path.GetDirectoryName(fullFilePath);

                var watcher = new FileSystemWatcher(dir, extensionFilter);

                var onCreated =
                    Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(h => watcher.Created += h,
                                                                                             h => watcher.Created -= h)
                              .Select<EventPattern<FileSystemEventArgs>, FileEvent>(
                                  args => new FileCreated(args.EventArgs.FullPath, args.EventArgs.Name));

                var onChanged =
                    Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(h => watcher.Changed += h,
                                                                                             h => watcher.Changed -= h)
                              .Select<EventPattern<FileSystemEventArgs>, FileEvent>(
                                  args => new FileChanged(args.EventArgs.FullPath, args.EventArgs.Name));

                var onRenamed =
                    Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(h => watcher.Renamed += h,
                                                                                       h => watcher.Renamed -= h)
                              .Select<EventPattern<RenamedEventArgs>, FileEvent>(
                                  ev =>
                                  new FileRenamed(ev.EventArgs.FullPath, ev.EventArgs.Name, ev.EventArgs.OldFullPath,
                                                  ev.EventArgs.OldName));

                var combined = onCreated.Merge(onChanged).Merge(onRenamed)
                                        .Where(ev => ev.Name == fileName)
                                        .Subscribe(obs);

                watcher.EnableRaisingEvents = true;

                return Disposable.Create(() =>
                {
                    combined.Dispose();
                    watcher.Dispose();
                });
            });
        }
    }
}