using RedditService.Model;

namespace RedditService.Interfaces
{
    public interface IParserGalleryService
    {
        public Task<List<JsonImage>> GetImageLinks(string urlToPost);
    }
}
