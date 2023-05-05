using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using EmrCloudApi.Services;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Requests.MainMenu;
using UseCase.MainMenu.GetStatisticMenu;
using EmrCloudApi.Presenters.MainMenu;
using UseCase.MainMenu.SaveStatisticMenu;
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

    [HttpGet(ApiPath.GetStatisticMenuList)]
    public ActionResult<Response<GetStatisticMenuResponse>> GetList([FromQuery] GetStatisticMenuRequest request)
    {
        var input = new GetStatisticMenuInputData(HpId, request.GrpId);
        var output = _bus.Handle(input);

        var presenter = new GetStatisticMenuPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetStatisticMenuResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveStatisticMenuList)]
    public ActionResult<Response<SaveStatisticMenuResponse>> SaveList([FromBody] SaveStatisticMenuRequest request)
    {
        var input = new SaveStatisticMenuInputData(HpId, UserId, request.GrpId, ConvertToMenuItem(request));
        var output = _bus.Handle(input);

        var presenter = new SaveStatisticMenuPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveStatisticMenuResponse>>(presenter.Result);
    }

    private List<StatisticMenuItem> ConvertToMenuItem(SaveStatisticMenuRequest request)
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