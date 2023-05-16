using DbService.Interfaces;
using DbService.Mapping;
using GrpcHelper.DbService;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Concurrent;
using DbPost = DbService.Models.Post;
using ILogger = Serilog.ILogger;
using Image = DbService.Models.Image;
using Post = GrpcHelper.DbService.Post;

namespace DbService.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<DbPost> _postRepository;
        private readonly IImageService _imageService;
        private readonly IHistoryService _historyService;
        private readonly ISettingsService _settingsService;

        private ILogger _logger;

        public PostService(IRepository<DbPost> postRepository, IImageService imageService, IHistoryService historyService, ISettingsService settingsService, ILogger logger)
        {
            _postRepository = postRepository;
            _imageService = imageService;
            _historyService = historyService;
            _settingsService = settingsService;
            _logger = logger;
        }

        public async Task<Post> GetPostBySettingId(string settingId)
        {
            var setting = await _settingsService.GetPosterSetting(settingId);
            if (setting == null)
            {
                return null;
            }

            var filter = Builders<DbPost>.Filter.AnyIn(x => x.Tags, setting.Tags);
            var posts = (await _postRepository.FindMany(filter, null, CancellationToken.None)).ToList();

            var rnd = new Random();
            var filterPost = setting.UseRandom ? posts[rnd.Next(posts.Count - 1)] : posts.FirstOrDefault();
            if (!setting.IgnoreHistory)
            {
                var histories = await _historyService.GetHistory(posts.Select(s => s.Id), setting.Source, setting.Group);
                filterPost = posts.FirstOrDefault(w => histories.All(a => a.EntityId != w.Id));
            }

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
                var savedImageId = await _imageService.SaveImage(image.File.ToByteArray(), image);
                if (savedImageId.Status)
                {
                    images.Add(image.ToDatabase(savedImageId.SavedImage));
                }
            }
            var result = await _postRepository.Insert(post.ToDatabase(images), null, CancellationToken.None);

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
                    var savedImage = await _imageService.SaveImage(image.File.ToByteArray(), image);

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
