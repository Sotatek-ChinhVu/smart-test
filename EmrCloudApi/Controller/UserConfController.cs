﻿using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.UserConf;
using EmrCloudApi.Requests.UserConf;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UserConf;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.GetUserConfList;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class UserConfController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public UserConfController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetUserConfListResponse>> GetList([FromQuery] GetUserConfListRequest request)
    {
        var input = new GetUserConfListInputData(HpId, UserId);
        var output = _bus.Handle(input);

        var presenter = new GetUserConfListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetUserConfListResponse>>(presenter.Result);
    }
}
