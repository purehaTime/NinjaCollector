using GrpcHelper.AuthService;
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
        private readonly IAuthServiceClient _authClient;

        public PingController(ILogger logger, IWorkerClientAggregator clients, IAuthServiceClient authClient)
        {
            _logger = logger;
            _clients = clients;
            _authClient = authClient;
        }
        
        [HttpGet]
        public async Task<ActionResult> Ping()
        {
            _logger.Warning("test message");

            await _authClient.Authorize(new AuthorizeModel
            {
                Invite = "invite",
                UserLogin = "test",
                UserPassword = "test",
            });

            return Ok("pong !");
        }
    }
}
