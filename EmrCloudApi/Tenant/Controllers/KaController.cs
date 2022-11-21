using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Ka;
using EmrCloudApi.Tenant.Requests.Ka;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Ka;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Ka.GetKaCodeList;
using UseCase.Ka.GetList;
using UseCase.Ka.SaveList;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class KaController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;

    public KaController(UseCaseBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpGet(ApiPath.GetList + "Mst")]
    public async Task<ActionResult<Response<GetKaMstListResponse>>> GetListMst()
    {
        var input = new GetKaMstListInputData();
        var output = await Task.Run(() => _bus.Handle(input));
        var presenter = new GetKaMstListPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetKaMstListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetListKaCode)]
    public async Task<ActionResult<Response<GetKaCodeMstListResponse>>> GetListKaCodeMst()
    {
        var input = new GetKaCodeMstInputData();
        var output = await Task.Run(() => _bus.Handle(input));
        var presenter = new GetKaCodeMstListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.SaveListKaMst)]
    public async Task<ActionResult<Response<SaveListKaMstResponse>>> Save([FromBody] SaveListKaMstRequest request)
    {
        int hpId = _userService.GetLoginUser().HpId;
        int userId = _userService.GetLoginUser().UserId;
        var input = new SaveKaMstInputData(hpId, userId, request.KaMstRequestItems.Select(input => new SaveKaMstInputItem(input.Id, input.KaId, input.ReceKaCd, input.KaSname, input.KaName)).ToList());
        var output = await Task.Run(() => _bus.Handle(input));
        var presenter = new SaveListKaMstPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<SaveListKaMstResponse>>(presenter.Result);
    }
}
