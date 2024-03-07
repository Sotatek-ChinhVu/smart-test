using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Ka;
using EmrCloudApi.Requests.Ka;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Ka;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Ka.GetKaCodeList;
using UseCase.Ka.GetKacodeMstYossi;
using UseCase.Ka.GetKacodeYousikiMst;
using UseCase.Ka.GetList;
using UseCase.Ka.SaveList;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class KaController : BaseParamControllerBase
{
    private readonly UseCaseBus _bus;

    public KaController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList + "Mst")]
    public ActionResult<Response<GetKaMstListResponse>> GetListMst(int isDeleted)
    {
        var input = new GetKaMstListInputData(HpId, isDeleted);
        var output = _bus.Handle(input);
        var presenter = new GetKaMstListPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetKaMstListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetListKaCode)]
    public ActionResult<Response<GetKaCodeMstListResponse>> GetListKaCodeMst()
    {
        var input = new GetKaCodeMstInputData(HpId);
        var output = _bus.Handle(input);
        var presenter = new GetKaCodeMstListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.SaveListKaMst)]
    public ActionResult<Response<SaveListKaMstResponse>> Save([FromBody] SaveListKaMstRequest request)
    {
        var input = new SaveKaMstInputData(HpId, UserId, request.KaMstRequestItems.Select(input => new SaveKaMstInputItem(input.Id, input.KaId, input.ReceKaCd, input.KaSname, input.KaName, input.YousikiKaCd)).ToList());
        var output = _bus.Handle(input);
        var presenter = new SaveListKaMstPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<SaveListKaMstResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetKaCodeMstYossi)]
    public ActionResult<Response<GetKaCodeMstListResponse>> GetKaCodeMstYossi()
    {
        var input = new GetKacodeMstYossiInputData(HpId);
        var output = _bus.Handle(input);
        var presenter = new GetKaCodeMstYossiPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpGet(ApiPath.GetKaCodeYousikiMst)]
    public ActionResult<Response<GetKaCodeYousikiMstResponse>> GetKaCodeYousikiMst()
    {
        var input = new GetKaCodeYousikiMstInputData(HpId);
        var output = _bus.Handle(input);
        var presenter = new GetKaCodeYousikiMstPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
