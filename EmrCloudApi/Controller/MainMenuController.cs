using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Services;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Requests.MainMenu;
using UseCase.MainMenu.GetDailyStatisticMenu;
using EmrCloudApi.Presenters.MainMenu;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class MainMenuController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public MainMenuController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetDailyStatisticMenuList)]
    public ActionResult<Response<GetDailyStatisticMenuResponse>> GetList([FromQuery] GetDailyStatisticMenuRequest request)
    {
        var input = new GetDailyStatisticMenuInputData(HpId, request.MenuId);
        var output = _bus.Handle(input);

        var presenter = new GetDailyStatisticMenuPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetDailyStatisticMenuResponse>>(presenter.Result);
    }
}