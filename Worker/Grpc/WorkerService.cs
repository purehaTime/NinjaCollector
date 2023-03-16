using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcHelper.WorkerService;
using Worker.Interfaces;
using Status = GrpcHelper.WorkerService.Status;

namespace Worker.Grpc
{
    public class WorkerService : GrpcHelper.WorkerService.WorkerService.WorkerServiceBase
    {
        private IWorkService _workService;

        public WorkerService(IWorkService workService)
        {
            _workService = workService;
        }

        public override Task<WorkerModel> GetWorkers(Empty request, ServerCallContext context)
        {
            var workers = _workService.GetWorkers();

            var result = new WorkerModel
            {
                Workers =
                {
                    workers.Select(s => new GrpcHelper.WorkerService.Worker
                    {
                        WorkerName = s.WorkerInstance.Name,
                        Works = { s.Works.Select(w => new Work
                        {
                            Group = w.Settings.ForGroup,
                            TaskId = w.TaskId,
                            SettingsId = w.Settings.Id ?? ""
                        }) }
                    })
                }
            };

            return Task.FromResult(result);
        }

        public override async Task<Status> RestartWorker(WorkerRestart request, ServerCallContext context)
        {
            var result = await _workService.RestartWorker(request.TaskId, request.SettingsId);

            return new Status { Success = result };
        }

        public override async Task<Status> RestartWorkers(Empty request, ServerCallContext context)
        {
            var result = await _workService.RestartWorkers();

            return new Status { Success = result };
        }
    }
}
