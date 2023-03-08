using EmrCalculateApi.Interface;
using EmrCalculateApi.Requests;
using EmrCalculateApi.Responses;
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

        [HttpPost("RunReceFutanCalculate")]
        public ActionResult RunCalculateReceFutan([FromBody] ReceCalculateRequest calculateRequest)
        {
            _receFutanCalculate.ReceFutanCalculateMain
            (
                calculateRequest.PtIds,
                calculateRequest.SeikyuYm
            );
            return Ok();
        }

        [HttpPost("GetListReceInf")]
        public ActionResult<GetListReceInfResponse> GetSinMeiList([FromBody] GetListReceInfRequest request)
        {
            int hpId = request.HpId;
            long ptId = request.PtId;
            int sinYm = request.SinYm;

            var response = _receFutanCalculate.KaikeiTotalCalculate(ptId, sinYm);

            return new ActionResult<GetListReceInfResponse>(new GetListReceInfResponse(response));
        }
    }
}
