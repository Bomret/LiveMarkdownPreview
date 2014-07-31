using System;

namespace LiveMarkdownPreview.Infrastructure.Types
{
    public sealed class Css
    {
        public Css(string name, string version, Uri downloadUrl)
        {
            DownloadUrl = downloadUrl;
            Version = version;
            Name = name;
        }

        public string Name { get; private set; }
        public string Version { get; private set; }
        public Uri DownloadUrl { get; private set; }
    }
}