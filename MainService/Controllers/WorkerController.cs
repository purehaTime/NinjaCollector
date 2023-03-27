using GrpcHelper.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace MainService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWorkerClientAggregator _clients;

        public WorkersController(ILogger logger, IWorkerClientAggregator clients)
        {
            _logger = logger;
            _clients = clients;
        }

        [HttpGet]
        public async Task<ActionResult> Workers(string workerId)
        {
            var redditWorkers = await _clients.Reddit.GetWorkers();
            return Ok(redditWorkers);
        }

        
    }
}
