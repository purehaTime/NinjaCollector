using DbService.Models;

namespace DbService.Interfaces
{
    public interface ISettingsService
    {
        public Task<List<ParserSettings>> GetParserSettings(string source);
        public Task<bool> SaveParserSettings(ParserSettings settings);

        public Task<List<PosterSettings>> GetPosterSettings(string service, string forGroup);
        public Task<bool> SavePosterSettings(PosterSettings settings);
    }
}
