using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Ka;
using EmrCloudApi.Requests.Ka;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Ka;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Ka.GetKaCodeList;
using UseCase.Ka.GetList;
using UseCase.Ka.SaveList;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class KaController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public KaController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList + "Mst")]
    public ActionResult<Response<GetKaMstListResponse>> GetListMst()
    {
        var input = new GetKaMstListInputData();
        var output = _bus.Handle(input);
        var presenter = new GetKaMstListPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetKaMstListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetListKaCode)]
    public ActionResult<Response<GetKaCodeMstListResponse>> GetListKaCodeMst()
    {
        var input = new GetKaCodeMstInputData();
        var output = _bus.Handle(input);
        var presenter = new GetKaCodeMstListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.SaveListKaMst)]
    public ActionResult<Response<SaveListKaMstResponse>> Save([FromBody] SaveListKaMstRequest request)
    {
        var input = new SaveKaMstInputData(HpId, UserId, request.KaMstRequestItems.Select(input => new SaveKaMstInputItem(input.Id, input.KaId, input.ReceKaCd, input.KaSname, input.KaName)).ToList());
        var output = _bus.Handle(input);
        var presenter = new SaveListKaMstPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<SaveListKaMstResponse>>(presenter.Result);
    }
}
