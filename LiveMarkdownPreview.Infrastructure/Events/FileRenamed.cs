using LiveMarkdownPreview.Infrastructure.Activities;
using LiveMarkdownPreview.Infrastructure.Contracts;

namespace LiveMarkdownPreview.Infrastructure.Events
{
    public sealed class FileRenamed : FileEvent
    {
        public FileRenamed(string fullPath, string name, string oldFullPath, string oldName) : base(fullPath, name)
        {
            OldFullPath = oldFullPath;
            OldName = oldName;
        }

        public string OldFullPath { get; private set; }
        public string OldName { get; private set; }
    }
}