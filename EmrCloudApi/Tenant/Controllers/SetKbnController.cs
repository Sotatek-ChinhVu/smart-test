using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SetKbnMst;
using EmrCloudApi.Tenant.Requests.SetKbnMst;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetKbnMst;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SetKbnMst.GetList;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<Response<GetSetKbnMstListResponse>>> GetList([FromQuery] GetSetKbnMstListRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetSetKbnMstListResponse>>(new Response<GetSetKbnMstListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetSetKbnMstListInputData(hpId, request.SinDate, request.SetKbnFrom, request.SetKbnTo);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetSetKbnMstListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetSetKbnMstListResponse>>(presenter.Result);
        }
    }
}
