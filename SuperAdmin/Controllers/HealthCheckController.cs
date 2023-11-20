using Microsoft.AspNetCore.Mvc;

namespace SuperAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthyController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Done");
        }
    }
}
