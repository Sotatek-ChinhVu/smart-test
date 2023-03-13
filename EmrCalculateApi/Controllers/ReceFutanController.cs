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

        [HttpPost("GetListReceInf")]
        public ActionResult<GetListReceInfResponse> GetListReceInf([FromBody] GetListReceInfRequest request)
        {
            int hpId = request.HpId;

            var response = _receFutanCalculate.KaikeiTotalCalculate(request.PtId, request.SinYm);

            return new ActionResult<GetListReceInfResponse>(new GetListReceInfResponse(response));
        }
    }
}
