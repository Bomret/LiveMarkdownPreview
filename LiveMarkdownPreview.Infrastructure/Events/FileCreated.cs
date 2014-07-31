using LiveMarkdownPreview.Infrastructure.Activities;
using LiveMarkdownPreview.Infrastructure.Contracts;

namespace LiveMarkdownPreview.Infrastructure.Events
{
    public sealed class FileCreated : FileEvent
    {
        public FileCreated(string fullPath, string name) : base(fullPath, name)
        {
        }
    }
}