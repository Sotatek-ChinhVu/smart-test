using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.UketukeSbt;
using EmrCloudApi.Tenant.Requests.UketukeSbt;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UketukeSbt;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.UketukeSbtMst.GetBySinDate;
using UseCase.UketukeSbtMst.GetList;
using UseCase.UketukeSbtMst.GetNext;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UketukeSbtController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;

    public UketukeSbtController(UseCaseBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpGet(ApiPath.GetList + "Mst")]
    public async Task<ActionResult<Response<GetUketukeSbtMstListResponse>>> GetListMst()
    {
        var input = new GetUketukeSbtMstListInputData();
        var output = await Task.Run(() => _bus.Handle(input));
        var presenter = new GetUketukeSbtMstListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpGet(ApiPath.Get + "BySinDate")]
    public async Task<ActionResult<Response<GetUketukeSbtMstBySinDateResponse>>> GetBySinDate([FromQuery] GetUketukeSbtMstBySinDateRequest req)
    {
        var input = new GetUketukeSbtMstBySinDateInputData(req.SinDate);
        var output = await Task.Run(() => _bus.Handle(input));
        var presenter = new GetUketukeSbtMstBySinDatePresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpGet(ApiPath.Get + "Next")]
    public async Task<ActionResult<Response<GetNextUketukeSbtMstResponse>>> GetNext([FromQuery] GetNextUketukeSbtMstRequest req)
    {
        var validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
        if (!validateToken)
        {
            return new ActionResult<Response<GetNextUketukeSbtMstResponse>>(new Response<GetNextUketukeSbtMstResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
        }
        var input = new GetNextUketukeSbtMstInputData(req.SinDate, req.CurrentKbnId, userId);
        var output = await Task.Run(() => _bus.Handle(input));
        var presenter = new GetNextUketukeSbtMstPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
