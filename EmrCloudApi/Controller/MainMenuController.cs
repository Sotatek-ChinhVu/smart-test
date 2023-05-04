using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Services;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Requests.MainMenu;
using UseCase.MainMenu.GetDailyStatisticMenu;
using EmrCloudApi.Presenters.MainMenu;
using UseCase.MainMenu.SaveDailyStatisticMenu;
using UseCase.MainMenu;

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
        var input = new GetDailyStatisticMenuInputData(HpId, request.GrpId);
        var output = _bus.Handle(input);

        var presenter = new GetDailyStatisticMenuPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetDailyStatisticMenuResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveDailyStatisticMenuList)]
    public ActionResult<Response<SaveDailyStatisticMenuResponse>> SaveList([FromBody] SaveDailyStatisticMenuRequest request)
    {
        var input = new SaveDailyStatisticMenuInputData(HpId, UserId, request.GrpId, ConvertToMenuItem(request));
        var output = _bus.Handle(input);

        var presenter = new SaveDailyStatisticMenuPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveDailyStatisticMenuResponse>>(presenter.Result);
    }

    private List<StatisticMenuItem> ConvertToMenuItem(SaveDailyStatisticMenuRequest request)
    {
        var result = request.StatisticMenuList.Select(menu => new StatisticMenuItem(
                                                                  menu.MenuId,
                                                                  request.GrpId,
                                                                  menu.ReportId,
                                                                  menu.SortNo,
                                                                  menu.MenuName,
                                                                  menu.IsPrint,
                                                                  menu.StaConfigList
                                                                      .Select(conf => new StaConfItem(
                                                                                          menu.MenuId,
                                                                                          conf.ConfId,
                                                                                          conf.Val
                                                                           )).ToList(),
                                                                  menu.IsDeleted
                                              )).ToList();
        return result;
    }
}