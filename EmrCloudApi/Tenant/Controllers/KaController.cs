﻿using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Ka;
using EmrCloudApi.Tenant.Requests.Ka;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Ka;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.KaMst.GetKaCodeList;
using UseCase.KaMst.GetList;
using UseCase.KaMst.SaveList;

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
    public ActionResult<Response<GetKaMstListResponse>> GetListMst()
    {
        var input = new GetKaMstListInputData();
        var output = _bus.Handle(input);
        var presenter = new GetKaMstListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
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

    [HttpPost(ApiPath.SaveList)]
    public ActionResult<Response<SaveListKaMstResponse>> Save([FromBody] SaveListKaMstRequest request)
    {
        var input = new SaveKaMstInputData(request.HpId, request.UserId, request.kaMstRequestItems.Select(input => new SaveKaMstInputItem(input.Id, input.KaId, input.ReceKaCd, input.KaSname, input.KaName)).ToList());
        var output = _bus.Handle(input);

        var presenter = new SaveListKaMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveListKaMstResponse>>(presenter.Result);
    }
}
