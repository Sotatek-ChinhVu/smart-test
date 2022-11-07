using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.UserConf;
using EmrCloudApi.Tenant.Requests.UserConf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UserConf;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.GetUserConfList;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserConfController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;

    public UserConfController(UseCaseBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpGet(ApiPath.GetList)]
    public async Task<ActionResult<Response<GetUserConfListResponse>>> GetList([FromQuery] GetUserConfListRequest request)
    {
        var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
        if (!validateToken)
        {
            return new ActionResult<Response<GetUserConfListResponse>>(new Response<GetUserConfListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
        }
        validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
        if (!validateToken)
        {
            return new ActionResult<Response<GetUserConfListResponse>>(new Response<GetUserConfListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
        }
        var input = new GetUserConfListInputData(hpId, userId);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new GetUserConfListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetUserConfListResponse>>(presenter.Result);
    }
}
