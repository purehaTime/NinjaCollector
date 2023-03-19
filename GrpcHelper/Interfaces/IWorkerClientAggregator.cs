using GrpcHelper.WorkerService;

namespace GrpcHelper.Interfaces
{
    public interface IWorkerClientAggregator
    {
        public IWorkerServiceClient Reddit { get; }

    }
}
