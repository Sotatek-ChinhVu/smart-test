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
using UseCase.MainMenu.GetKensaIrai;
using UseCase.MainMenu.GetKensaCenterMstList;
using UseCase.MainMenu.CreateDataKensaIraiRenkei;
using Domain.Models.KensaIrai;
using EmrCloudApi.Requests.MainMenu.RequestItem;
using UseCase.MainMenu.GetKensaInf;
using UseCase.MainMenu.DeleteKensaInf;

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

    [HttpGet(ApiPath.GetKensaIrai)]
    public ActionResult<Response<GetKensaIraiResponse>> GetKensaIrai([FromQuery] GetKensaIraiRequest request)
    {
        var input = new GetKensaIraiInputData(HpId, request.PtId, request.StartDate, request.EndDate, request.KensaCenterMstCenterCd, request.KensaCenterMstPrimaryKbn);
        var output = _bus.Handle(input);
        var presenter = new GetKensaIraiPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetKensaIraiResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetKensaInf)]
    public ActionResult<Response<GetKensaInfResponse>> GetKensaInf([FromQuery] GetKensaInfRequest request)
    {
        var input = new GetKensaInfInputData(HpId, request.StartDate, request.EndDate, request.CenterCd);
        var output = _bus.Handle(input);
        var presenter = new GetKensaInfPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetKensaInfResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.GetKensaIraiByList)]
    public ActionResult<Response<GetKensaIraiResponse>> GetKensaIraiByList([FromBody] GetKensaIraiByListRequest request)
    {
        var input = new GetKensaIraiInputData(HpId, request.KensaIraiByListRequest.Select(item => ConvertToKensaInfModel(item)).ToList());
        var output = _bus.Handle(input);
        var presenter = new GetKensaIraiPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetKensaIraiResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetKensaCenterMstList)]
    public ActionResult<Response<GetKensaCenterMstListResponse>> GetKensaCenterMstList()
    {
        var input = new GetKensaCenterMstListInputData(HpId);
        var output = _bus.Handle(input);
        var presenter = new GetKensaCenterMstListPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetKensaCenterMstListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.CreateDataKensaIraiRenkei)]
    public ActionResult<Response<CreateDataKensaIraiRenkeiResponse>> CreateDataKensaIraiRenkei([FromBody] CreateDataKensaIraiRenkeiRequest request)
    {
        var input = new CreateDataKensaIraiRenkeiInputData(HpId, UserId, request.KensaIraiList.Select(item => ConvertToKensaIraiModel(item)).ToList(), request.CenterCd, request.SystemDate, false);
        var output = _bus.Handle(input);
        var presenter = new CreateDataKensaIraiRenkeiPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<CreateDataKensaIraiRenkeiResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.ReCreateDataKensaIraiRenkei)]
    public ActionResult<Response<CreateDataKensaIraiRenkeiResponse>> ReCreateDataKensaIraiRenkei([FromBody] ReCreateDataKensaIraiRenkeiRequest request)
    {
        var input = new CreateDataKensaIraiRenkeiInputData(HpId, UserId, request.KensaIraiList.Select(item => ConvertToKensaIraiModel(item)).ToList(), string.Empty, request.SystemDate, true);
        var output = _bus.Handle(input);
        var presenter = new CreateDataKensaIraiRenkeiPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<CreateDataKensaIraiRenkeiResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.DeleteKensaInf)]
    public ActionResult<Response<DeleteKensaInfResponse>> DeleteKensaInf([FromBody] DeleteKensaInfRequest request)
    {
        var kensaInfList = request.KensaInfList
            .Select(item => new KensaInfModel(
                                item.PtId,
                                item.RaiinNo, 
                                item.IraiCd, 
                                item.KensaInfDetailList.Select(item => new KensaInfDetailModel(
                                                                           item.SeqNo, 
                                                                           item.PtId, 
                                                                           item.IraiCd))
                                                       .ToList()))
            .ToList();
        var input = new DeleteKensaInfInputData(HpId, UserId, kensaInfList);
        var output = _bus.Handle(input);
        var presenter = new DeleteKensaInfPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<DeleteKensaInfResponse>>(presenter.Result);
    }

    #region private function
    private KensaInfModel ConvertToKensaInfModel(KensaIraiByListRequestItem requestItem)
    {
        return new KensaInfModel(requestItem.PtId,
                                 requestItem.RaiinNo,
                                 requestItem.CenterCd,
                                 requestItem.PrimaryKbn,
                                 requestItem.IraiCd);
    }

    private KensaIraiModel ConvertToKensaIraiModel(KensaIraiRequestItem request)
    {
        List<KensaIraiDetailModel> kensaIraiDetailList = request.KensaIraiDetails.Select(item => new KensaIraiDetailModel(
                                                                                                     item.TenKensaItemCd,
                                                                                                     item.ItemCd,
                                                                                                     item.ItemName,
                                                                                                     item.KanaName1,
                                                                                                     item.CenterCd,
                                                                                                     item.KensaItemCd,
                                                                                                     item.CenterItemCd,
                                                                                                     item.KensaKana,
                                                                                                     item.KensaName,
                                                                                                     item.ContainerCd,
                                                                                                     item.RpNo,
                                                                                                     item.RpEdaNo,
                                                                                                     item.RowNo,
                                                                                                     item.SeqNo))
                                                                                  .ToList();
        return new KensaIraiModel(
                   request.SinDate,
                   request.RaiinNo,
                   request.IraiCd,
                   request.PtId,
                   request.PtNum,
                   request.Name,
                   request.KanaName,
                   request.Sex,
                   request.Birthday,
                   request.TosekiKbn,
                   request.SikyuKbn,
                   kensaIraiDetailList);
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

    #endregion
}