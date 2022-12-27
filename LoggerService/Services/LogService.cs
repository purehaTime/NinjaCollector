using Grpc.Core;
using GrpcHelper.LogService;

namespace LoggerService.Services
{
    public class LogService : Logger.LoggerClient
    {
        public override AsyncUnaryCall<WriteResponse> WriteLogAsync(LogModel request, Metadata headers = null, DateTime? deadline = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return new AsyncUnaryCall<WriteResponse>(null, null, null, null, null, null);
        }
    }
}
