using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SetKbnMst;
using EmrCloudApi.Tenant.Requests.SetKbnMst;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetKbnMst;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SetKbnMst.GetList;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SetKbnController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public SetKbnController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetSetKbnMstListResponse>> GetList([FromQuery] GetSetKbnMstListRequest request)
        {
            int hpId = _userService.GetLoginUser().HpId;
            var input = new GetSetKbnMstListInputData(hpId, request.SinDate, request.SetKbnFrom, request.SetKbnTo);
            var output = _bus.Handle(input);

            var presenter = new GetSetKbnMstListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSetKbnMstListResponse>>(presenter.Result);
        }
    }
}
