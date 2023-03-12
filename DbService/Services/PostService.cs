using System.Collections.Concurrent;
using DbService.Interfaces;
using GrpcHelper.DbService;
using MongoDB.Bson;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;
using Post = DbService.Models.Post;
using PosterSettings = DbService.Models.PosterSettings;

namespace DbService.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IImageService _imageService;
        private readonly IHistoryService _historyService;

        private ILogger _logger;

        public PostService(IRepository<Post> postRepository, IImageService imageService, IHistoryService historyService, ILogger logger)
        {
            _postRepository = postRepository;
            _imageService = imageService;
            _historyService = historyService;
            _logger = logger;
        }

        public async Task<Post> GetPostByTags(List<string> tags, PosterSettings poster)
        {
            var filter = Builders<Post>.Filter.AnyIn(x => x.Tags, tags);
            var posts = (await _postRepository.FindMany(filter, null!, CancellationToken.None)).ToList();

            var histories = await _historyService.GetHistory(posts.Select(s => s.Id), poster.Service, poster.ForGroup);
            var filterPost = posts.FirstOrDefault(w => histories.All(a => a.EntityId != w.Id));

            return filterPost!;
        }

        public async Task<bool> SavePost(Post post)
        {
            var result = await _postRepository.Insert(post, null!, CancellationToken.None);

            if (!result)
            {
                _logger.Error($"Can't save post from {post.GroupName}");
            }

            return result;
        }

        public async Task<bool> SavePosts(PostModel posts)
        {
            var dbPosts = new List<Post>();

            foreach (var post in posts.Posts)
            {
                var imageBag = new ConcurrentBag<ObjectId>();
                var images = post.Images.ToList();
                await Parallel.ForEachAsync(images, async (image, token) =>
                {
                    var savedImage = await _imageService.SaveImage(image.File.ToByteArray(),
                        image.Description,
                        image.Tags.ToList(),
                        image.OriginalLink,
                        image.Width,
                        image.Height);

                    if (savedImage.Status)
                    {
                        imageBag.Add(savedImage.SavedImage);
                    }
                });

                dbPosts.Add(new Post
                {
                    Description = post.Description,
                    GroupName = post.Group,
                    Id = ObjectId.GenerateNewId(),
                    Source = post.Source,
                    Images = imageBag,
                    Title = post.Title,
                    UserName = post.UserName,
                    OriginalLink = post.OriginalLink,
                    Tags = post.Tags,
                    Text = post.Text,
                    PostDate = post.PostDate.ToDateTime(),
                    PostId = post.PostId
                });
            }
            
            
            var result = await _postRepository.InsertMany(dbPosts, null!, CancellationToken.None);

            if (!result)
            {
                _logger.Error("Can't save posts in SavePosts method");
            }

            return result;
        }
    }
}
