using DbService.Interfaces;
using DbService.Mapping;
using GrpcHelper.DbService;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Concurrent;
using System.Linq;
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
        private readonly IGridFsService _gridFsService;


        private ILogger _logger;

        public PostService(IRepository<DbPost> postRepository, 
            IImageService imageService,
            IHistoryService historyService,
            ISettingsService settingsService,
            IGridFsService gridFsService,
            ILogger logger)
        {
            _postRepository = postRepository;
            _imageService = imageService;
            _historyService = historyService;
            _settingsService = settingsService;
            _gridFsService = gridFsService;
            _logger = logger;
        }

        public async Task<Post> GetPostBySettingId(string settingId)
        {
            var setting = await _settingsService.GetPosterSetting(settingId);
            if (setting == null)
            {
                return new Post();
            }

            var filter = Builders<DbPost>.Filter.AnyIn(x => x.Tags, setting.Tags);
            var posts = (await _postRepository.FindMany(filter, null, CancellationToken.None)).ToList();

            if (posts.Count == 0)
            {
                return null;
            }

            if (!setting.IgnoreHistory)
            {
                _logger.Information($"Start getting history posts for: {setting.Id}");
                var histories = await _historyService.GetHistory(posts.Select(s => s.PostId), setting.Source, setting.Group);
                posts = posts.ExceptBy(histories.Select(s => s.EntityId), post => post.PostId).ToList();

                if (posts.Count == 0)
                {
                    _logger.Information("All posts was posted by history");
                    return null;
                }
            }

            var rnd = new Random();
            var filterPost = setting.UseRandom ? posts[rnd.Next(posts.Count - 1)] : posts.First();

            var resultImages = new List<GrpcHelper.DbService.Image>();
            foreach (var image in filterPost.Images)
            {
                var byteImage = await _gridFsService.GetFileAsBytes(image.GridFsId, null, CancellationToken.None);
                var mapped = image.ToGrpcData(byteImage, filterPost?.Tags);
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
                var savedImageId = await _imageService.SaveImage(image);
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
