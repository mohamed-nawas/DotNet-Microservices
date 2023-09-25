using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/v1/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController() { }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("Inbound POST # CommandsService");
            return Ok("Inbound connection OK from PlatformsController");
        }
    }
}