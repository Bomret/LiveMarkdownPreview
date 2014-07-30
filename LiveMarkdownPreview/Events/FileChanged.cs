namespace LiveMarkdownPreview.Events
{
    public sealed class FileChanged
    {
        public FileChanged(string fullPath)
        {
            FullPath = fullPath;
        }

        public string FullPath { get; private set; }
    }
}