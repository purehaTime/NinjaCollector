using RedditService.Interfaces;
using RedditService.Model;

namespace RedditService.Services
{
    public class RedditService : IRedditService
    {
        private readonly IRedditApiClient _apiClient;
        private readonly IParserGalleryService _galleryService;
        private readonly IFileDownloadService _fileDownload;
        public RedditService(IRedditApiClient apiClient, IParserGalleryService galleryService, IFileDownloadService fileDownload)
        {
            _apiClient = apiClient;
            _galleryService = galleryService;
            _fileDownload = fileDownload;
        }
        public async Task<Content> GetLastPost(string subReddit)
        {
            var post = await _apiClient.GetLastPost(subReddit);

            var imageLink = post.Listing.URL;

            var images = new List<Image>();
            if (imageLink.Contains("reddit.com/gallery"))
            {
                var links = await _galleryService.GetImageLinks(imageLink);
                

            }
            else if (imageLink.Contains("i.redd.it"))
            {
                var imageData = await _fileDownload.GetFile(imageLink);
            }

            return new Content();
        }
    }

    //postHint : image
    //URl -> link to image
    // https://www.reddit.com/gallery/mrhsyw
    //post type SelfPost or LinkPost
}
