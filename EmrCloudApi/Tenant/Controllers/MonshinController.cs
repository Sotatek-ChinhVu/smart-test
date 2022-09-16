using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using Microsoft.AspNetCore.Mvc;
using EmrCloudApi.Realtime;
using UseCase.Core.Sync;
using EmrCloudApi.Tenant.Responses.MonshinInfor;
using EmrCloudApi.Tenant.Requests.MonshinInfor;
using UseCase.MonshinInfor.GetList;
using EmrCloudApi.Tenant.Presenters.MonshinInf;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonshinController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IWebSocketService _webSocketService;

        public MonshinController(UseCaseBus bus,
        IWebSocketService webSocketService)
        {
            _bus = bus;
            _webSocketService = webSocketService;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetMonshinInforListResponse>> GetList([FromQuery] GetMonshinInforListRequest request)
        {
            var input = new GetMonshinInforListInputData(request.HpId, request.PtId);
            var output = _bus.Handle(input);
            var presenter = new GetMonshinInforListPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
