using LiveMarkdownPreview.Infrastructure.Contracts;
using MarkdownSharp;

namespace LiveMarkdownPreview.Infrastructure.Activities
{
    public sealed class ParseMarkdown : IParseMarkdown
    {
        private readonly Markdown _markdown;

        public ParseMarkdown()
        {
            _markdown = new Markdown();
        }

        public string AsHtml(string markdown)
        {
            return _markdown.Transform(markdown);
        }
    }
}