namespace RedditService.Interfaces
{
    public interface IFileDownloadService
    {
        Task<byte[]> GetFile(string link);
    }
}
