using ClosedXML.Excel;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.DrugInfor;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Requests.DrugInfor;
using EmrCloudApi.Requests.ExportPDF;
using EmrCloudApi.Requests.MedicalExamination;
using Helper.Enum;
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
using Reporting.OutDrug.Model.Output;
using Reporting.ReceiptList.Model;
using Reporting.ReportServices;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using UseCase.DrugInfor.GetDataPrintDrugInfo;
using UseCase.MedicalExamination.GetDataPrintKarte2;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class PdfCreatorController : ControllerBase
{
    private static HttpClient _httpClient = new HttpClient();
    private readonly IReportService _reportService;
    private readonly IConfiguration _configuration;
    private readonly IHistoryCommon _historyCommon;
    private readonly IGetCommonDrugInf _commonDrugInf;

    public PdfCreatorController(IReportService reportService, IConfiguration configuration, IHistoryCommon historyCommon, IGetCommonDrugInf commonDrugInf)
    {
        _reportService = reportService;
        _configuration = configuration;
        _historyCommon = historyCommon;
        _commonDrugInf = commonDrugInf;
    }

    [HttpGet(ApiPath.ExportKarte1)]
    public async Task<IActionResult> GenerateKarte1Report([FromQuery] Karte1ExportRequest request)
    {
        var karte1Data = _reportService.GetKarte1ReportingData(request.HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei, request.SyuByomei);
        return await RenderPdf(karte1Data, ReportType.Common, karte1Data.JobName);
    }

    [HttpGet(ApiPath.ExportNameLabel)]
    public async Task<IActionResult> GenerateNameLabelReport([FromQuery] NameLabelExportRequest request)
    {
        var data = _reportService.GetNameLabelReportingData(request.PtId, request.KanjiName, request.SinDate);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ExportDrugInfo)]
    public async Task<IActionResult> GenerateDrugInfReport([FromQuery] DrugInfoExportRequest request)
    {
        var drugInfo = _reportService.SetOrderInfo(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
        return await RenderPdf(drugInfo, ReportType.DrugInfo, "薬情.pdf");
    }

    [HttpGet(ApiPath.ExportByomei)]
    public async Task<IActionResult> GenerateByomeiReport([FromQuery] ByomeiExportRequest request)
    {
        var byomeiData = _reportService.GetByomeiReportingData(request.HpId, request.PtId, request.FromDay, request.ToDay, request.TenkiIn, request.HokenIdList);
        return await RenderPdf(byomeiData, ReportType.Common, byomeiData.JobName);
    }

    [HttpGet(ApiPath.ExportOrderLabel)]
    public async Task<IActionResult> GenerateOrderLabelReport([FromQuery] OrderLabelExportRequest request)
    {
        List<(int from, int to)> odrKouiKbns = new();
        foreach (var item in request.OdrKouiKbns)
        {
            odrKouiKbns.Add(new(item.From, item.To));
        }
        var data = _reportService.GetOrderLabelReportingData(0, request.HpId, request.PtId, request.SinDate, request.RaiinNo, odrKouiKbns, new());
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ExportSijisen)]
    public async Task<IActionResult> GenerateSijisenReport([FromQuery] SijisenExportRequest request)
    {
        var odrKouiKbns = new List<(int from, int to)>();
        foreach (var item in request.OdrKouiKbns)
        {
            odrKouiKbns.Add(new(item.From, item.To));
        }
        var sijisenData = _reportService.GetSijisenReportingData(request.HpId, request.FormType, request.PtId, request.SinDate, request.RaiinNo, odrKouiKbns, request.PrintNoOdr);
        return await RenderPdf(sijisenData, ReportType.Common, sijisenData.JobName);
    }

    [HttpGet(ApiPath.MedicalRecordWebId)]
    public async Task<IActionResult> GenerateMedicalRecordWebIdReport([FromQuery] MedicalRecordWebIdRequest request)
    {
        var data = _reportService.GetMedicalRecordWebIdReportingData(request.HpId, request.PtId, request.SinDate);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.OutDrug)]
    public async Task<IActionResult> GenerateOutDrugWebIdReport([FromQuery] OutDrugRequest request)
    {
        var data = _reportService.GetOutDrugReportingData(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
        return await RenderPdf(data, ReportType.OutDug, "院外処方箋.pdf");
    }

    [HttpGet(ApiPath.ReceiptCheck)]
    public async Task<IActionResult> GetReceiptCheckReport([FromQuery] ReceiptCheckRequest request)
    {
        var data = _reportService.GetReceiptCheckCoReportService(request.HpId, request.PtIds, request.SeikyuYm);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpPost(ApiPath.ReceiptList)]
    public async Task<IActionResult> GetReceiptListReport([FromForm] AccountingReportRequest requestStringJson)
    {
        var stringJson = requestStringJson.JsonAccounting;
        var request = JsonSerializer.Deserialize<GetReceiptListRequest>(stringJson) ?? new();

        var receInputList = request.ReceiptListModels.Select(item => new ReceiptInputModel(
                                                                         item.SinYm,
                                                                         item.PtId,
                                                                         item.HokenId))
                                                     .ToList();

        var data = _reportService.GetReceiptListReportingData(request.HpId, request.SeikyuYm, receInputList);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.PeriodReceiptReport)]
    public async Task<IActionResult> GenerateAccountingReport([FromQuery] PeriodReceiptRequest request)
    {
        List<CoAccountingParamModel> requestConvert = request.PtInfList.Select(item => new CoAccountingParamModel(
                                                                                           item.PtId, request.StartDate, request.EndDate, item.RaiinNos, item.HokenId,
                                                                                           request.MiseisanKbn, request.SaiKbn, request.MisyuKbn, request.SeikyuKbn, item.HokenKbn,
                                                                                           request.HokenSeikyu, request.JihiSeikyu, request.NyukinBase,
                                                                                           request.HakkoDay, request.Memo,
                                                                                           request.PrintType, request.FormFileName))
                                                                       .ToList();
        var data = _reportService.GetAccountingReportingData(request.HpId, requestConvert);
        return await RenderPdf(data, ReportType.Accounting, data.JobName);
    }

    [HttpPost(ApiPath.AccountingReport)]
    public async Task<IActionResult> GenerateAccountingReport([FromForm] AccountingReportRequest requestStringJson)
    {
        var stringJson = requestStringJson.JsonAccounting;
        var request = JsonSerializer.Deserialize<AccountingCoReportModelRequest>(stringJson) ?? new();
        var multiAccountDueListModels = request.MultiAccountDueListModels.Select(item => ConvertToCoAccountDueListModel(item)).ToList();

        //public async Task<IActionResult> GenerateAccountingReport([FromBody] AccountingCoReportModelRequest request)
        //{
        //    var multiAccountDueListModels = request.MultiAccountDueListModels.Select(item => ConvertToCoAccountDueListModel(item)).ToList();

        var data = _reportService.GetAccountingData(request.HpId, request.Mode, request.PtId, multiAccountDueListModels, request.IsPrintMonth, request.Ryoshusho, request.Meisai);
        return await RenderPdf(data, ReportType.Accounting, data.JobName);
    }

    [HttpGet(ApiPath.ReceiptReport)]
    public async Task<IActionResult> GenerateReceiptReport([FromQuery] ReceiptExportRequest request)
    {
        var data = _reportService.GetAccountingReportingData(request.HpId, request.PtId, request.PrintType, request.RaiinNoList, request.RaiinNoPayList, request.IsCalculateProcess);
        return await RenderPdf(data, ReportType.Accounting, data.JobName);
    }

    [HttpGet(ApiPath.GrowthCurve)]
    public async Task<IActionResult> GetGrowthCurvePrintData([FromQuery] GrowthCurvePrintDataRequest request)
    {
        CommonReportingRequestModel data;
        if (request.Type == 0)
        {
            data = _reportService.GetGrowthCurveA5PrintData(request.HpId, new GrowthCurveConfig(request.PtNum, request.PtId, request.PtName, request.Sex, request.BirthDay, request.PrintMode, request.PrintDate, request.WeightVisible, request.HeightVisible, request.Per50, request.Per25, request.Per10, request.Per3, request.SDAvg, request.SD1, request.SD2, request.SD25, request.Legend, request.Scope));
        }
        else
        {
            data = _reportService.GetGrowthCurveA4PrintData(request.HpId, new GrowthCurveConfig(request.PtNum, request.PtId, request.PtName, request.Sex, request.BirthDay, request.PrintMode, request.PrintDate, request.WeightVisible, request.HeightVisible, request.Per50, request.Per25, request.Per10, request.Per3, request.SDAvg, request.SD1, request.SD2, request.SD25, request.Legend, request.Scope));
        }
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.StaticReport)]
    public async Task<IActionResult> GenerateStatisticReport([FromQuery] StatisticExportRequest request)
    {
        var data = _reportService.GetStatisticReportingData(request.HpId, request.FormName, request.MenuId, request.MonthFrom, request.MonthTo, request.DateFrom, request.DateTo, request.TimeFrom, request.TimeTo, request.CoFileType, request.IsPutTotalRow, request.TenkiDateFrom, request.TenkiDateTo, request.EnableRangeFrom, request.EnableRangeTo, request.PtNumFrom, request.PtNumTo);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.PatientManagement)]
    public async Task<IActionResult> GeneratePatientManagement([FromQuery] PatientManagementRequest request)
    {
        var data = _reportService.GetPatientManagement(request.HpId, request.MenuId);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ReceiptPreview)]
    public async Task<IActionResult> ReceiptPreview([FromQuery] ReceiptPreviewRequest request)
    {
        var data = _reportService.GetReceiptData(request.HpId, request.PtId, request.SinYm, request.HokenId);
        var result = await RenderPdf(data, ReportType.Common, data.JobName);
        return result;
    }

    [HttpGet(ApiPath.SyojyoSyoki)]
    public async Task<IActionResult> SyojyoSyoki([FromQuery] SyojyoSyokiRequest request)
    {
        var data = _reportService.GetSyojyoSyokiReportingData(request.HpId, request.PtId, request.SeiKyuYm, request.HokenId);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.Kensalrai)]
    public async Task<IActionResult> Kensalrai([FromQuery] KensalraiRequest request)
    {
        var data = _reportService.GetKensalraiData(request.HpId, request.SystemDate, request.FromDate, request.ToDate, request.CenterCd);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ReceiptPrint)]
    public async Task<IActionResult> ReceiptPrint([FromQuery] ReceiptPrintRequest request)
    {
        var data = _reportService.GetReceiptPrint(request.HpId, request.FormName, request.PrefNo, request.ReportId, request.ReportEdaNo, request.DataKbn, request.PtId, request.SeikyuYm, request.SinYm, request.HokenId, request.DiskKind, request.DiskCnt, request.WelfareType, request.PrintHokensyaNos);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.WelfareDisk)]
    public IActionResult GenerateKarte1Report([FromQuery] ReceiptPrintExcelRequest request)
    {
        var data = _reportService.GetReceiptPrintExcel(request.HpId, request.PrefNo, request.ReportId, request.ReportEdaNo, request.DataKbn, request.SeikyuYm);
        return RenderExcel(data);
    }

    [HttpPost(ApiPath.ReceListCsv)]
    public IActionResult GenerateKarteCsvReport([FromBody] ReceiptListExcelRequest request)
    {
        var data = _reportService.GetReceiptListExcel(request.receiptListModel);
        return RenderExcel(data);
    }


    [HttpPost(ApiPath.MemoMsgPrint)]
    public async Task<IActionResult> MemoMsgPrint([FromForm] StringObjectRequest requestString)
    {
        var request = JsonSerializer.Deserialize<MemoMsgPrintRequest>(requestString.StringJson) ?? new();
        var data = _reportService.GetMemoMsgReportingData(request.ReportName, request.Title, request.ListMessage);
        return await RenderPdf(data, ReportType.Common, "MemoMsgPrint");
    }

    [HttpGet(ApiPath.ReceTarget)]
    public async Task<IActionResult> ReceTarget([FromQuery] ReceTargetRequest request)
    {
        var data = _reportService.GetReceTargetPrint(request.HpId, request.SeikyuYm);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.DrugNoteSeal)]
    public async Task<IActionResult> GetDrugNoteSealPrintData([FromQuery] DrugNoteSealPrintDataRequest request)
    {
        var data = _reportService.GetDrugNoteSealPrintData(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.InDrug)]
    public async Task<IActionResult> GetInDrugPrintData([FromQuery] InDrugPrintDataRequest request)
    {
        var data = _reportService.GetInDrugPrintData(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.Yakutai)]
    public async Task<IActionResult> Yakutai([FromQuery] YakutaiRequest request)
    {
        var data = _reportService.GetYakutaiReportingData(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.KensaLabel)]
    public async Task<IActionResult> KensaLabel([FromQuery] KensaLabelRequest request)
    {
        var data = _reportService.GetKensaLabelPrintData(request.HpId, request.PtId, request.RaiinNo, request.SinDate, new KensaPrinterModel(request.ItemCd, request.ContainerName, request.ContainerCd, request.Count, request.PrinterName, request.InoutKbn, request.OdrKouiKbn));
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.AccountingCard)]
    public async Task<IActionResult> GetAccountingCardPrintData([FromQuery] AccountingCardReportingRequest request)
    {
        var data = _reportService.GetAccountingCardReportingData(request.HpId, request.PtId, request.SinYm, request.HokenId, request.IncludeOutDrug);

        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ExportKarte3)]
    public async Task<IActionResult> GetKarte3ReportingData([FromQuery] Karte3ReportingRequest request)
    {
        var data = _reportService.GetKarte3ReportingData(request.HpId, request.PtId, request.StartSinYm, request.EndSinYm, request.IncludeHoken, request.IncludeJihi);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpPost(ApiPath.AccountingCardList)]
    public async Task<IActionResult> GetAccountingCardListReportingData([FromForm] StringObjectRequest requestString)
    {
        var request = JsonSerializer.Deserialize<AccountingCardListRequest>(requestString.StringJson) ?? new();
        var data = _reportService.GetAccountingCardListReportingData(request.HpId, request.Targets, request.IncludeOutDrug, request.KaName, request.TantoName, request.UketukeSbt, request.Hoken);
        return await RenderPdf(data, ReportType.Common, data.JobName);
    }

    [HttpGet(ApiPath.ExportKarte2)]
    public async Task<IActionResult> GenerateKarte2Report([FromQuery] GetDataPrintKarte2Request request)
    {
        var inputData = new GetDataPrintKarte2InputData(request.PtId, request.HpId, request.SinDate, request.StartDate, request.EndDate, request.IsCheckedHoken, request.IsCheckedJihi, request.IsCheckedHokenJihi, request.IsCheckedJihiRece, request.IsCheckedHokenRousai, request.IsCheckedHokenJibai, request.IsCheckedDoctor, request.IsCheckedStartTime, request.IsCheckedVisitingTime, request.IsCheckedEndTime, request.IsUketsukeNameChecked, request.IsCheckedSyosai, request.IsIncludeTempSave, request.IsCheckedApproved, request.IsCheckedInputDate, request.IsCheckedSetName, request.DeletedOdrVisibilitySetting, request.IsIppanNameChecked, request.IsCheckedHideOrder, request.EmptyMode);

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
                    PdfReader pdfReader = new PdfReader(streamingData);
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
        var inputData = new GetDataPrintDrugInfoInputData(request.HpId, request.SinDate, request.ItemCd, request.Level, string.Empty, request.YJCode, request.Type);

        var drugInfo = _commonDrugInf.GetDrugInforModel(inputData.HpId, inputData.SinDate, inputData.ItemCd);
        string htmlData = string.Empty;
        switch (inputData.Type)
        {
            case TypeHTMLEnum.ShowProductInf:
                htmlData = _commonDrugInf.ShowProductInf(inputData.HpId, inputData.SinDate, inputData.ItemCd, inputData.Level, inputData.DrugName, inputData.YJCode);
                break;
            case TypeHTMLEnum.ShowKanjaMuke:
                htmlData = _commonDrugInf.ShowKanjaMuke(inputData.ItemCd, inputData.Level, inputData.DrugName, inputData.YJCode);
                break;
            case TypeHTMLEnum.ShowMdbByomei:
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
                    PdfReader pdfReader = new PdfReader(streamingData);
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
                && data.ReportType != (int)CoReportType.MemoMsg
                && data.ReportType != (int)CoReportType.Receipt))
        {
            returnNoData = true;
        }
        return await ActionReturnPDF(returnNoData, data, reportType, fileName);
    }

    private async Task<IActionResult> RenderPdf(DrugInfoData data, ReportType reportType, string fileName)
    {
        bool returnNoData = !data.drugInfoList.Any();
        return await ActionReturnPDF(returnNoData, data, reportType, fileName);
    }

    private async Task<IActionResult> RenderPdf(CoOutDrugReportingOutputData data, ReportType reportType, string fileName)
    {
        bool returnNoData = !data.Data.Any();
        return await ActionReturnPDF(returnNoData, data, reportType, fileName);
    }

    private async Task<IActionResult> RenderPdf(AccountingResponse data, ReportType reportType, string fileName)
    {
        bool returnNoData = !data.AccountingReportingRequestItems.Any();
        return await ActionReturnPDF(returnNoData, data, reportType, fileName);
    }

    private async Task<IActionResult> ActionReturnPDF(bool returnNoData, object data, ReportType reportType, string fileName)
    {
        var json = JsonSerializer.Serialize(data);
        Console.WriteLine("DataJsonTestPdfString: " + json);
        if (returnNoData)
        {
            return Content(@"
            <meta charset=""utf-8"">
            <title>印刷対象が見つかりません。</title>
            <p style='text-align: center;font-size: 25px;font-weight: 300'>印刷対象が見つかりません。</p>
            ", "text/html");
        }

        StringContent jsonContent = (reportType == ReportType.DrugInfo)
          ? new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json") :
          new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

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
                FileName = fileName,
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

    private IActionResult RenderExcel(CommonExcelReportingModel dataModel)
    {
        var dataList = dataModel.Data;
        if (!dataList.Any())
        {
            return Content(@"
            <meta charset=""utf-8"">
            <title>印刷対象が見つかりません。</title>
            <p style='text-align: center;font-size: 25px;font-weight: 300'>印刷対象が見つかりません。</p>
            ", "text/html");
        }
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        using (var workbook = new XLWorkbook())
        {
            IXLWorksheet worksheet =
            workbook.Worksheets.Add(dataModel.SheetName);
            int rowIndex = 1;
            foreach (var row in dataList)
            {
                List<string> colDataList = row.Split(',').ToList();
                int colIndex = 1;
                foreach (var cellData in colDataList)
                {
                    worksheet.Cell(rowIndex, colIndex).Value = cellData;
                    colIndex++;
                }
                rowIndex++;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, contentType, dataModel.FileName);
            }
        }
    }
}
