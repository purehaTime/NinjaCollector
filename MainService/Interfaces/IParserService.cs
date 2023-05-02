using MainService.Models;

namespace MainService.Interfaces
{
    public interface IParserService
    {
        public Task<bool> SaveParserSettings(ParserModel parserSettings);

        public Task<List<ParserModel>> GetAllParserSettings();

        public Task<bool> DeleteParserSettings(string id);
    }
}
