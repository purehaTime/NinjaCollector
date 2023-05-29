using GrpcHelper.Interfaces;
using MainService.Models;

namespace MainService.Services
{
    public class TelegramStatusService : BaseStatusService<TelegramStatusModel>
    {
        public TelegramStatusService(IWorkerClientFactory clientFactory) : base(clientFactory, "telegram")
        {
        }
    }
}
