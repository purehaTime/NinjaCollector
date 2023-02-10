namespace RedditService.Interfaces
{
    public interface IRedditSession
    {
        public Task<string> GetAccessToken();
    }
}
