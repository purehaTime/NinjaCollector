using ModelsHelper.Models;

namespace MainService.Interfaces
{
    public interface IParserService
    {
        public Task<bool> SaveParserSettings(ParserSettings parserSettings);

        public Task<List<ParserSettings>> GetAllParserSettings();

        public Task<ParserSettings> GetParserSettings(string settingId);

        public Task<bool> DeleteParserSettings(string id);
    }
}
