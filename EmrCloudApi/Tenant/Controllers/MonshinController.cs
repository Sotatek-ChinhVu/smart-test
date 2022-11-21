using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Tenant.Responses.MonshinInfor;
using EmrCloudApi.Tenant.Requests.MonshinInfor;
using UseCase.MonshinInfor.GetList;
using EmrCloudApi.Tenant.Presenters.MonshinInf;
using UseCase.MonshinInfor.Save;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MonshinController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public MonshinController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet(ApiPath.GetList)]
        public async Task<ActionResult<Response<GetMonshinInforListResponse>>> GetList([FromQuery] GetMonshinInforListRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new GetMonshinInforListInputData(hpId, request.PtId, request.SinDate, request.IsDeleted);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new GetMonshinInforListPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }

        [HttpPost(ApiPath.SaveList)]
        public async Task<ActionResult<Response<SaveMonshinInforListResponse>>> SaveList([FromBody] SaveMonshinInforListRequest request)
        {
            int userId = _userService.GetLoginUser().UserId;
            var input = new SaveMonshinInputData(request.Monshins, userId);
            var output = await Task.Run(() => _bus.Handle(input));
            var presenter = new SaveMonshinInforListPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
