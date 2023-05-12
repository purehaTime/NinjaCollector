using Google.Protobuf.WellKnownTypes;
using GrpcHelper.DbService;
using MongoDB.Bson;
using ParserSettings = DbService.Models.ParserSettings;

namespace DbService.Mapping
{
    public static class ParserSettingsMapping
    {
        public static ParserSettings ToDatabase(this ParserSettingsModel model)
        {
            return new ParserSettings
            {
                Id = string.IsNullOrEmpty(model.Id) ? ObjectId.GenerateNewId() : ObjectId.Parse(model.Id),
                Description = model.Description,
                Source = model.Source,
                Group = model.Group,
                Timeout = model.Timeout,
                Hold = model.Hold,
                Counts = model.Counts,
                RetryAfterErrorCount = model.RetryAfterErrorCount,
                Tags = model.Tags.ToList(),
                FromDate = model.FromDate.ToDateTime(),
                UntilDate = model.FromDate.ToDateTime(),
                FromPostId = model.FromPostId,
                UntilPostId = model.UntilPostId,
                ByLastPostId = model.ByLastPostId,
                ContinueMonitoring = model.ContinueMonitoring,
                Disabled = model.Disabled,
                IgnoreVideo = model.Filter.IgnoreVideo,
                IgnoreRepost = model.Filter.IgnoreRepost,
                IgnoreDescriptions = model.Filter.IgnoreDescriptions.ToList(),
                IgnoreAuthors = model.Filter.IgnoreAuthors.ToList(),
                IgnoreTitles = model.Filter.IgnoreTitles.ToList(),
                IgnoreWords = model.Filter.IgnoreWords.ToList()
            };
        }

        public static ParserSettingsModel ToGrpcData(this ParserSettings model)
        {
            return new ParserSettingsModel
            {
                Id = model.Id.ToString(),
                Description = model.Description,
                Source = model.Source,
                Group = model.Group,
                Timeout = model.Timeout,
                Hold = model.Hold,
                Counts = model.Counts,
                RetryAfterErrorCount = model.RetryAfterErrorCount,
                Tags = { model.Tags },
                FromDate = Timestamp.FromDateTime(model.FromDate),
                UntilDate = Timestamp.FromDateTime(model.FromDate),
                FromPostId = model.FromPostId,
                UntilPostId = model.UntilPostId,
                ByLastPostId = model.ByLastPostId,
                ContinueMonitoring = model.ContinueMonitoring,
                Disabled = model.Disabled,
                Filter = new Filter 
                {
                    IgnoreVideo = model.IgnoreVideo,
                    IgnoreRepost = model.IgnoreRepost,
                    IgnoreDescriptions = { model.IgnoreDescriptions },
                    IgnoreAuthors = { model.IgnoreAuthors.ToList() },
                    IgnoreTitles = { model.IgnoreTitles.ToList() },
                    IgnoreWords = { model.IgnoreWords.ToList() }
                }
            };
        }
    }
}
