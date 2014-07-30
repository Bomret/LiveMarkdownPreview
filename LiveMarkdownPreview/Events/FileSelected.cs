namespace LiveMarkdownPreview.Events
{
    public sealed class FileSelected
    {
        public FileSelected(string fileName)
        {
            FullPath = fileName;
        }

        public string FullPath { get; private set; }
    }
}