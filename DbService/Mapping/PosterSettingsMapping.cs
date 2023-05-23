using GrpcHelper.DbService;
using MongoDB.Bson;
using PosterSettings =  DbService.Models.PosterSettings;

namespace DbService.Mapping
{
    public static class PosterSettingsMapping
    {
        public static PosterSettings ToDatabase(this PosterSettingsModel model)
        {
            return new PosterSettings
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
                UseScheduling = model.UseScheduling,
                ScheduleInterval = model.ScheduleInterval,
                UseRandom = model.UseRandom,
                IgnoreHistory = model.IgnoreHistory,
                UseImagesOnly = model.UseImagesOnly,
                UseSettingsText = model.UseSettingsText,
                TextForPost = model.TextForPost,
                ContinuePosting = model.ContinuePosting,
                Disabled = model.Disabled
            };
        }

        public static PosterSettingsModel ToGrpcData(this PosterSettings model)
        {
            return new PosterSettingsModel
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
                UseScheduling = model.UseScheduling,
                ScheduleInterval = model.ScheduleInterval,
                UseRandom = model.UseRandom,
                IgnoreHistory = model.IgnoreHistory,
                UseImagesOnly = model.UseImagesOnly,
                UseSettingsText = model.UseSettingsText,
                TextForPost = model.TextForPost,
                ContinuePosting = model.ContinuePosting,
                Disabled = model.Disabled
            };
        }
    }

}
