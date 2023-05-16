using DbService.Models;
using MongoDB.Bson;

namespace DbService.Interfaces
{
    public interface ISettingsService
    {
        public Task<List<ParserSettings>> GetParserSettings(string source, ObjectId? settingsId);
        public Task<bool> SaveParserSettings(ParserSettings settings);
        public Task<bool> RemoveParserSettings(string id);

        public Task<List<PosterSettings>> GetPosterSettings(string service, ObjectId? settingsId);
        public Task<bool> SavePosterSettings(PosterSettings settings);
        public Task<bool> RemovePosterSettings(string id);
        public Task<PosterSettings> GetPosterSetting(string id);
    }
}
