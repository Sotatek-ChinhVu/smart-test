using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Responses.MonshinInfor;
using EmrCloudApi.Requests.MonshinInfor;
using UseCase.MonshinInfor.GetList;
using EmrCloudApi.Presenters.MonshinInf;
using UseCase.MonshinInfor.Save;
using EmrCloudApi.Services;

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

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetMonshinInforListResponse>> GetList([FromQuery] GetMonshinInforListRequest request)
        {
            var input = new GetMonshinInforListInputData(HpId, request.PtId, request.SinDate, request.IsDeleted);
            var output = _bus.Handle(input);
            var presenter = new GetMonshinInforListPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.SaveList)]
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
