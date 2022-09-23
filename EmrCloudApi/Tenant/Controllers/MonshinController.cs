using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using Microsoft.AspNetCore.Mvc;
using EmrCloudApi.Realtime;
using UseCase.Core.Sync;
using EmrCloudApi.Tenant.Responses.MonshinInfor;
using EmrCloudApi.Tenant.Requests.MonshinInfor;
using UseCase.MonshinInfor.GetList;
using EmrCloudApi.Tenant.Presenters.MonshinInf;
using UseCase.MonshinInfor.Save;

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
        
        public MonshinController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetMonshinInforListResponse>> GetList([FromQuery] GetMonshinInforListRequest request)
        {
            var input = new GetMonshinInforListInputData(request.HpId, request.PtId, request.SinDate, request.IsDeleted);
            var output = _bus.Handle(input);
            var presenter = new GetMonshinInforListPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.SaveList)]
        public ActionResult<Response<SaveMonshinInforListResponse>> SaveList([FromBody] SaveMonshinInforListRequest request)
        {
            var input = new SaveMonshinInputData(request.Monshins);
            var output = _bus.Handle(input);
            var presenter = new SaveMonshinInforListPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
