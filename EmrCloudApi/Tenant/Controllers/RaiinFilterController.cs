using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.RaiinFilter;
using EmrCloudApi.Tenant.Requests.RaiinFilter;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.RaiinFilter;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.RaiinFilterMst.GetList;
using UseCase.RaiinFilterMst.SaveList;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RaiinFilterController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;

    public RaiinFilterController(UseCaseBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpGet(ApiPath.GetList + "Mst")]
    public async Task<ActionResult<Response<GetRaiinFilterMstListResponse>>> GetList()
    {
        var input = new GetRaiinFilterMstListInputData();
        var output = await Task.Run(() => _bus.Handle(input));
        var presenter = new GetRaiinFilterMstListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.SaveList + "Mst")]
    public async Task<ActionResult<Response<SaveRaiinFilterMstListResponse>>> SaveList([FromBody] SaveRaiinFilterMstListRequest req)
    {
        var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
        if (!validateToken)
        {
            return new ActionResult<Response<SaveRaiinFilterMstListResponse>>(new Response<SaveRaiinFilterMstListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
        }
        validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
        if (!validateToken)
        {
            return new ActionResult<Response<SaveRaiinFilterMstListResponse>>(new Response<SaveRaiinFilterMstListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
        }
        var input = new SaveRaiinFilterMstListInputData(req.FilterMsts, hpId, userId);
        var output = await Task.Run(() => _bus.Handle(input));
        var presenter = new SaveRaiinFilterMstListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
