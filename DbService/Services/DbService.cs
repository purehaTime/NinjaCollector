using DbService.Interfaces;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcHelper.DbService;
using MongoDB.Bson;
using Filter = DbService.Models.Filter;
using ParserSettings = DbService.Models.ParserSettings;
using Post = DbService.Models.Post;
using Status = GrpcHelper.DbService.Status;
using FilterRequest = GrpcHelper.DbService.Filter;

namespace DbService.Services
{
    public class DbService : Database.DatabaseBase
    {
        private ISettingsService _settings;
        private IPostService _post;
        private IImageService _image;
        private IHistoryService _history;
        private IUserService _userService;

        public DbService(ISettingsService settings, IPostService post, IImageService image, IHistoryService history, IUserService userService)
        {
            _settings = settings;
            _post = post;
            _image = image;
            _history = history;
            _userService = userService;
        }

        public override async Task<Status> AddUser(AddUserModel request, ServerCallContext context)
        {
            var result = await _userService.CreateUser(request.UserName, request.Password);
            return new Status { Success = result };
        }

        public override async Task<UserModel> GetUser(UserRequest request, ServerCallContext context)
        {
            var user = await _userService.GetUser(request.UserName);
            if (user != null)
            {
                return new UserModel
                {
                    Id = user.Id.ToString(),
                    UserName = user.UserName,
                    Created = user.Created.ToTimestamp(),
                    Password = user.HashPassword
                };
            }
            return new UserModel();
        }

        public override async Task<Status> AddPost(GrpcHelper.DbService.Post post, ServerCallContext context)
        {
            var result = await _post.SavePost(new Post
            {
                PostId = post.PostId,
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

            return new Status { Success = result };
        }

        public override async Task<Status> AddPosts(PostModel posts, ServerCallContext context)
        {
            var result = await _post.SavePosts(posts);

            return new Status { Success = result };
        }

        public override async Task<Status> SaveParserSettings(ParserSettingsModel request, ServerCallContext context)
        {
            var result = await _settings.SaveParserSettings(new ParserSettings
            {
               Description = request.Description,
               Source = request.Source,
               ByUpdate = request.ByUpdate,
               Timeout = request.Timeout,
               Id = string.IsNullOrEmpty(request.Id) ? ObjectId.GenerateNewId() : ObjectId.Parse(request.Id),
               LastLoadPostId = request.LastPostId,
               StartFromLastLoadPost = request.StartFromPastPost,
               JobInterval = request.JobInterval,
               PostsCount = request.PostsCount,
               Tags = request.Tags,
               FromDate = request.FromDate.ToDateTime(),
               Group = request.Group,
               Hold = request.Hold,
               UntilDate = request.UntilDate.ToDateTime(),
               Disabled = request.Disabled,
               ContinueMonitoring = request.ContinueMonitoring,
               Filters = FilterRequestMapping(request.Filters)
            });
            
            return new Status
            {
                Success = result
            };
        }

        public override async Task<GrpcHelper.DbService.ParserSettings> GetParserSettings(ParserSettingsRequest request, ServerCallContext context)
        {
            ObjectId? settingsId = string.IsNullOrEmpty(request.SettingsId)
                ? null
                : ObjectId.Parse(request.SettingsId);

            var response = await _settings.GetParserSettings(request.Source, settingsId);

            return new GrpcHelper.DbService.ParserSettings
            {
                 ParserSetting = { response.Select(s => new ParserSettingsModel
                 {
                     Description = s.Description,
                     Id = s.Id.ToString(),
                     ByUpdate = s.ByUpdate,
                     Group = s.Group,
                     Timeout = s.Timeout,
                     JobInterval = s.JobInterval,
                     LastPostId = s.LastLoadPostId,
                     PostsCount = s.PostsCount,
                     Source = s.Source,
                     StartFromPastPost = s.StartFromLastLoadPost,
                     Tags = { s.Tags },
                     Hold = s.Hold,
                     FromDate = Timestamp.FromDateTime(s.FromDate),
                     UntilDate = Timestamp.FromDateTime(s.UntilDate),
                     Disabled = s.Disabled,
                     ContinueMonitoring = s.ContinueMonitoring,
                     Filters = FilterMapping(s.Filters),
                 } ) }
            };
        }

        private Filter FilterRequestMapping(FilterRequest filter)
        {
            var result = filter == null ? null :
                new Filter
                {
                    IgnoreRepost = filter.IgnoreRepost,
                    IgnoreVideo = filter.IgnoreVideo,
                    IgnoreAuthors = filter.IgnoreAuthors.ToList(),
                    IgnoreDescriptions = filter.IgnoreDescriptions.ToList(),
                    IgnoreTitles = filter.IgnoreTitles.ToList(),
                    IgnoreWords = filter.IgnoreWords.ToList(),
                };

            return result;
        }

        private FilterRequest FilterMapping(Filter filter)
        {
            return new FilterRequest
            {
                IgnoreRepost = filter.IgnoreRepost,
                IgnoreVideo = filter.IgnoreVideo,
                IgnoreAuthors = { filter.IgnoreAuthors },
                IgnoreDescriptions = { filter.IgnoreDescriptions },
                IgnoreTitles = { filter.IgnoreTitles },
                IgnoreWords = { filter.IgnoreWords },
            };
        }
    }
}
