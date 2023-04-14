﻿using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using EmrCloudApi.Requests.MedicalExamination;
using Helper.Enum;
using Interactor.MedicalExamination.HistoryCommon;
using Microsoft.AspNetCore.Mvc;
using Reporting.OutDrug.Service;
using Reporting.ReportServices;
using System.Text;
using System.Text.Json;
using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class PdfCreatorController : ControllerBase
{
    private static HttpClient _httpClient = new HttpClient();
    private readonly IReportService _reportService;
    private readonly IConfiguration _configuration;
    private readonly IOutDrugCoReportService _outDrugCoReportService;
    private readonly IHistoryCommon _historyCommon;

    public PdfCreatorController(IReportService reportService, IConfiguration configuration, IOutDrugCoReportService outDrugCoReportService, IHistoryCommon historyCommon)
    {
        _reportService = reportService;
        _configuration = configuration;
        _outDrugCoReportService = outDrugCoReportService;
        _historyCommon = historyCommon;
    }

    [HttpGet(ApiPath.ExportKarte1)]
    public async Task<IActionResult> GenerateKarte1Report([FromQuery] Karte1ExportRequest request)
    {
        var karte1Data = _reportService.GetKarte1ReportingData(request.HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei, request.SyuByomei);
        return await RenderPdf(karte1Data, ReportType.Karte1);
    }

    [HttpGet(ApiPath.ExportNameLabel)]
    public async Task<IActionResult> GenerateNameLabelReport([FromQuery] NameLabelExportRequest request)
    {
        var data = _reportService.GetNameLabelReportingData(request.PtId, request.KanjiName, request.SinDate);
        return await RenderPdf(data, ReportType.Common);
    }

    [HttpGet(ApiPath.ExportDrugInfo)]
    public async Task<IActionResult> GenerateDrugInfReport([FromQuery] DrugInfoExportRequest request)
    {
        var drugInfo = _reportService.SetOrderInfo(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
        return await RenderPdf(drugInfo, ReportType.DrugInfo);
    }

    [HttpGet(ApiPath.ExportByomei)]
    public async Task<IActionResult> GenerateByomeiReport([FromQuery] ByomeiExportRequest request)
    {
        var byomeiData = _reportService.GetByomeiReportingData(request.PtId, request.FromDay, request.ToDay, request.TenkiIn, request.HokenIdList);
        return await RenderPdf(byomeiData, ReportType.Common);
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
        return await RenderPdf(data, ReportType.Common);
    }

    [HttpGet(ApiPath.ExportSijisen)]
    public async Task<IActionResult> GenerateSijisenReport([FromQuery] SijisenExportRequest request)
    {
        var odrKouiKbns = new List<(int from, int to)>();
        foreach (var item in request.OdrKouiKbns)
        {
            odrKouiKbns.Add(new(item.From, item.To));
        }
        var sijisenData = _reportService.GetSijisenReportingData(request.FormType, request.PtId, request.SinDate, request.RaiinNo, odrKouiKbns, request.PrintNoOdr);
        return await RenderPdf(sijisenData, ReportType.Common);
    }

    [HttpGet(ApiPath.MedicalRecordWebId)]
    public async Task<IActionResult> GenerateMedicalRecordWebIdReport([FromQuery] MedicalRecordWebIdRequest request)
    {
        var data = _reportService.GetMedicalRecordWebIdReportingData(request.HpId, request.PtId, request.SinDate);
        return await RenderPdf(data, ReportType.Common);
    }

    [HttpGet(ApiPath.OutDrug)]
    public async Task<IActionResult> GenerateOutDrugWebIdReport([FromQuery] OutDrugRequest request)
    {
        var data = _outDrugCoReportService.GetOutDrugReportingData(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
        return await RenderPdf(data, ReportType.OutDug);
    }

    [HttpGet("ExportKarte2")]
    public async Task<IActionResult> GenerateKarte2Report([FromQuery] GetDataPrintKarte2Request request)
    {
        var inputData = new GetDataPrintKarte2InputData(request.PtId, request.HpId, request.SinDate, request.StartDate, request.EndDate, request.IsCheckedHoken, request.IsCheckedJihi, request.IsCheckedHokenJihi, request.IsCheckedJihiRece, request.IsCheckedHokenRousai, request.IsCheckedHokenJibai, request.IsCheckedDoctor, request.IsCheckedStartTime, request.IsCheckedVisitingTime, request.IsCheckedEndTime, request.IsUketsukeNameChecked, request.IsCheckedSyosai, request.IsIncludeTempSave, request.IsCheckedApproved, request.IsCheckedInputDate, request.IsCheckedSetName, request.DeletedOdrVisibilitySetting, request.IsIppanNameChecked, request.IsCheckedHideOrder);

        var karte2Result = _historyCommon.GetDataKarte2(inputData);
        var stringKarte2Result = JsonSerializer.Serialize(karte2Result);

        byte[] bytes = System.IO.File.ReadAllBytes(@"..\EmrCloudApi\Source\index.html");
        using (var memoryStream = new MemoryStream())
        {
            //files.CopyTo(memoryStream);
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

                return File(byteData, "application/pdf");
            }
        }
    }

    private async Task<IActionResult> RenderPdf(object data, ReportType reportType)
    {
        StringContent jsonContent = (reportType ==
          ReportType.Karte1
          || reportType == ReportType.DrugInfo)
          ? new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json") :
          new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

        string basePath = _configuration.GetSection("RenderPdf")["BasePath"]!;

        string functionName = reportType switch
        {
            ReportType.Karte1 => "reporting-fm-karte1",
            ReportType.DrugInfo => "reporting-fm-drugInfo",
            ReportType.Common => "common-reporting",
            ReportType.OutDug => "reporting-out-drug",
            _ => throw new NotImplementedException($"The reportType is incorrect: {reportType}")
        } ?? string.Empty;

        using (HttpResponseMessage response = await _httpClient.PostAsync($"{basePath}{functionName}", jsonContent))
        {
            response.EnsureSuccessStatusCode();

            using (var streamingData = (MemoryStream)response.Content.ReadAsStream())
            {
                var byteData = streamingData.ToArray();

                return File(byteData, "application/pdf");
            }
        }
    }
}
