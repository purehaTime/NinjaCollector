using GrpcHelper.DbService;
using PosterSettings = ModelsHelper.Models.PosterSettings;

namespace ModelsHelper.Mapping
{
    public static class PosterSettingsMapping
    {
        public static PosterSettingsModel ToGrpcData(this PosterSettings model)
        {
            return new PosterSettingsModel
            {
                Id = model.Id ?? string.Empty,
                Description = model.Description ?? string.Empty,
                Source = model.Source ?? string.Empty,
                Group = model.Group ?? string.Empty,
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
                TextForPost = model.TextForPost ?? string.Empty,
                ContinuePosting = model.ContinuePosting,
                Disabled = model.Disabled
            };
        }

        public static PosterSettings ToModel(this PosterSettingsModel model)
        {
            return new PosterSettings
            {
                Id = model.Id ?? string.Empty,
                Description = model.Description ?? string.Empty,
                Source = model.Source ?? string.Empty,
                Group = model.Group ?? string.Empty,
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
    }

}
