using ModelsHelper.Models;
using RedditService.Model;

namespace RedditService.Interfaces
{
    public interface IParserGalleryService
    {
        public Task<IEnumerable<ParsedImage>> GetImageLinks(string urlToGallery);
    }
}
