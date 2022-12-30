using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Santei;
using EmrCloudApi.Requests.Santei;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Santei;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Santei.GetListSanteiInf;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class SanteiController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    public SanteiController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetListSanteiInfResponse>> GetList([FromQuery] GetListSanteiInfRequest request)
    {
        var input = new GetListSanteiInfInputData(HpId, request.PtId, request.SinDate, request.HokenPid);
        var output = _bus.Handle(input);

        var presenter = new GetListSanteiInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListSanteiInfResponse>>(presenter.Result);
    }
}
