using ModelsHelper.Models;
using RedditService.Interfaces;
using RedditService.Model;
using System.Collections.Concurrent;
using Content = ModelsHelper.Models.Post;
using Post = Reddit.Controllers.Post;

namespace RedditService.Services
{
    public class ParserService : IParserService
    {
        private readonly IParserGalleryService _galleryService;
        private readonly IFileDownloadService _fileDownloadService;
        private readonly IFilterService _filterService;

        public ParserService(IParserGalleryService galleryService, IFileDownloadService fileDownloadService, IFilterService filterService)
        {
            _galleryService = galleryService;
            _fileDownloadService = fileDownloadService;
            _filterService = filterService;
        }

        public async Task<Content> ParsePost(Post post, Filter filter)
        {
            if (!_filterService.IsValid(post, filter))
            {
                return null;
            }

            var images = await ParseImages(post);

            var content = new Content
            {
                PostId = post.Listing.Id,
                Images = images,
                PostDate = post.Created,
                Title = post.Title,
                Text = post.Listing.SelfText,
                UserName = post.Author,
                OriginalLink = post.Permalink,
                Source = post.Subreddit,
                Description = post.Listing.LinkFlairText
            };

            return content;
        }


        private async Task<List<Image>> ParseImages(Post post)
        {
            var imageLink = post.Listing.URL;

            var images = new ConcurrentBag<Image>();
            if (imageLink.Contains("reddit.com/gallery"))
            {
                var parsedImages = await _galleryService.GetImageLinks(imageLink);
                await Parallel.ForEachAsync(parsedImages, async (image, ct) =>
                {
                    var data = await _fileDownloadService.GetFile(image.DirectLink);
                    images.Add(new Image
                    {
                        DirectLink = imageLink,
                        Width = image.Width,
                        Height = image.Height,
                        Name = post.Fullname,
                        File = data,
                        ImageType = "unknow",
                        Description = image.Description,
                        Tags = new List<string> {"reddit", "image"}
                    });
                });
            }
            else if (post.Listing.Preview != null)
            {
                var preview = post.Listing.Preview;
                var source = preview.SelectToken("images.[*].source");
                var resultParse = source?.ToObject<PreviewImage>();

                if (resultParse != null)
                {
                    var imageData = await _fileDownloadService.GetFile(imageLink);
                    images.Add(new Image
                    {
                        DirectLink = imageLink,
                        Width = resultParse.Width,
                        Height = resultParse.Height,
                        Name = post.Fullname,
                        File = imageData,
                        ImageType = "unknow",
                        Description = resultParse.Url,
                        Tags = new List<string> { "reddit", "image" }
                    });
                }
            }

            return images.ToList();
        }
    }
}
