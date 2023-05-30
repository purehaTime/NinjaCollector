using RedditService.Interfaces;
using System.Collections.Concurrent;
using System.Web;
using ILogger = Serilog.ILogger;

namespace RedditService.Services
{
    public class FileDownloadService : IFileDownloadService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;


        public FileDownloadService(IHttpClientFactory clientFactory, ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<byte[]> GetFile(string link)
        {
            try
            {
                using var httpClient = _clientFactory.CreateClient("with_header");
                var bytes = await httpClient.GetByteArrayAsync(HttpUtility.HtmlDecode(link));
                return bytes;
            }
            catch (Exception err)
            {
                _logger.Error($"FileDownloadService.GetFile: {err.Message}");
                return null;
            }
        }

        public async Task<List<byte[]>> GetFiles(IEnumerable<string> links)
        {
            var result = new ConcurrentBag<byte[]>();
            {
                await Parallel.ForEachAsync(links, async (link, ct) =>
                {
                    try
                    {
                        using var httpClient = _clientFactory.CreateClient();
                        var bytes = await httpClient.GetByteArrayAsync(HttpUtility.HtmlDecode(link), ct);
                        result.Add(bytes);
                    }
                    catch (Exception err)
                    {
                        _logger.Error($"FileDownloadService.GetFiles parallel: {err.Message}");
                    }
                });
            }

            return result.ToList();
        }

        public async Task<string> GetData(string link)
        {
            try
            {
                using var httpClient = _clientFactory.CreateClient("with_header");
                var data = await httpClient.GetStringAsync(HttpUtility.HtmlDecode(link));
                return data;
            }
            catch (Exception err)
            {
                _logger.Error($"FileDownloadService.GetData: {err.Message}");
                return null;
            }
        }
    }
}
