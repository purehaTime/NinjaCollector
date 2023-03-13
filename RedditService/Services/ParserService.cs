using Reddit.Controllers;
using RedditService.Interfaces;
using RedditService.Model;
using System.Collections.Concurrent;

namespace RedditService.Services
{
    public class ParserService : IParserService
    {
        private readonly IParserGalleryService _galleryService;
        private readonly IFileDownloadService _fileDownloadService;

        public ParserService(IParserGalleryService galleryService, IFileDownloadService fileDownloadService)
        {
            _galleryService = galleryService;
            _fileDownloadService = fileDownloadService;
        }

        public async Task<Content> ParsePost(Post post)
        {
            if (post.Listing.Domain == "v.redd.it") //ignore video posts
            {
                return null;
            }

            var images = await ParseImages(post);

            var content = new Content
            {
                Id = post.Listing.Id,
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
                    var data = await _fileDownloadService.GetFile(image.DirectLink);
                    images.Add(new ImageContainer
                    {
                        Data = data,
                        Image = image
                    });
                });
            }
            else if (post.Listing.Preview != null)
            {
                var preview = post.Listing.Preview;
                var source = preview.SelectToken("images.[*].source");
                var resultParse = source.ToObject<PreviewImage>();

                if (resultParse != null)
                {
                    var imageData = await _fileDownloadService.GetFile(imageLink);
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

            }

            return images.ToList();
        }
    }
}
