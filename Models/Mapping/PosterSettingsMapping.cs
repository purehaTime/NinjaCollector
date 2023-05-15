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
                Id = model.Id,
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
                ContinuePosting = model.ContinuePosting,
                Disabled = model.Disabled
            };
        }

        public static PosterSettings ToModel(this PosterSettingsModel model)
        {
            return new PosterSettings
            {
                Id = model.Id,
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
                ContinuePosting = model.ContinuePosting,
                Disabled = model.Disabled
            };
        }
    }

}
