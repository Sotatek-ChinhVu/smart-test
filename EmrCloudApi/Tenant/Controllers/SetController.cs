using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SetMst;
using EmrCloudApi.Tenant.Requests.SetMst;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetMst;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SetMst.GetList;

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
        public ActionResult<Response<GetSetMstListResponse>> GetList([FromQuery] GetSetMstListRequest request)
        {
            var input = new GetSetMstListInputData(request.HpId, request.SetKbn, request.SetKbnEdaNo, request.TextSearch, request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetSetMstListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSetMstListResponse>>(presenter.Result);
        }
    }
}
