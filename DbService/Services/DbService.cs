using DbService.Interfaces;
using DbService.Models;
using Grpc.Core;
using GrpcHelper.DbService;
using MongoDB.Bson;
using ParserSettings = DbService.Models.ParserSettings;
using Status = GrpcHelper.DbService.Status;

namespace DbService.Services
{
    public class DbService : Database.DatabaseBase
    {
        private ISettingsService _settings;
        private IPostService _post;
        private IImageService _image;
        private IHistoryService _history;

        public DbService(ISettingsService settings, IPostService post, IImageService image, IHistoryService history)
        {
            _settings = settings;
            _post = post;
            _image = image;
            _history = history;
        }

        public override async Task<Status> AddPost(IAsyncStreamReader<PostModel> requestStream, ServerCallContext context)
        {
            var posts = requestStream.ReadAllAsync();
            var isSuccess = true;

            await Parallel.ForEachAsync(posts, async (post, ct) =>
            {
                var result = await _post.SavePost(new Post
                {
                    Description = post.Description,
                    GroupName = post.Group,
                    Images = null,
                    OriginalLink = post.OriginalLink,
                    UserName = post.UserName,
                    PostDate = post.PostDate.ToDateTime(),
                    Source = post.Source,
                    Tags = post.Tags,
                    Text = post.Text,
                    Title = post.Title,
                });

                isSuccess = result && isSuccess;
            });

            return new Status { Success = isSuccess };
        }

        public override async Task<Status> SaveParserSettings(ParserSettingsModel request, ServerCallContext context)
        {
            var result = await _settings.SaveParserSettings(new ParserSettings
            {
               Description = request.Description,
               Source = request.Source,
               ByUpdate = request.ByUpdate,
               Interval = request.Interval,
               Id = ObjectId.Parse(request.Id),
               LastLoadPostId = request.LastPostId,
               StartFromLastLoadPost = request.StartFromPastPost,
               JobInterval = request.JobInterval,
               PostsCount = request.PostsCount,
            });
            
            return new Status
            {
                Success = result
            };
        }

        public override async Task<GrpcHelper.DbService.ParserSettings> GetParserSettings(ParserSettingsRequest request, ServerCallContext context)
        {
            var response = await _settings.GetParserSettings(request.Source);

            return new GrpcHelper.DbService.ParserSettings
            {
                 ParserSetting = { response.Select(s => new ParserSettingsModel
                 {
                     Description = s.Description,
                     Id = s.Id.ToString(),
                     ByUpdate = s.ByUpdate,
                     Interval = s.Interval,
                     JobInterval = s.JobInterval,
                     LastPostId = s.LastLoadPostId,
                     PostsCount = s.PostsCount,
                     Source = s.Source,
                     StartFromPastPost = s.StartFromLastLoadPost,
                 } ) }
            };
        }
    }
}
