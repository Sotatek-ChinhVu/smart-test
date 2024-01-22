using Domain.Models.UserToken;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.DrugInfor;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Requests.DrugInfor;
using EmrCloudApi.Requests.ExportPDF;
using EmrCloudApi.Requests.File;
using EmrCloudApi.Requests.KensaHistory;
using EmrCloudApi.Requests.MedicalExamination;
using EmrCloudApi.Requests.PatientManagement;
using Helper.Enum;
using Helper.Extension;
using Helper.Redis;
using Interactor.DrugInfor.CommonDrugInf;
using Interactor.MedicalExamination.HistoryCommon;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using Reporting.Accounting.Model;
using Reporting.Accounting.Model.Output;
using Reporting.DrugInfo.Model;
using Reporting.GrowthCurve.Model;
using Reporting.KensaLabel.Model;
using Reporting.Mappers.Common;
using Reporting.PatientManagement.Models;
using Reporting.ReceiptList.Model;
using Reporting.ReportServices;
using StackExchange.Redis;
using System.Drawing;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Web;
using UseCase.DrugInfor.GetDataPrintDrugInfo;
using UseCase.MedicalExamination.GetDataPrintKarte2;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class PdfCreatorController : CookieController
{
    private static HttpClient _httpClient = new HttpClient();
    private readonly IReportService _reportService;
    private readonly IConfiguration _configuration;
    private readonly IHistoryCommon _historyCommon;
    private readonly IGetCommonDrugInf _commonDrugInf;
    private readonly IDatabase _cache;
    private readonly string NoDataMessage = @"<meta charset=""utf-8"">
                                              <title>印刷対象が見つかりません。</title>
                                              <p style='text-align: center;font-size: 25px;font-weight: 300'>印刷対象が見つかりません。</p>
                                              ";
    private readonly string NotAuhorize = @"<meta charset=""utf-8"">
                                              <title>エラー</title>
                                              <p style='text-align: center;font-size: 25px;font-weight: 300'>閲覧権限がないため、ファイルを閲覧することができません。
                                                                                                             SmartKarteにログインしてから、再度読み込みしてください。</p>
                                              ";

    public PdfCreatorController(IReportService reportService, IConfiguration configuration, IHistoryCommon historyCommon, IGetCommonDrugInf commonDrugInf, IHttpContextAccessor httpContextAccessor, IUserTokenRepository userTokenRepository) : base(httpContextAccessor, userTokenRepository)
    {
        _reportService = reportService;
        _configuration = configuration;
        _historyCommon = historyCommon;
        _commonDrugInf = commonDrugInf;
        GetRedis();
        _cache = RedisConnectorHelper.Connection.GetDatabase();
    }

    private void GetRedis()
    {
        string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
        if (RedisConnectorHelper.RedisHost != connection)
        {
            RedisConnectorHelper.RedisHost = connection;
        }
    }

    [HttpGet(ApiPath.ExportKarte1)]
    public async Task<IActionResult> GenerateKarte1Report([FromQuery] Karte1ExportRequest request)
    {
        _reportService.Instance(5);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var karte1Data = _reportService.GetKarte1ReportingData(HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei, request.SyuByomei);
        _reportService.ReleaseResource();
        return await RenderPdf(karte1Data, ReportType.Common, karte1Data.JobName);
    }

    [HttpGet(ApiPath.ExportNameLabel)]
    public async Task<IActionResult> GenerateNameLabelReport([FromQuery] NameLabelExportRequest request)
    {
        _reportService.Instance(6);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetNameLabelReportingData(request.PtId, request.KanjiName, request.SinDate);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ExportDrugInfo)]
    public async Task<IActionResult> GenerateDrugInfReport([FromQuery] DrugInfoExportRequest request)
    {
        _reportService.Instance(2);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var drugInfo = _reportService.SetOrderInfo(HpId, request.PtId, request.SinDate, request.RaiinNo);
        _reportService.ReleaseResource();
        return await RenderPdf(drugInfo, ReportType.DrugInfo, "薬情.pdf");
    }

    [HttpGet(ApiPath.ExportByomei)]
    public async Task<IActionResult> GenerateByomeiReport([FromQuery] ByomeiExportRequest request)
    {
        _reportService.Instance(4);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var byomeiData = _reportService.GetByomeiReportingData(HpId, request.PtId, request.FromDay, request.ToDay, request.TenkiIn, request.HokenIdList);
        _reportService.ReleaseResource();
        return await RenderPdf(byomeiData, ReportType.Common, byomeiData.JobName);
    }

    [HttpGet(ApiPath.ExportOrderLabel)]
    public async Task<IActionResult> GenerateOrderLabelReport([FromQuery] OrderLabelExportRequest request)
    {
        _reportService.Instance(1);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        List<(int from, int to)> odrKouiKbns = new();
        foreach (var item in request.OdrKouiKbns)
        {
            odrKouiKbns.Add(new(item.From, item.To));
        }
        var data = _reportService.GetOrderLabelReportingData(0, HpId, request.PtId, request.SinDate, request.RaiinNo, odrKouiKbns, new());
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ExportSijisen)]
    public async Task<IActionResult> GenerateSijisenReport([FromQuery] SijisenExportRequest request)
    {
        _reportService.Instance(3);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var odrKouiKbns = new List<(int from, int to)>();
        foreach (var item in request.OdrKouiKbns)
        {
            odrKouiKbns.Add(new(item.From, item.To));
        }
        var sijisenData = _reportService.GetSijisenReportingData(HpId, request.FormType, request.PtId, request.SinDate, request.RaiinNo, odrKouiKbns, request.PrintNoOdr);
        _reportService.ReleaseResource();
        return await RenderPdf(sijisenData, ReportType.Common, sijisenData.JobName);
    }

    [HttpGet(ApiPath.MedicalRecordWebId)]
    public async Task<IActionResult> GenerateMedicalRecordWebIdReport([FromQuery] MedicalRecordWebIdRequest request)
    {
        _reportService.Instance(7);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetMedicalRecordWebIdReportingData(HpId, request.PtId, request.SinDate);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.OutDrug)]
    public async Task<IActionResult> GetOutDrugReportingData([FromQuery] OutDrugRequest request)
    {
        _reportService.Instance(10);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetOutDrugReportingData(HpId, request.PtId, request.SinDate, request.RaiinNo);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ReceiptCheck)]
    public async Task<IActionResult> GetReceiptCheckReport([FromQuery] ReceiptCheckRequest request)
    {
        _reportService.Instance(8);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetReceiptCheckCoReportService(HpId, request.PtIds, request.SeikyuYm);
        if (string.IsNullOrEmpty(data.DataJsonConverted))
        {
            return await RenderPdf(data, ReportType.Common, data.JobName);
        }
        _reportService.ReleaseResource();
        return await RenderPdf(data.DataJsonConverted, ReportType.Common, data.JobName);
    }

    [HttpPost(ApiPath.ReceiptList)]
    public async Task<IActionResult> GetReceiptListReport([FromForm] AccountingReportRequest requestStringJson)
    {
        _reportService.Instance(9);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var stringJson = requestStringJson.JsonAccounting;
        var request = JsonSerializer.Deserialize<GetReceiptListRequest>(stringJson) ?? new();

        var receInputList = request.ReceiptListModels.Select(item => new ReceiptInputModel(
                                                                         item.SinYm,
                                                                         item.PtId,
                                                                         item.HokenId))
                                                     .ToList();

        var data = _reportService.GetReceiptListReportingData(HpId, request.SeikyuYm, receInputList);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpPost(ApiPath.PeriodReceiptReport)]
    public async Task<IActionResult> PeriodReceiptReport([FromForm] AccountingReportRequest requestStringJson)
    {
        _reportService.Instance(11);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var stringJson = requestStringJson.JsonAccounting;
        var request = JsonSerializer.Deserialize<PeriodReceiptListRequest>(stringJson) ?? new();
        List<(int grpId, string grpCd)> grpConditions = new();
        foreach (var item in request.GrpConditions)
        {
            grpConditions.Add(new(item.GrpId, item.GrpCd));
        }
        var data = _reportService.GetPeriodPrintData(HpId, request.StartDate, request.EndDate, request.SourcePt, grpConditions, request.PrintSort, request.IsPrintList, request.PrintByMonth, request.PrintByGroup, request.MiseisanKbn, request.SaiKbn, request.MisyuKbn, request.SeikyuKbn, request.HokenKbn, request.HakkoDay, request.Memo, request.FormFileName, request.NyukinBase);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Accounting, data.JobName);
    }

    [HttpPost(ApiPath.AccountingReport)]
    public async Task<IActionResult> GenerateAccountingReport([FromForm] AccountingReportRequest requestStringJson)
    {
        _reportService.Instance(23);
        _reportService.Instance(11);
        //if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var stringJson = requestStringJson.JsonAccounting;
        var request = JsonSerializer.Deserialize<AccountingCoReportModelRequest>(stringJson) ?? new();
        var multiAccountDueListModels = request.MultiAccountDueListModels.Select(item => ConvertToCoAccountDueListModel(item)).ToList();
        var data = _reportService.GetAccountingData(HpId, request.Mode, request.PtId, multiAccountDueListModels, request.IsPrintMonth, request.Ryoshusho, request.Meisai);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Accounting, data.JobName);
    }

    [HttpGet(ApiPath.ReceiptReport)]
    public async Task<IActionResult> GenerateReceiptReport([FromQuery] ReceiptExportRequest request)
    {
        _reportService.Instance(11);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetAccountingReportingData(HpId, request.PtId, request.PrintType, request.RaiinNoList, request.RaiinNoPayList, request.IsCalculateProcess);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Accounting, data.JobName);
    }

    [HttpGet(ApiPath.GrowthCurve)]
    public async Task<IActionResult> GetGrowthCurvePrintData([FromQuery] GrowthCurvePrintDataRequest request)
    {
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        CommonReportingRequestModel data;
        if (request.Type == 0)
        {
            _reportService.Instance(28);
            data = _reportService.GetGrowthCurveA5PrintData(HpId, new GrowthCurveConfig(request.PtNum, request.PtId, request.PtName, request.Sex, request.BirthDay, request.PrintMode, request.PrintDate, request.WeightVisible, request.HeightVisible, request.Per50, request.Per25, request.Per10, request.Per3, request.SDAvg, request.SD1, request.SD2, request.SD25, request.Legend, request.Scope));
            _reportService.ReleaseResource();
        }
        else
        {
            _reportService.Instance(27);
            data = _reportService.GetGrowthCurveA4PrintData(HpId, new GrowthCurveConfig(request.PtNum, request.PtId, request.PtName, request.Sex, request.BirthDay, request.PrintMode, request.PrintDate, request.WeightVisible, request.HeightVisible, request.Per50, request.Per25, request.Per10, request.Per3, request.SDAvg, request.SD1, request.SD2, request.SD25, request.Legend, request.Scope));
            _reportService.ReleaseResource();
        }
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.StaticReport)]
    public async Task<IActionResult> GenerateStatisticReport([FromQuery] StatisticExportRequest request)
    {
        _reportService.Instance(12);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetStatisticReportingData(HpId, request.FormName, request.MenuId, request.MonthFrom, request.MonthTo, request.DateFrom, request.DateTo, request.TimeFrom, request.TimeTo, request.CoFileType, request.IsPutTotalRow, request.TenkiDateFrom, request.TenkiDateTo, request.EnableRangeFrom, request.EnableRangeTo, request.PtNumFrom, request.PtNumTo);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpPost(ApiPath.PatientManagement)]
    public async Task<IActionResult> GeneratePatientManagement([FromForm] StringObjectRequest requestString)
    {
        _reportService.Instance(14);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var request = JsonSerializer.Deserialize<PatientManagementRequest>(requestString.StringJson) ?? new();
        PatientManagementModel patientManagementModel = ConvertToPatientManagementModel(request.PatientManagement);
        var data = _reportService.GetPatientManagement(HpId, patientManagementModel);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ReceiptPreview)]
    public async Task<IActionResult> ReceiptPreview([FromQuery] ReceiptPreviewRequest request)
    {
        _reportService.Instance(13);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetReceiptData(HpId, request.PtId, request.SinYm, request.HokenId, request.SeiKyuYm, request.HokenKbn, request.IsIncludeOutDrug, request.IsModePrint, request.isOpenedFromAccounting);
        var result = await RenderPdf(data, ReportType.Common, data.JobName);
        _reportService.ReleaseResource();
        return result;
    }

    [HttpGet(ApiPath.SyojyoSyoki)]
    public async Task<IActionResult> SyojyoSyoki([FromQuery] SyojyoSyokiRequest request)
    {
        _reportService.Instance(15);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetSyojyoSyokiReportingData(HpId, request.PtId, request.SeiKyuYm, request.HokenId);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.Kensalrai)]
    public async Task<IActionResult> Kensalrai([FromQuery] KensalraiRequest request)
    {
        _reportService.Instance(16);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetKensalraiData(HpId, request.SystemDate, request.FromDate, request.ToDate, request.CenterCd);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ReceiptPrint)]
    public async Task<IActionResult> ReceiptPrint([FromQuery] ReceiptPrintRequest request)
    {
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        if (request.PrintType == 0)
        {
            _reportService.Instance(17);
            var data = _reportService.GetReceiptPrint(HpId, request.FormName, request.PrefNo, request.ReportId, request.ReportEdaNo, request.DataKbn, request.PtId, request.SeikyuYm, request.SinYm, request.HokenId, request.DiskKind, request.DiskCnt, request.WelfareType, request.PrintHokensyaNos, request.HokenKbn, request.SelectedReseputoShubeusu, request.DepartmentId, request.DoctorId, request.PrintNoFrom, request.PrintNoTo, request.IncludeTester, request.IsIncludeOutDrug, request.Sort, request.PrintPtIds);
            _reportService.ReleaseResource();
            return await RenderPdf(data, ReportType.Common, data.JobName);
        }
        else
        {
            _reportService.Instance(30);
            var data = _reportService.GetReceiptPrintExcel(HpId, request.PrefNo, request.ReportId, request.ReportEdaNo, request.DataKbn, request.SeikyuYm);
            _reportService.ReleaseResource();
            return RenderCsv(data);
        }
    }

    [HttpGet(ApiPath.KensaHistoryReport)]
    public async Task<IActionResult> KensaHistoryReport([FromQuery] KensaHistoryReportRequest request)
    {
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        if (request.IraiDate != 0)
        {
            _reportService.Instance(34);
            var data = _reportService.GetKensaHistoryPrint(HpId, request.UserId, request.PtId, request.SetId, request.IraiDate, request.StartDate, request.EndDate, request.ShowAbnormalKbn, request.SinDate);
            _reportService.ReleaseResource();
            return await RenderPdf(data, ReportType.Common, data.JobName);
        }
        else
        {
            _reportService.Instance(35);
            var data = _reportService.GetKensaResultMultiPrint(HpId, request.UserId, request.PtId, request.SetId, request.StartDate, request.EndDate, request.ShowAbnormalKbn, request.SinDate);
            _reportService.ReleaseResource();
            return await RenderPdf(data, ReportType.Common, data.JobName);
        }
    }

    [HttpPost(ApiPath.MemoMsgPrint)]
    public async Task<IActionResult> MemoMsgPrint([FromForm] StringObjectRequest requestString)
    {
        _reportService.Instance(18);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var request = JsonSerializer.Deserialize<MemoMsgPrintRequest>(requestString.StringJson) ?? new();
        var data = _reportService.GetMemoMsgReportingData(request.ReportName, request.Title, request.ListMessage);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, request.FileName);
    }

    [HttpGet(ApiPath.ReceTarget)]
    public async Task<IActionResult> ReceTarget([FromQuery] ReceTargetRequest request)
    {
        _reportService.Instance(19);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetReceTargetPrint(HpId, request.SeikyuYm);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.DrugNoteSeal)]
    public async Task<IActionResult> GetDrugNoteSealPrintData([FromQuery] DrugNoteSealPrintDataRequest request)
    {
        _reportService.Instance(20);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetDrugNoteSealPrintData(HpId, request.PtId, request.SinDate, request.RaiinNo);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.InDrug)]
    public async Task<IActionResult> GetInDrugPrintData([FromQuery] InDrugPrintDataRequest request)
    {
        _reportService.Instance(26);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetInDrugPrintData(HpId, request.PtId, request.SinDate, request.RaiinNo);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.Yakutai)]
    public async Task<IActionResult> Yakutai([FromQuery] YakutaiRequest request)
    {
        _reportService.Instance(21);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetYakutaiReportingData(HpId, request.PtId, request.SinDate, request.RaiinNo);
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.KensaLabel)]
    public async Task<IActionResult> KensaLabel([FromQuery] KensaLabelRequest request)
    {
        _reportService.Instance(29);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetKensaLabelPrintData(HpId, request.PtId, request.RaiinNo, request.SinDate, new KensaPrinterModel(request.ItemCd, request.ContainerName, request.ContainerCd, request.Count, request.InoutKbn, request.OdrKouiKbn));
        _reportService.ReleaseResource();
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.AccountingCard)]
    public async Task<IActionResult> GetAccountingCardPrintData([FromQuery] AccountingCardReportingRequest request)
    {
        _reportService.Instance(22);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetAccountingCardReportingData(HpId, request.PtId, request.SinYm, request.HokenId, request.IncludeOutDrug);
        _reportService.ReleaseResource();

        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ExportKarte3)]
    public async Task<IActionResult> GetKarte3ReportingData([FromQuery] Karte3ReportingRequest request)
    {
        _reportService.Instance(24);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var data = _reportService.GetKarte3ReportingData(HpId, request.PtId, request.StartSinYm, request.EndSinYm, request.IncludeHoken, request.IncludeJihi);
        _reportService.ReleaseResource();

        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpPost(ApiPath.AccountingCardList)]
    public async Task<IActionResult> GetAccountingCardListReportingData([FromForm] StringObjectRequest requestString)
    {
        _reportService.Instance(25);
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var request = JsonSerializer.Deserialize<AccountingCardListRequest>(requestString.StringJson) ?? new();
        var data = _reportService.GetAccountingCardListReportingData(HpId, request.Targets, request.IncludeOutDrug, request.KaName, request.TantoName, request.UketukeSbt, request.Hoken);
        _reportService.ReleaseResource();

        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.SetDownloadNameReport)]
    public IActionResult SetDownloadNameReportReportingData([FromQuery] SetDownloadNameReportRequest request)
    {
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        string key = "KensaIraiPdfReport_" + request.KeyReport;
        ContentDisposition cd = new ContentDisposition
        {
            FileName = HttpUtility.UrlEncode(request.DownloadName),
            Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
        };
        Response.Headers.Add("Content-Disposition", cd.ToString());
        if (!_cache.KeyExists(key))
        {
            return Content(NoDataMessage, "text/html");
        }
        var result = _cache.StringGet(key);
        return File(Convert.FromBase64String(result.AsString()), "application/pdf");
    }

    [HttpGet(ApiPath.ExportKarte2)]
    public async Task<IActionResult> GenerateKarte2Report([FromQuery] GetDataPrintKarte2Request request)
    {
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var inputData = new GetDataPrintKarte2InputData(request.PtId, HpId, request.SinDate, request.StartDate, request.EndDate, request.IsCheckedHoken, request.IsCheckedJihi, request.IsCheckedHokenJihi, request.IsCheckedJihiRece, request.IsCheckedHokenRousai, request.IsCheckedHokenJibai, request.IsCheckedDoctor, request.IsCheckedStartTime, request.IsCheckedVisitingTime, request.IsCheckedEndTime, request.IsUketsukeNameChecked, request.IsCheckedSyosai, request.IsIncludeTempSave, request.IsCheckedApproved, request.IsCheckedInputDate, request.IsCheckedSetName, request.DeletedOdrVisibilitySetting, request.IsIppanNameChecked, request.IsCheckedHideOrder, request.EmptyMode);

        var outputData = _historyCommon.GetDataKarte2(inputData);

        var present = new GetDataPrintKarte2Presenter();
        present.Complete(outputData);

        var stringKarte2Result = JsonSerializer.Serialize(present.Result);

        string baseUrl = _configuration.GetSection("Karte2TemplateDefault").Value!;

        using (var clientResponse = await _httpClient.GetAsync(baseUrl))
        {
            byte[] bytes = await clientResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            using (var memoryStream = new MemoryStream())
            {
                string decoded = Encoding.UTF8.GetString(bytes);

                decoded = decoded.Replace("__DATA_KARTE2__", stringKarte2Result);

                bytes = Encoding.UTF8.GetBytes(decoded);
            }

            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new StringContent("0"), "marginTop");
            form.Add(new StringContent("0".ToString()), "marginBottom");
            form.Add(new StringContent("0".ToString()), "marginLeft");
            form.Add(new StringContent("0".ToString()), "marginRight");
            form.Add(new StringContent("8.27"), "paperWidth");
            form.Add(new StringContent("11.7"), "paperHeight");
            form.Add(new StringContent("window.status === 'ready'"), "waitForExpression");
            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "files", "index.html");

            string basePath = _configuration.GetSection("RenderKarte2ReportApi")["BasePath"]!;

            using (HttpResponseMessage response = await _httpClient.PostAsync($"{basePath}", form))
            {
                response.EnsureSuccessStatusCode();

                using (var streamingData = (MemoryStream)response.Content.ReadAsStream())
                {
                    var byteData = streamingData.ToArray();
                    var result = SetTitleMetadata(byteData, "カルテ２号紙.pdf");
                    ContentDisposition cd = new ContentDisposition
                    {
                        FileName = "カルテ２号紙.pdf",
                        Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
                    };
                    Response.Headers.Add("Content-Disposition", cd.ToString());
                    return File(result, "application/pdf");
                }
            }
        }
    }

    [HttpGet(ApiPath.GetDataPrintDrugInfo)]
    public async Task<IActionResult> GetDataPrintDrugInfo([FromQuery] GetDataPrintDrugInfoRequest request)
    {
        // if HpId = -1, return 401 page
        if (HpId == -1)
        {
            return Content(NotAuhorize, "text/html");
        }
        var inputData = new GetDataPrintDrugInfoInputData(HpId, request.SinDate, request.ItemCd, request.Level, string.Empty, request.YJCode, request.Type);

        var drugInfo = _commonDrugInf.GetDrugInforModel(inputData.HpId, inputData.SinDate, inputData.ItemCd);
        string htmlData = string.Empty;
        switch (inputData.Type)
        {
            case TypeHTMLObject.ShowProductInf:
                htmlData = _commonDrugInf.ShowProductInf(inputData.HpId, inputData.SinDate, inputData.ItemCd, inputData.Level, inputData.DrugName, inputData.YJCode);
                break;
            case TypeHTMLObject.ShowKanjaMuke:
                htmlData = _commonDrugInf.ShowKanjaMuke(inputData.ItemCd, inputData.Level, inputData.DrugName, inputData.YJCode);
                break;
            case TypeHTMLObject.ShowMdbByomei:
                htmlData = _commonDrugInf.ShowMdbByomei(inputData.ItemCd, inputData.Level, inputData.DrugName, inputData.YJCode);
                break;
        }
        var outputData = new GetDataPrintDrugInfoOutputData(drugInfo, htmlData, (int)inputData.Type);

        var present = new GetDataPrintDrugInfoPresenter();
        present.Complete(outputData);

        var stringPrintDrugInfoResult = JsonSerializer.Serialize(present.Result);

        string baseUrl = _configuration.GetSection("DrugInfoTemplateDefault").Value!;

        using (var clientResponse = await _httpClient.GetAsync(baseUrl))
        {
            byte[] bytes = await clientResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

            using (var memoryStream = new MemoryStream())
            {
                string decoded = Encoding.UTF8.GetString(bytes);

                decoded = decoded.Replace("__DATA_DRUG_INFORMATION__", stringPrintDrugInfoResult);

                bytes = Encoding.UTF8.GetBytes(decoded);
            }

            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new StringContent("0.7"), "marginTop");
            form.Add(new StringContent("0.7".ToString()), "marginBottom");
            form.Add(new StringContent("0.7".ToString()), "marginLeft");
            form.Add(new StringContent("0.7".ToString()), "marginRight");
            form.Add(new StringContent("8.27"), "paperWidth");
            form.Add(new StringContent("11.7"), "paperHeight");
            form.Add(new StringContent("window.status === 'ready'"), "waitForExpression");
            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "files", "index.html");

            string basePath = _configuration.GetSection("RenderKarte2ReportApi")["BasePath"]!;

            using (HttpResponseMessage response = await _httpClient.PostAsync($"{basePath}", form))
            {
                response.EnsureSuccessStatusCode();

                using (var streamingData = (MemoryStream)response.Content.ReadAsStream())
                {
                    var byteData = streamingData.ToArray();
                    var result = SetTitleMetadata(byteData, "医薬品情報.pdf");
                    ContentDisposition cd = new ContentDisposition
                    {
                        FileName = "医薬品情報.pdf",
                        Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
                    };
                    Response.Headers.Add("Content-Disposition", cd.ToString());
                    return File(result, "application/pdf");
                }
            }
        }
    }

    [HttpGet(ApiPath.ResizeImage)]
    public async Task<IActionResult> Resize([FromQuery] ResizeImageRequest request)
    {
        try
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(request.ImagePath))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return BadRequest($"Failed to download image from {request.ImagePath}");
                    }

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var originalImage = Image.FromStream(stream))
                        {
                            int newWidth = (int)Math.Floor((double)originalImage.Width / originalImage.Height * request.Height);

                            using (var resizedImage = new Bitmap(originalImage, new Size(newWidth, request.Height)))
                            {
                                var resultStream = new MemoryStream();
                                resizedImage.Save(resultStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                resultStream.Position = 0;

                                return File(resultStream, "image/jpeg");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            return BadRequest("Error resizing image");
        }
    }

    #region private function
    private byte[] SetTitleMetadata(byte[] pdf, string title)
    {
        using var inputStream = new MemoryStream(pdf);
        using var reader = new PdfReader(inputStream);
        using var outputStream = new MemoryStream();
        using var writer = new PdfWriter(outputStream);
        using (var document = new PdfDocument(reader, writer))
        {
            var documentInfo = document.GetDocumentInfo();

            documentInfo.SetTitle(title);
        }

        // The PdfDocument must be closed first to write to the output stream
        return outputStream.ToArray();
    }

    private async Task<IActionResult> RenderPdf(CommonReportingRequestModel data, ReportType reportType, string fileName)
    {
        bool returnNoData = false;
        if (data.ReportType <= 0
            || (!data.TableFieldData.Any()
                && !data.ListTextData.Any()
                && !data.SingleFieldList.Any()
                && !data.SetFieldData.Any()
                && data.ReportType != (int)CoReportType.MemoMsg))
        {
            returnNoData = true;
        }
        return await ActionReturnPDF(returnNoData, data, reportType, fileName);
    }

    private async Task<IActionResult> RenderPdf(string data, ReportType reportType, string fileName)
    {
        StringContent jsonContent = new StringContent(data, Encoding.UTF8, "application/json");

        string basePath = _configuration.GetSection("RenderPdf")["BasePath"]!;

        string functionName = reportType switch
        {
            ReportType.DrugInfo => "reporting-fm-drugInfo",
            ReportType.Common => "common-reporting",
            ReportType.OutDug => "reporting-out-drug",
            ReportType.Accounting => "reporting-accounting",
            _ => throw new NotImplementedException($"The reportType is incorrect: {reportType}")
        } ?? string.Empty;

        using (HttpResponseMessage response = await _httpClient.PostAsync($"{basePath}{functionName}", jsonContent))
        {
            response.EnsureSuccessStatusCode();
            fileName = fileName.Replace(".rse", "").Replace(".pdf", "") + ".pdf";
            ContentDisposition cd = new ContentDisposition
            {
                FileName = HttpUtility.UrlEncode(fileName),
                Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            using (var streamingData = (MemoryStream)response.Content.ReadAsStream())
            {
                var byteData = streamingData.ToArray();

                return File(byteData, "application/pdf");
            }
        }
    }

    private async Task<IActionResult> RenderPdf(DrugInfoData data, ReportType reportType, string fileName)
    {
        bool returnNoData = !data.DrugInfoList.Any();
        return await ActionReturnPDF(returnNoData, data, reportType, fileName);
    }

    private async Task<IActionResult> RenderPdf(AccountingResponse data, ReportType reportType, string fileName)
    {
        bool returnNoData = !data.AccountingReportingRequestItems.Any();
        return await ActionReturnPDF(returnNoData, data, reportType, fileName);
    }

    private async Task<IActionResult> ActionReturnPDF(bool returnNoData, object data, ReportType reportType, string fileName)
    {
        if (returnNoData)
        {
            return Content(NoDataMessage, "text/html");
        }

        StringContent jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

        string basePath = _configuration.GetSection("RenderPdf")["BasePath"]!;

        string functionName = reportType switch
        {
            ReportType.DrugInfo => "reporting-fm-drugInfo",
            ReportType.Common => "common-reporting",
            ReportType.Accounting => "reporting-accounting",
            _ => throw new NotImplementedException($"The reportType is incorrect: {reportType}")
        } ?? string.Empty;

        using (HttpResponseMessage response = await _httpClient.PostAsync($"{basePath}{functionName}", jsonContent))
        {
            response.EnsureSuccessStatusCode();
            fileName = fileName.Replace(".rse", "").Replace(".pdf", "") + ".pdf";
            ContentDisposition cd = new ContentDisposition
            {
                FileName = HttpUtility.UrlEncode(fileName),
                Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            using (var streamingData = (MemoryStream)response.Content.ReadAsStream())
            {
                var byteData = streamingData.ToArray();
                return File(byteData, "application/pdf");
            }
        }
    }

    private CoAccountDueListModel ConvertToCoAccountDueListModel(CoAccountDueListRequestModel request)
    {
        return new CoAccountDueListModel(
                   request.SinDate,
                   0,
                   request.RaiinNo,
                   request.OyaRaiinNo
               );
    }

    private IActionResult RenderCsv(CommonExcelReportingModel dataModel)
    {
        var dataList = dataModel.Data;
        if (!dataList.Any())
        {
            return Content(NoDataMessage, "text/html");
        }
        var csv = new StringBuilder();

        string contentType = "text/csv";

        foreach (var row in dataList)
        {
            csv.AppendLine(row);
        }
        var content = Encoding.UTF8.GetBytes(csv.ToString());
        var result = Encoding.UTF8.GetPreamble().Concat(content).ToArray();
        return File(result, contentType, dataModel.FileName);
    }

    private PatientManagementModel ConvertToPatientManagementModel(PatientManagementItem requestItem)
    {
        PatientManagementModel result = new();
        result.OutputOrder = requestItem.OutputOrder1;
        result.OutputOrder2 = requestItem.OutputOrder2;
        result.OutputOrder3 = requestItem.OutputOrder3;
        result.ReportType = requestItem.ReportType;
        result.PtNumFrom = requestItem.PtNumFrom;
        result.PtNumTo = requestItem.PtNumTo;
        result.KanaName = requestItem.KanaName;
        result.Name = requestItem.Name;
        result.BirthDayFrom = requestItem.BirthDayFrom;
        result.BirthDayTo = requestItem.BirthDayTo;
        result.AgeFrom = requestItem.AgeFrom;
        result.AgeTo = requestItem.AgeTo;
        result.AgeRefDate = requestItem.AgeRefDate.AsInteger();
        result.Sex = requestItem.Sex;
        if (!string.IsNullOrEmpty(requestItem.HomePost))
        {
            var homePost = requestItem.HomePost.Replace("-", string.Empty);
            if (homePost.Length > 3)
            {
                result.ZipCD1 = homePost.Substring(0, 3);
                result.ZipCD2 = homePost.Substring(3);
            }
        }
        result.Address = requestItem.Address.AsString();
        result.PhoneNumber = requestItem.PhoneNumber.AsString();
        result.RegistrationDateFrom = requestItem.RegistrationDateFrom;
        result.RegistrationDateTo = requestItem.RegistrationDateTo;
        result.IncludeTestPt = requestItem.IncludeTestPt;
        result.GroupSelected = requestItem.GroupSelected.AsString();
        result.HokensyaNoFrom = requestItem.HokensyaNoFrom.AsString();
        result.HokensyaNoTo = requestItem.HokensyaNoTo.AsString();
        result.Kigo = requestItem.Kigo.AsString();
        result.EdaNo = requestItem.EdaNo.AsString();
        result.Bango = requestItem.Bango.AsString();
        result.HokenKbn = requestItem.HokenKbn;
        result.KohiFutansyaNoFrom = requestItem.KohiFutansyaNoFrom.AsString();
        result.KohiFutansyaNoTo = requestItem.KohiFutansyaNoTo.AsString();
        result.KohiTokusyuNoFrom = requestItem.KohiTokusyuNoFrom.AsString();
        result.KohiTokusyuNoTo = requestItem.KohiTokusyuNoTo.AsString();
        result.ExpireDateFrom = requestItem.ExpireDateFrom;
        result.ExpireDateTo = requestItem.ExpireDateTo;
        result.HokenSbt = requestItem.HokenSbt;
        result.Houbetu1 = requestItem.Houbetu1;
        result.Houbetu2 = requestItem.Houbetu2;
        result.Houbetu3 = requestItem.Houbetu3;
        result.Houbetu4 = requestItem.Houbetu4;
        result.Houbetu5 = requestItem.Houbetu5;
        result.Kogaku = requestItem.Kogaku.AsString();
        result.KohiHokenNoFrom = requestItem.KohiHokenNoFrom;
        result.KohiHokenEdaNoFrom = requestItem.KohiHokenEdaNoFrom;
        result.KohiHokenNoTo = requestItem.KohiHokenNoTo;
        result.KohiHokenEdaNoTo = requestItem.KohiHokenEdaNoTo;
        result.StartDateFrom = requestItem.StartDateFrom;
        result.StartDateTo = requestItem.StartDateTo;
        result.TenkiDateFrom = requestItem.TenkiDateFrom;
        result.TenkiDateTo = requestItem.TenkiDateTo;
        result.TenkiKbns = requestItem.TenkiKbns;
        result.SikkanKbns = requestItem.SikkanKbns;
        result.NanbyoCds = requestItem.NanbyoCds;
        result.IsDoubt = requestItem.IsDoubt;
        result.SearchWord = requestItem.SearchWord.AsString();
        result.SearchWordMode = requestItem.SearchWordMode;
        result.ByomeiCds = requestItem.ByomeiCds;
        result.ByomeiCdOpt = requestItem.ByomeiCdOpt;
        result.FreeByomeis = requestItem.FreeByomeis;
        result.SindateFrom = requestItem.SindateFrom;
        result.SindateTo = requestItem.SindateTo;
        result.LastVisitDateFrom = requestItem.LastVisitDateFrom;
        result.LastVisitDateTo = requestItem.LastVisitDateTo;
        result.Statuses = requestItem.Statuses;
        result.UketukeSbtId = requestItem.UketukeSbtId;
        result.KaMstId = requestItem.KaMstId;
        result.UserMstId = requestItem.UserMstId;
        result.IsSinkan = requestItem.IsSinkan;
        result.RaiinAgeFrom = requestItem.RaiinAgeFrom;
        result.RaiinAgeTo = requestItem.RaiinAgeTo;
        result.JikanKbns = requestItem.JikanKbns;
        result.DataKind = requestItem.DataKind;
        result.ItemCds = requestItem.ItemCds;
        result.ItemCdOpt = requestItem.ItemCdOpt;
        result.ItemCmts = requestItem.ItemCmts;
        result.MedicalSearchWord = requestItem.MedicalSearchWord;
        result.WordOpt = requestItem.WordOpt;
        result.KarteKbns = requestItem.KarteKbns;
        result.KarteSearchWords = requestItem.KarteSearchWords;
        result.KarteWordOpt = requestItem.KarteWordOpt;
        result.StartIraiDate = requestItem.StartIraiDate;
        result.EndIraiDate = requestItem.EndIraiDate;
        result.KensaItemCds = requestItem.KensaItemCds;
        result.KensaItemCdOpt = requestItem.KensaItemCdOpt;
        result.ListPtNums = requestItem.ListPtNums;
        return result;
    }
    #endregion
}
