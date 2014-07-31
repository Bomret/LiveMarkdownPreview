using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LiveMarkdownPreview.Infrastructure.Contracts;
using LiveMarkdownPreview.Infrastructure.Types;
using Newtonsoft.Json;
using NiceTry;

namespace LiveMarkdownPreview.Infrastructure.Activities
{
    public sealed class CssFiles
    {
        private readonly string _cssConfig;
        private readonly string _cssFolder;
        private readonly IDownloadFiles _downloadFile;
        private readonly IReadFiles _readFile;
        private readonly IWriteFiles _writeFile;

        private CssFiles(IReadFiles readFile, IWriteFiles writeFile, IDownloadFiles downloadFile, string cssFolder,
                         string cssConfig)
        {
            _readFile = readFile;
            _writeFile = writeFile;
            _downloadFile = downloadFile;
            _cssFolder = cssFolder;
            _cssConfig = cssConfig;
        }

        public static CssFiles Create(IReadFiles readFile, IWriteFiles writeFile, IDownloadFiles downloadFile)
        {
            var appFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var cssFolder = Path.Combine(appFolder, "css");
            var cssConfig = Path.Combine(cssFolder, "availableCss.json");

            return new CssFiles(readFile, writeFile, downloadFile, cssFolder, cssConfig);
        }

        public async Task<Try<string>> GetContent(Css css)
        {
            var fileName = GetFileName(css);
            var cssFile = Path.Combine(_cssFolder, fileName);

            if (File.Exists(cssFile))
                return await _readFile.From(cssFile);

            var downloadFile = await _downloadFile.From(css.DownloadUrl);
            if (downloadFile.IsFailure) return downloadFile;

            await _writeFile.With(cssFile, downloadFile.Value);

            return Try.Success(downloadFile.Value);
        }

        private static string GetFileName(Css css)
        {
            return string.Format("{0}.{1}.css", css.Name, css.Version).ToLowerInvariant();
        }

        public async Task<IEnumerable<Css>> Load()
        {
            var getJson = await _readFile.From(_cssConfig);

            return getJson.Map(JsonConvert.DeserializeObject<IEnumerable<Css>>)
                          .GetOrElse(Enumerable.Empty<Css>());
        }

        public async Task Replace(IEnumerable<Css> newCssFiles)
        {
            var json = JsonConvert.SerializeObject(newCssFiles);

            await _writeFile.With(_cssConfig, json);
        }
    }
}