using Worker.Model;

namespace Worker
{
    public interface IWorker
    {
        Task<List<Settings>> Init();
        Task<Settings> Run(Settings setting);
    }
}