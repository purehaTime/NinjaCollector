using DbService.Models;
using MongoDB.Bson;

namespace DbService.Interfaces
{
    public interface ISettingsService
    {
        public Task<List<DbParserSettings>> GetParserSettings(string source, ObjectId? settingsId);
        public Task<bool> SaveParserSettings(DbParserSettings settings);

        public Task<List<PosterSettings>> GetPosterSettings(string service, string forGroup);
        public Task<bool> SavePosterSettings(PosterSettings settings);
    }
}
