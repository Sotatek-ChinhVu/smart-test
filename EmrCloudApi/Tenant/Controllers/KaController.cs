using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Ka;
using EmrCloudApi.Tenant.Requests.Ka;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Ka;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Ka.GetKaCodeList;
using UseCase.Ka.GetList;
using UseCase.Ka.SaveList;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KaController : ControllerBase
{
    private readonly UseCaseBus _bus;

    public KaController(UseCaseBus bus)
    {
        _bus = bus;
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
        var input = new SaveKaMstInputData(request.HpId, request.UserId, request.kaMstRequestItems.Select(input => new SaveKaMstInputItem(input.Id, input.KaId, input.ReceKaCd, input.KaSname, input.KaName)).ToList());
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new SaveListKaMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveListKaMstResponse>>(presenter.Result);
    }
}
