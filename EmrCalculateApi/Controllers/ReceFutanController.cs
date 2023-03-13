using EmrCalculateApi.Interface;
using EmrCalculateApi.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmrCalculateApi.Controllers
{
#pragma warning disable CS8625
    [Route("api/[controller]")]
    [ApiController]
    public class ReceFutanController : ControllerBase
    {
        private readonly IReceFutanViewModel _receFutanCalculate;
        public ReceFutanController(IReceFutanViewModel receFutanCalculate)
        {
            _receFutanCalculate = receFutanCalculate;
        }

        [HttpPost("ReceFutanCalculateMain")]
        public ActionResult ReceFutanCalculateMain([FromBody] ReceCalculateRequest calculateRequest)
        {
            _receFutanCalculate.ReceFutanCalculateMain
            (
                calculateRequest.PtIds,
                calculateRequest.SeikyuYm
            );
            return Ok();
        }
    }
}
