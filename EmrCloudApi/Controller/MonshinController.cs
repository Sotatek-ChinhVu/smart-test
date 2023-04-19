using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.MonshinInf;
using EmrCloudApi.Requests.MonshinInfor;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MonshinInfor;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MonshinInfor.GetList;
using UseCase.MonshinInfor.Save;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class MonshinController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public MonshinController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
            _userService = userService;
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
