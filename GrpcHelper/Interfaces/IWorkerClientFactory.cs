namespace GrpcHelper.Interfaces
{
    public interface IWorkerClientFactory
    {
        public IWorkerServiceClient CreateClient(string name);
    }
}
