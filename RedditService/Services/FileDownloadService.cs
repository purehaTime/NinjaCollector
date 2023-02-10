using RedditService.Interfaces;
using RestSharp;

namespace RedditService.Services
{
    public class FileDownloadService : IFileDownloadService
    {
        private IRestClient _client;


        public FileDownloadService(IRestClient client)
        {
            _client = client;
        }

        public async Task<byte[]> GetFile(string link)
        {
            var request = new RestRequest(link);
            var data = await Task.Run(() => _client.DownloadData(request));
            return data;
        }

    }
}
