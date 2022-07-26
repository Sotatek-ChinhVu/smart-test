using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Set;
using EmrCloudApi.Tenant.Requests.Set;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Set;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Set.GetList;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public SetController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetSetListResponse>> GetList([FromQuery] GetSetListRequest request)
        {
            var input = new GetSetListInputData(request.HpId, request.SetKbn, request.SetKbnEdaNo, request.TextSearch, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetSetListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSetListResponse>>(presenter.Result);
        }
    }
}
