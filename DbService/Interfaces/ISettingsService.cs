using DbService.Models;

namespace DbService.Interfaces
{
    public interface ISettingsService
    {
        public Task<List<ParserSettings>> GetSettings(string source);
        public Task<bool> SaveSettings(ParserSettings settings);
    }
}
