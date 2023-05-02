using Google.Protobuf.WellKnownTypes;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using MainService.Interfaces;
using MainService.Models;
using ILogger = Serilog.ILogger;

namespace MainService.Services
{
    public class ParserService : IParserService
    {
        private readonly IDatabaseServiceClient _dbClient;
        private readonly ILogger _logger;

        public ParserService(IDatabaseServiceClient dbClient, ILogger logger)
        {
            _dbClient = dbClient;
            _logger = logger;
        }

        public async Task<bool> SaveParserSettings(ParserModel parserSettings)
        {
            try
            {
                var result = await _dbClient.SaveParserSettings(new ParserSettingsModel
                {
                    Id = parserSettings.Id,
                    Description = parserSettings.Description,
                    Source = parserSettings.Source,
                    Group = parserSettings.Group,
                    Timeout = parserSettings.Timeout,
                    Hold = parserSettings.Hold,
                    Counts = parserSettings.Counts,
                    RetryAfterErrorCount = parserSettings.RetryAfterErrorCount,
                    Tags = { parserSettings.Tags.Split("|") },
                    FromDate = Timestamp.FromDateTime(parserSettings.FromDate),
                    UntilDate = Timestamp.FromDateTime(parserSettings.UntilDate),
                    FromPostId = parserSettings.FromPostId,
                    UntilPostId = parserSettings.UntilPostId,
                    ByLastPostId = parserSettings.ByLastPostId,
                    ContinueMonitoring = parserSettings.ContinueMonitoring,
                    Disabled = parserSettings.Disabled,
                    Filters = new Filter
                    {
                        IgnoreVideo = parserSettings.IgnoreVideo,
                        IgnoreRepost = parserSettings.IgnoreRepost,
                        IgnoreAuthors = { parserSettings.IgnoreAuthors },
                        IgnoreTitles = { parserSettings.IgnoreTitles },
                        IgnoreWords = { parserSettings.IgnoreWords },
                        IgnoreDescriptions = { parserSettings.IgnoreDescriptions }
                    }
                });

                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return false;
        }

        public async Task<List<ParserModel>> GetAllParserSettings()
        {
            try
            {
                var result = await _dbClient.GetParserSettings(new ParserSettingsRequest());

                return result.Select(s => new ParserModel
                {
                    Id = s.Id,
                    Description = s.Description,
                    Source = s.Source,
                    Group = s.Group,
                    Timeout = s.Timeout,
                    Hold = s.Hold,
                    Counts = s.Counts,
                    RetryAfterErrorCount = s.RetryAfterErrorCount,
                    Tags = string.Join('|', s.Tags),
                    FromDate = s.FromDate.ToDateTime(),
                    UntilDate = s.UntilDate.ToDateTime(),
                    FromPostId = s.FromPostId,
                    UntilPostId = s.UntilPostId,
                    ByLastPostId = s.ByLastPostId,
                    ContinueMonitoring = s.ContinueMonitoring,
                    Disabled = s.Disabled,
                    IgnoreVideo = s.Filters.IgnoreVideo,
                    IgnoreRepost = s.Filters.IgnoreRepost,
                    IgnoreAuthors = string.Join('|', s.Filters.IgnoreAuthors) ,
                    IgnoreTitles = string.Join('|', s.Filters.IgnoreTitles),
                    IgnoreWords = string.Join('|', s.Filters.IgnoreWords),
                    IgnoreDescriptions = string.Join('|', s.Filters.IgnoreDescriptions)
                }).ToList();
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return null;
        }

        public Task<bool> DeleteParserSettings(string id)
        {
            throw new NotImplementedException();
        }
    }
}
