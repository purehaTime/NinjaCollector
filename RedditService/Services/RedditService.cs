using RedditService.Interfaces;
using RedditService.Model;
using System.Collections.Concurrent;
using Reddit.Controllers;

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

            var images = await ParseImages(post);

            var content = new Content
            {
                Images = images,
                Created = post.Created,
                Title = post.Title,
                Text = post.Listing.SelfText,
                UserName = post.Author,
                OriginalLink = post.Permalink,
                SubredditName = post.Subreddit,
                Description = post.Listing.LinkFlairText
            };

            return content;
        }

        private async Task<List<ImageContainer>> ParseImages(Post post)
        {
            var imageLink = post.Listing.URL;

            var images = new ConcurrentBag<ImageContainer>();
            if (imageLink.Contains("reddit.com/gallery"))
            {
                var parsedImages = await _galleryService.GetImageLinks(imageLink);
                await Parallel.ForEachAsync(parsedImages, async (image, ct) =>
                {
                    var data = await _fileDownload.GetFile(image.DirectLink);
                    images.Add(new ImageContainer
                    {
                        Data = data,
                        Image = image
                    });
                });
            }
            else if (imageLink.Contains("i.redd.it"))
            {
                var preview = post.Listing.Preview;
                var source = preview.SelectToken("images.[*].source");
                var resultParse = source.ToObject<PreviewImage>();
                var imageData = await _fileDownload.GetFile(imageLink);
                images.Add(new ImageContainer
                {
                    Data = imageData,
                    Image = new Image
                    {
                        DirectLink = imageLink,
                        Width = resultParse.Width,
                        Height = resultParse.Height,
                        Name = post.Fullname,
                    }
                });

            }

            return images.ToList();
        }
    }
}
