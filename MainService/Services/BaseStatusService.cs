using GrpcHelper.Interfaces;
using MainService.Interfaces;
using MainService.Models;

namespace MainService.Services
{
    public abstract class BaseStatusService<TStatusModel> : IWorkerService<TStatusModel> where TStatusModel : StatusModel, new()
    {
        private readonly IWorkerServiceClient _workerClient;
        private readonly string _serviceName;

        protected BaseStatusService(IWorkerClientFactory clientFactory, string serviceName)
        {
            _workerClient = clientFactory.CreateClient(serviceName);
            _serviceName = serviceName;
        }
        public virtual async Task<bool> StopWorker(int taskId)
        {
            return await _workerClient.StopWorker(taskId);
        }

        public virtual async Task<TStatusModel> GetStatus()
        {
            var status = await _workerClient.GetWorkers();

            var works = new List<Work>();
            foreach (var worker in status.Workers)
            {
                works.AddRange(worker.Works.Select(s => new Work
                {
                     WorkerName = worker.WorkerName,
                     SettingsId = s.SettingsId,
                     Group = s.Group,
                     TaskId = s.TaskId
                }));
            }

            return new TStatusModel
            {
                Name = _serviceName,
                Works = works
            };
        }

        public async Task<bool> StopAll()
        {
            var result = await _workerClient.StopAll();
            return result;
        }

        public async Task<bool> RunAll()
        {
            var result = await _workerClient.RunAll();
            return result;
        }
    }
}
