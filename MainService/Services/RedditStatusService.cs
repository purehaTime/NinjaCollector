using GrpcHelper.Interfaces;
using MainService.Interfaces;
using MainService.Models;

namespace MainService.Services
{
    public class RedditStatusService : BaseStatusService<RedditStatusModel>
    {
        public RedditStatusService(IWorkerClientFactory clientFactory) : base(clientFactory, "reddit")
        {
        }
    }
}
