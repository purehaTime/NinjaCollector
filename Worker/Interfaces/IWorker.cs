using ModelsHelper.Models;
using Worker.Model;

namespace Worker.Interfaces
{
    public interface IWorker
    {
        string Name { get; }
        Task<List<ParserSettings>> Init();
        Task<ParserSettings> LoadSettings(string settingsId);
        Task<ParserSettings> Run(ParserSettings setting);
    }
}