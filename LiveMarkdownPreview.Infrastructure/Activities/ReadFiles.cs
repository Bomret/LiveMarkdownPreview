using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using LiveMarkdownPreview.Infrastructure.Contracts;
using NiceTry;

namespace LiveMarkdownPreview.Infrastructure.Activities
{
    public sealed class ReadFiles : IReadFiles
    {
        public async Task<Try<string>> From(string fullPathName)
        {
            return await ReadFile(fullPathName);
        }

        private static async Task<Try<string>> ReadFile(string fullPath)
        {
            try
            {
                using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs))
                {
                    var content = await reader.ReadToEndAsync();

                    return Try.Success(content);
                }
            }
            catch (Exception err)
            {
                return Try.Failure(err);
            }
        }
    }

    public sealed class DownloadFiles : IDownloadFiles
    {
        public async Task<Try<string>> From(Uri downloadUrl)
        {
            return await ReadFile(downloadUrl);
        }

        private static async Task<Try<string>> ReadFile(Uri fullPath)
        {
            try
            {
                using (var http = new HttpClient())
                {
                    var content = await http.GetStringAsync(fullPath);

                    return Try.Success(content);
                }
            }
            catch (Exception err)
            {
                return Try.Failure(err);
            }
        }
    }

    public sealed class WriteFiles : IWriteFiles
    {
        public async Task With(string fullPathName, string content)
        {
            using (var fs = new FileStream(fullPathName, FileMode.OpenOrCreate))
            {
                using (var witer = new StreamWriter(fs))
                {
                    await witer.WriteAsync(content);
                }
            }
        }

        private static async Task<Try<string>> ReadFile(string fullPath)
        {
            try
            {
                using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs))
                {
                    var content = await reader.ReadToEndAsync();

                    return Try.Success(content);
                }
            }
            catch (Exception err)
            {
                return Try.Failure(err);
            }
        }
    }
}