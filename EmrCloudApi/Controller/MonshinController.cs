using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.MonshinInf;
using EmrCloudApi.Requests.MonshinInfor;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MonshinInfor;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MonshinInfor.GetList;
using UseCase.MonshinInfor.Save;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class MonshinController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;
        public MonshinController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetMonshinInf)]
        public ActionResult<Response<GetMonshinInforListResponse>> GetMonshinInf([FromQuery] GetMonshinInforListRequest request)
        {
            var input = new GetMonshinInforListInputData(HpId, request.PtId, request.RaiinNo, request.IsDeleted, request.IsGetAll);
            var output = _bus.Handle(input);
            var presenter = new GetMonshinInforListPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.SaveMonshinInf)]
        public ActionResult<Response<SaveMonshinInforListResponse>> SaveList([FromBody] SaveMonshinInforListRequest request)
        {
            var input = new SaveMonshinInputData(request.Monshins, UserId);
            var output = _bus.Handle(input);
            var presenter = new SaveMonshinInforListPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
