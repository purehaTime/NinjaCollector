using RedditService.Model;

namespace RedditService.Interfaces
{
    public interface IFileDownloadService
    {
        Task<byte[]> GetFile(string link);
        Task<List<byte[]>> GetFiles(IEnumerable<string> links);
        Task<string> GetData(string link);
    }
}
