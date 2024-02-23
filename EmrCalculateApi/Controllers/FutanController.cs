using CalculateService.Interface;
using CalculateService.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmrCalculateApi.Controllers
{
#pragma warning disable CS8625
    [Route("api/[controller]")]
    [ApiController]
    public class FutanController : ControllerBase
    {
        private readonly IFutancalcViewModel _futanCalculate;
        public FutanController(IFutancalcViewModel futanCalculate)
        {
            _futanCalculate = futanCalculate;
        }

        [HttpPost("RunFutanCalculate")]
        public ActionResult RunCalculateFutan([FromBody] CalculateRequest calculateRequest)
        {
            _futanCalculate.FutanCalculation
            (
                calculateRequest.HpId,
                calculateRequest.PtId,
                calculateRequest.SinDate,
                null, null, null, null,
                calculateRequest.SeikyuUp
            );
            _futanCalculate.ReleaseResource();
            return Ok();
        }
    }
}