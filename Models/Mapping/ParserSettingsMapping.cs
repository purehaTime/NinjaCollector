using Google.Protobuf.WellKnownTypes;
using GrpcHelper.DbService;
using ParserSettings = Models.Models.ParserSettings;

namespace Models.Mapping
{
    public static class ParserSettingsMapping
    {
        public static ParserSettingsModel ToGrpcData(this ParserSettings model)
        {
            return new ParserSettingsModel
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
                FromDate = Timestamp.FromDateTime(model.FromDate),
                UntilDate = Timestamp.FromDateTime(model.FromDate),
                FromPostId = model.FromPostId,
                UntilPostId = model.UntilPostId,
                ByLastPostId = model.ByLastPostId,
                ContinueMonitoring = model.ContinueMonitoring,
                Disabled = model.Disabled,
                Filter = model.Filter.ToGrpcData()
            };
        }

        public static ParserSettings ToModel(this ParserSettingsModel model)
        {
            return new ParserSettings
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
                FromDate = model.FromDate.ToDateTime(),
                UntilDate = model.FromDate.ToDateTime(),
                FromPostId = model.FromPostId,
                UntilPostId = model.UntilPostId,
                ByLastPostId = model.ByLastPostId,
                ContinueMonitoring = model.ContinueMonitoring,
                Disabled = model.Disabled,
                Filter = model.Filter.ToModel()
            };
        }
    }

}
