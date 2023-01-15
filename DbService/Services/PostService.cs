using DbService.Interfaces;
using DbService.Models;
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

        public Task<Post> GetPostByTags(List<string> tags, PosterSettings poster)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SavePost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
