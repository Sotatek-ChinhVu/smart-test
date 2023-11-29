using Domain.Models.KensaIrai;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Insurance;
using EmrCloudApi.Presenters.MainMenu;
using EmrCloudApi.Requests.Insurance;
using EmrCloudApi.Requests.MainMenu;
using EmrCloudApi.Requests.MainMenu.RequestItem;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Insurance;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Services;
using Helper.Messaging;
using Helper.Messaging.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using UseCase.Core.Sync;
using UseCase.Insurance.FindPtHokenList;
using UseCase.MainMenu;
using UseCase.MainMenu.CreateDataKensaIraiRenkei;
using UseCase.MainMenu.DeleteKensaInf;
using UseCase.MainMenu.GetKensaCenterMstList;
using UseCase.MainMenu.GetKensaInf;
using UseCase.MainMenu.GetKensaIrai;
using UseCase.MainMenu.GetKensaIraiLog;
using UseCase.MainMenu.GetListQualification;
using UseCase.MainMenu.GetStaCsvMstModel;
using UseCase.MainMenu.GetStatisticMenu;
using UseCase.MainMenu.ImportKensaIrai;
using UseCase.MainMenu.KensaIraiReport;
using UseCase.MainMenu.RsvInfToConfirm;
using UseCase.MainMenu.SaveStaCsvMst;
using UseCase.MainMenu.SaveStatisticMenu;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class MainMenuController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    private static HttpClient _httpClient = new HttpClient();
    private readonly IConfiguration _configuration;
    private readonly IMessenger _messenger;

    public MainMenuController(IConfiguration configuration, UseCaseBus bus, IUserService userService, IMessenger messenger) : base(userService)
    {
        _bus = bus;
        _configuration = configuration;
        _messenger = messenger;
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

    [HttpGet(ApiPath.GetKensaIraiLog)]
    public ActionResult<Response<GetKensaIraiLogResponse>> GetKensaIraiLog([FromQuery] GetKensaIraiLogRequest request)
    {
        var input = new GetKensaIraiLogInputData(HpId, request.StartDate, request.EndDate);
        var output = _bus.Handle(input);
        var presenter = new GetKensaIraiLogLogPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetKensaIraiLogResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.KensaIraiReport)]
    public ActionResult<Response<KensaIraiReportResponse>> KensaIraiReport([FromBody] KensaIraiReportRequest request)
    {

        var input = new KensaIraiReportInputData(HpId, UserId, request.CenterCd, request.SystemDate, request.FromDate, request.ToDate, ConvertToListKensaIraiModel(request.KensaIraiList));
        var output = _bus.Handle(input);
        var presenter = new KensaIraiReportPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<KensaIraiReportResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetStaCsvMst)]
    public ActionResult<Response<GetStaCsvMstResponse>> GetStaCsvMst()
    {
        var input = new GetStaCsvMstInputData(HpId);
        var output = _bus.Handle(input);
        var presenter = new GetStaCsvMstPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetStaCsvMstResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveStaCsvMst)]
    public ActionResult<Response<SaveStaCsvMstResponse>> SaveStaCsvMst([FromBody] SaveStaCsvMstRequest request)
    {
        var input = new SaveStaCsvMstInputData(HpId, UserId, request.StaCsvMstModels);
        var output = _bus.Handle(input);
        var presenter = new SaveStaCsvMstPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<SaveStaCsvMstResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.ImportKensaIrai)]
    public void ImportKensaIrai()
    {
        try
        {
            _messenger.Register<KensaInfMessageStatus>(this, UpdateKensaInfMessageStatus);
            HttpContext.Response.ContentType = "application/json";

            var input = new ImportKensaIraiInputData(HpId, UserId, _messenger, Request.Body);
            _bus.Handle(input);
        }
        finally
        {
            _messenger.Deregister<KensaInfMessageStatus>(this, UpdateKensaInfMessageStatus);
            HttpContext.Response.Body.Close();
        }
    }

    [HttpGet(ApiPath.GetRsvInfToConfirm)]
    public ActionResult<Response<GetRsvInfToConfirmResponse>> GetRsvInfToConfirm([FromQuery] GetRsvInfToConfirmRequest request)
    {
        var input = new GetRsvInfToConfirmInputData(HpId, request.SinDate);
        var output = _bus.Handle(input);
        var presenter = new GetRsvInfToConfirmPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetRsvInfToConfirmResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetListQualificationInf)]
    public ActionResult<Response<GetListQualificationInfResponse>> GetListQualificationInf()
    {
        var input = new GetListQualificationInfInputData();
        var output = _bus.Handle(input);
        var presenter = new GetListQualificationInfPresenter();
        presenter.Complete(output);
        return new ActionResult<Response<GetListQualificationInfResponse>>(presenter.Result);
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
        List<KensaIraiDetailModel> kensaIraiDetailList = request.KensaIraiDetails
                                                                .Where(item => !string.IsNullOrEmpty(item.KensaItemCd))
                                                                .Select(item => new KensaIraiDetailModel(
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
                   request.UpdateDate,
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

    private List<KensaIraiModel> ConvertToListKensaIraiModel(List<KensaIraiReportRequestItem> requestItemList)
    {
        var result = requestItemList.Select(item => new KensaIraiModel(
                                                        item.SinDate,
                                                        item.RaiinNo,
                                                        item.IraiCd,
                                                        item.PtId,
                                                        item.PtNum,
                                                        item.Name,
                                                        item.KanaName,
                                                        item.Sex,
                                                        item.Birthday,
                                                        item.TosekiKbn,
                                                        item.SikyuKbn,
                                                        item.UpdateDate,
                                                        item.KensaIraiDetailList.Select(detail => new KensaIraiDetailModel(
                                                                                                      detail.RpNo,
                                                                                                      detail.RpEdaNo,
                                                                                                      detail.RowNo,
                                                                                                      detail.SeqNo,
                                                                                                      detail.KensaItemCd,
                                                                                                      detail.CenterItemCd,
                                                                                                      detail.KensaKana,
                                                                                                      detail.KensaName,
                                                                                                      detail.ContainerCd
                                                                                )).ToList()
                                    )).ToList();
        return result;
    }

    private void UpdateKensaInfMessageStatus(KensaInfMessageStatus status)
    {
        string result = "\n" + JsonSerializer.Serialize(status);
        var resultForFrontEnd = Encoding.UTF8.GetBytes(result.ToString());
        HttpContext.Response.Body.WriteAsync(resultForFrontEnd, 0, resultForFrontEnd.Length);
        HttpContext.Response.Body.FlushAsync();
    }
    #endregion
}