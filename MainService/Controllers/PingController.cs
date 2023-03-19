using GrpcHelper.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace MainService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWorkerClientAggregator _clients;

        public PingController(ILogger logger, IWorkerClientAggregator clients)
        {
            _logger = logger;
            _clients = clients;
        }
        
        [HttpGet]
        public ActionResult Ping()
        {
            _logger.Warning("test message");
            return Ok("pong !");
        }

        [HttpGet]
        public ActionResult Workers()
        {
            var redditWorkers = _clients.Reddit.GetWorkers();
            return Ok(redditWorkers);
        }
    }
}
