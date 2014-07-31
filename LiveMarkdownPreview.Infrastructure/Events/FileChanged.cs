using LiveMarkdownPreview.Infrastructure.Activities;
using LiveMarkdownPreview.Infrastructure.Contracts;

namespace LiveMarkdownPreview.Infrastructure.Events
{
    public sealed class FileChanged : FileEvent
    {
        public FileChanged(string fullPath, string name) : base(fullPath, name)
        {
        }
    }
}