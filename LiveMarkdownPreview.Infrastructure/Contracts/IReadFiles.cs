using System;
using System.Threading.Tasks;
using NiceTry;

namespace LiveMarkdownPreview.Infrastructure.Contracts
{
    public interface IReadFiles
    {
        Task<Try<string>> From(string fullPathName);
    }

    public interface IDownloadFiles
    {
        Task<Try<string>> From(Uri downloadUrl);
    }

    public interface IWriteFiles
    {
        Task With(string fullPathName, string content);
    }
}