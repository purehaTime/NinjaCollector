using ModelsHelper.Models;

namespace MainService.Interfaces
{
    public interface IPosterService
    {
        public Task<bool> SavePosterSettings(PosterSettings parserSettings);

        public Task<List<PosterSettings>> GetAllPosterSettings();

        public Task<PosterSettings> GetPosterSettings(string settingId);

        public Task<bool> DeletePosterSettings(string id);
    }
}
