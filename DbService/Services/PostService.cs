using DbService.Interfaces;
using DbService.Models;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

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
    }
}
