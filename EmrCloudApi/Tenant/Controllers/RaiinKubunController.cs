using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.RaiinKubun;
using EmrCloudApi.Tenant.Requests.RaiinKubun;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.RaiinKubun;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.RaiinKubunMst.GetList;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaiinKubunController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public RaiinKubunController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList + "Mst")]
        public ActionResult<Response<GetRaiinKubunMstListResponse>> GetRaiinKubunMstList([FromQuery] GetRaiinKubunMstListRequest request)
        {
            var input = new GetRaiinKubunMstListInputData(request.IsDeleted);
            var output = _bus.Handle(input);

            var presenter = new GetRaiinKubunMstListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetRaiinKubunMstListResponse>>(presenter.Result);
        }
    }
}
