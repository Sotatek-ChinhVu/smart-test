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
using System.Text;
using System.Text.Json;
using EmrCloudApi.Responses.Insurance;
using UseCase.Insurance.FindPtHokenList;
using EmrCloudApi.Requests.Insurance;
using EmrCloudApi.Presenters.Insurance;
using UseCase.Insurance.FindHokenInfByPtId;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class MainMenuController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    private static HttpClient _httpClient = new HttpClient();
    private readonly IConfiguration _configuration;

    public MainMenuController(IConfiguration configuration, UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
        _configuration = configuration;
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

    [HttpGet(ApiPath.GetListStaticReport)]
    public ActionResult<Response<GetListReportResponse>> GetListStaticReport()
    {
        GetListReportRequest request = new GetListReportRequest("Statistics");
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        string basePath = _configuration.GetSection("RenderPdf")["BasePath"]!;

        List<string> fileNameList = new();
        using (HttpResponseMessage response = _httpClient.PostAsync($"{basePath}{"getListReport"}", jsonContent).Result)
        {
            response.EnsureSuccessStatusCode();
            var contentResult = response.Content.ReadAsStringAsync().Result;
            fileNameList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(contentResult) ?? new();
        }
        Response<GetListReportResponse> result = new();
        result.Data = new GetListReportResponse(fileNameList);
        result.Message = ResponseMessage.Success;
        result.Status = 1;
        return result;
    }

    [HttpGet(ApiPath.FindPtHokenList)]
    public ActionResult<Response<FindPtHokenListResponse>> FindPtHokenList([FromQuery] FindPtHokenListRequest request)
    {
        var input = new FindPtHokenListInputData(HpId, request.PtId, request.SinDate);
        var output = _bus.Handle(input);
        var presenter = new FindPtHokenListPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<FindPtHokenListResponse>>(presenter.Result);
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
                                                                  menu.IsDeleted,
                                                                  menu.IsSaveTemp
                                              )).ToList();
        return result;
    }


}