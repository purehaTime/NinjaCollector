using Models.Models;

namespace MainService.Interfaces
{
    public interface IParserService
    {
        public Task<bool> SaveParserSettings(ParserSettings parserSettings);

        public Task<List<ParserSettings>> GetAllParserSettings();

        public Task<bool> DeleteParserSettings(string id);
    }
}
