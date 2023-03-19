namespace GrpcHelper.Interfaces
{
    public interface ILoggerServiceClient
    {
        public Task<bool> WriteLog(string message, string eventId, string application);
    }
}
