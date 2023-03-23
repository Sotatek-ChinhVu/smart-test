using Domain.Models.Futan;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Requests;
using EmrCalculateApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace EmrCalculateApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateController : ControllerBase
    {
        private readonly IIkaCalculateViewModel _ikaCalculate;
        public CalculateController(IIkaCalculateViewModel ikaCalculate)
        {
            _ikaCalculate = ikaCalculate;
        }

        [HttpPost("RunCalculateOne")]
        public ActionResult RunCalculateOne([FromBody] CalculateOneRequest calculateOneRequest)
        {
            _ikaCalculate.RunCalculateOne(
                calculateOneRequest.HpId,
                calculateOneRequest.PtId,
                calculateOneRequest.SinDate,
                calculateOneRequest.SeikyuUp,
                calculateOneRequest.Prefix);
            return Ok();
        }

        [HttpPost("RunCalculate")]
        public ActionResult RunCalculate([FromBody] CalculateRequest calculateRequest)
        {
            _ikaCalculate.RunCalculate(
                calculateRequest.HpId,
                calculateRequest.PtId,
                calculateRequest.SinDate,
                calculateRequest.SeikyuUp,
                calculateRequest.Prefix);
            return Ok();
        }

        [HttpPost("RunTrialCalculate")]
        public ActionResult RunTrialCalculate([FromBody] RunTraialCalculateRequest calculateRequest)
        {
            var data = _ikaCalculate.RunTraialCalculate(
                calculateRequest.OrderInfoList,
                calculateRequest.Reception,
                calculateRequest.CalcFutan);
            var result = new RunTraialCalculateResponse(data.Item1, data.Item2.Select(k => new KaikeiInfItemResponse(k)).ToList(), data.Item3);
            return Ok(result);
        }

        [HttpPost("RunCalculateMonth")]
        public ActionResult RunCalculateMonth([FromBody] RunCalculateMonthRequest monthRequest)
        {
            _ikaCalculate.RunCalculateMonth(
                monthRequest.HpId,
                monthRequest.SeikyuYm,
                monthRequest.PtIds,
                monthRequest.PreFix);

            return Ok();
        }
    }
}
