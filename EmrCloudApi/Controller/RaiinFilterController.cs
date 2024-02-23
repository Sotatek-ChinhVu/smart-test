﻿using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.RaiinFilter;
using EmrCloudApi.Requests.RaiinFilter;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.RaiinFilter;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.RaiinFilterMst.GetList;
using UseCase.RaiinFilterMst.SaveList;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class RaiinFilterController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public RaiinFilterController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList + "Mst")]
    public ActionResult<Response<GetRaiinFilterMstListResponse>> GetList()
    {
        var input = new GetRaiinFilterMstListInputData(HpId);
        var output = _bus.Handle(input);
        var presenter = new GetRaiinFilterMstListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.SaveList + "Mst")]
    public ActionResult<Response<SaveRaiinFilterMstListResponse>> SaveList([FromBody] SaveRaiinFilterMstListRequest req)
    {
        var input = new SaveRaiinFilterMstListInputData(req.FilterMsts, HpId, UserId);
        var output = _bus.Handle(input);
        var presenter = new SaveRaiinFilterMstListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
