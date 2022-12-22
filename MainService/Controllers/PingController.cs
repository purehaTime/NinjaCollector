using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace MainService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly ILogger _logger;

        public PingController(ILogger logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public ActionResult Ping()
        {
            return Ok("pong !");
        }
    }
}
