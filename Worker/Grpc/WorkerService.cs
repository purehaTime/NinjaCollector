using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcHelper.WorkerService;

namespace Worker.Grpc
{
    public class WorkerService : GrpcHelper.WorkerService.Worker.WorkerBase
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
                    workers.Select(s => new Work
                    {
                        ApiService = s.Settings.ApiName,
                        Group = s.Settings.ForGroup,
                        TaskId = s.TaskId,
                    })
                }
            };

            return Task.FromResult(result);
        }
    }
}
