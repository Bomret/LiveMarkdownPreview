namespace LiveMarkdownPreview.Infrastructure.Contracts
{
    public interface IParseMarkdown
    {
        string AsHtml(string markdown);
    }
}