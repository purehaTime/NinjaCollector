using Worker.Model;

namespace Worker.Interfaces
{
    public interface IWorker
    {
        string Name { get; }
        Task<List<Settings>> Init();
        Task<Settings> LoadSettings(string settingsId);
        Task<Settings> Run(Settings setting);
    }
}