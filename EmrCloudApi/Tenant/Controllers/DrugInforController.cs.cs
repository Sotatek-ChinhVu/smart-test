using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.DrugDetail;
using EmrCloudApi.Tenant.Requests.DrugDetail;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.DrugDetail;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.DrugDetail;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugInforController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public DrugInforController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet("GetMenuDataAndDetail")]
        public ActionResult<Response<GetDrugDetailResponse>> GetMenuDataAndDetail([FromQuery] GetDrugDetailRequest request)
        {
            var input = new GetDrugDetailInputData(request.HpId, request.SinDate, request.ItemCd);
            var output = _bus.Handle(input);

            var presenter = new GetDrugDetailPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDrugDetailResponse>>(presenter.Result);
        }
    }
}