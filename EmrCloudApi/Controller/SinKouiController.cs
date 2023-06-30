using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SinKoui;
using EmrCloudApi.Requests.SinKoui;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SinKoui;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SinKoui.GetSinKoui;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class SinKouiController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public SinKouiController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetSinKouiResponse>> GetList([FromQuery] GetSinKouiRequest req)
    {
        var input = new GetSinKouiInputData(HpId, req.PtId);
        var output = _bus.Handle(input);

        var presenter = new GetSinKouiPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSinKouiResponse>>(presenter.Result);
    }
}
