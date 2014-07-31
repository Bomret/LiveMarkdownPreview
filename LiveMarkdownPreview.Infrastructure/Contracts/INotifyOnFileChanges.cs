using System;

namespace LiveMarkdownPreview.Infrastructure.Contracts
{
    public interface INotifyOnFileChanges
    {
        IObservable<FileEvent> For(string fullFilePath, string extensionFilter);
    }
}