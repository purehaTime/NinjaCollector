using DbService.Interfaces;
using DbService.Mapping;
using GrpcHelper.DbService;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Concurrent;
using DbPost = DbService.Models.Post;
using ILogger = Serilog.ILogger;
using Image = DbService.Models.Image;
using Post = GrpcHelper.DbService.Post;
using PosterSettings = DbService.Models.PosterSettings;

namespace DbService.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<DbPost> _postRepository;
        private readonly IImageService _imageService;
        private readonly IHistoryService _historyService;

        private ILogger _logger;

        public PostService(IRepository<DbPost> postRepository, IImageService imageService, IHistoryService historyService, ILogger logger)
        {
            _postRepository = postRepository;
            _imageService = imageService;
            _historyService = historyService;
            _logger = logger;
        }

        public async Task<Post> GetPostByTags(List<string> tags, PosterSettings poster)
        {
            var filter = Builders<DbPost>.Filter.AnyIn(x => x.Tags, tags);
            var posts = (await _postRepository.FindMany(filter, null!, CancellationToken.None)).ToList();

            var histories = await _historyService.GetHistory(posts.Select(s => s.Id), poster.Service, poster.ForGroup);
            var filterPost = posts.FirstOrDefault(w => histories.All(a => a.EntityId != w.Id));

            var images = await _imageService.GetImagesForPost(filterPost?.Id ?? ObjectId.Empty);

            var resultImages = new List<GrpcHelper.DbService.Image>();
            foreach (var dbImage in images)
            {
                var mapped = dbImage.image.ToGrpcData(dbImage.stream.ToArray(), filterPost?.Tags);
                resultImages.Add(mapped);
            }

            var result = filterPost.ToGrpcData(resultImages);
            return result;
        }

        public async Task<bool> SavePost(Post post)
        {
            var images = new List<Image>();
            foreach (var image in post.Images)
            {
                var savedImageId = await _imageService.SaveImage(image.File.ToByteArray(),
                    image.Description,
                    image.Tags?.ToList() ?? post.Tags.ToList(),
                    image.DirectLink,
                    image.Width,
                    image.Height);

                if (savedImageId.Status)
                {
                    images.Add(image.ToDatabase(savedImageId.SavedImage));
                }
            }
            var result = await _postRepository.Insert(post.ToDatabase(images), null!, CancellationToken.None);

            if (!result)
            {
                _logger.Error($"Can't save post from {post.Group}");
            }

            return result;
        }

        public async Task<bool> SavePosts(PostModel posts)
        {
            var dbPosts = new List<DbPost>();

            foreach (var post in posts.Posts)
            {
                var imageBag = new ConcurrentBag<Image>();
                var images = post.Images.ToList();
                await Parallel.ForEachAsync(images, async (image, token) =>
                {
                    var savedImage = await _imageService.SaveImage(image.File.ToByteArray(),
                        image.Description,
                        image.Tags.ToList(),
                        image.DirectLink,
                        image.Width,
                        image.Height);

                    if (savedImage.Status)
                    {
                        imageBag.Add(image.ToDatabase(savedImage.SavedImage));
                    }
                });
                dbPosts.Add(post.ToDatabase(imageBag.ToList()));
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
