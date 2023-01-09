using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.UserConf;
using EmrCloudApi.Requests.UserConf;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UserConf;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.GetUserConfList;
using UseCase.User.Sagaku;
using UseCase.User.UpdateUserConf;
using UseCase.UserConf.UpdateAdoptedByomeiConfig;

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

    [HttpPut(ApiPath.UpdateAdoptedByomeiConfig)]
    public ActionResult<Response<UpdateAdoptedByomeiConfigResponse>> UpdateAdoptedByomeiConfig([FromBody] UpdateAdoptedByomeiConfigRequest request)
    {
        var input = new UpdateAdoptedByomeiConfigInputData(request.AdoptedValue, HpId, UserId);
        var output = _bus.Handle(input);

        var presenter = new UpdateAdoptedByomeiConfigPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateAdoptedByomeiConfigResponse>>(presenter.Result);
    }

    [HttpPut(ApiPath.Update)]
    public ActionResult<Response<UpdateUserConfResponse>> Update([FromBody] UpdateUserConfRequest request)
    {
        var input = new UpdateUserConfInputData(HpId, UserId, request.GrpCd, request.Value);
        var output = _bus.Handle(input);

        var presenter = new UpdateUserConfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateUserConfResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.Sagaku)]
    public ActionResult<Response<SagakuResponse>> Sagaku([FromQuery] SagakuRequest request)
    {
        var input = new SagakuInputData(HpId, UserId, request.FromRece);
        var output = _bus.Handle(input);

        var presenter = new SagakuPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SagakuResponse>>(presenter.Result);
    }
}
