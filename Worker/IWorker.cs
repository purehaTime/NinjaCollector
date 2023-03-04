using Worker.Model;

namespace Worker
{
    public interface IWorker
    {
        Task<List<Settings>> Init();
        Task Run(Settings setting);
        void GetStatus();
    }
}