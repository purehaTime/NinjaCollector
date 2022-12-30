namespace GrpcHelper.Interfaces
{
    public interface ILoggerServiceClient
    {
        public Task WriteLog(string? message);
    }
}
