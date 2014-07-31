namespace LiveMarkdownPreview.Infrastructure.Contracts
{
    public abstract class FileEvent
    {
        protected FileEvent(string fullPath, string name)
        {
            Name = name;
            FullPath = fullPath;
        }

        public string FullPath { get; private set; }
        public string Name { get; private set; }
    }
}