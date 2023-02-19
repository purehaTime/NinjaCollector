using RedditService.Model;

namespace RedditService.Interfaces
{
    public interface IParserGalleryService
    {
        public Task<IEnumerable<Image>> GetImageLinks(string urlToGallery);
    }
}
