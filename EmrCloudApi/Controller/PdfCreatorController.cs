using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using Helper.Enum;
using Microsoft.AspNetCore.Mvc;
using Reporting.DrugInfo.Service;
using Reporting.ReportServices;
using System.Text;
using System.Text.Json;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class PdfCreatorController : ControllerBase
{
    private static HttpClient _httpClient = new HttpClient();
    private readonly IReportService _reportService;
    private readonly IConfiguration _configuration;

    public PdfCreatorController(IReportService reportService, IConfiguration configuration)
    {
        _reportService = reportService;
        _configuration = configuration;
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

    [HttpPost(ApiPath.ExportByomei)]
    public async Task<IActionResult> GenerateByomeiReport([FromBody] ByomeiExportRequest request)
    {
        var byomeiData = _reportService.GetByomeiReportingData(request.PtId, request.FromDay, request.ToDay, request.TenkiIn, request.HokenIdList);
        return await RenderPdf(byomeiData, ReportType.Common);
    }

    [HttpPost(ApiPath.ExportOrderLabel)]
    public async Task<IActionResult> GenerateOrderLabelReport([FromBody] OrderLabelExportRequest request)
    {
        List<(int from, int to)> odrKouiKbns = new();
        foreach (var item in request.OdrKouiKbns)
        {
            odrKouiKbns.Add(new(item.From, item.To));
        }
        var data = _reportService.GetOrderLabelReportingData(0, request.HpId, request.PtId, request.SinDate, request.RaiinNo, odrKouiKbns, new());
        return await RenderPdf(data, ReportType.Common);
    }

    [HttpPost(ApiPath.ExportSijisen)]
    public async Task<IActionResult> GenerateSijisenReport([FromBody] SijisenExportRequest request)
    {
        var odrKouiKbns = new List<(int from, int to)>();
        foreach (var item in request.OdrKouiKbns)
        {
            odrKouiKbns.Add(new(item.From, item.To));
        }
        var sijisenData = _reportService.GetSijisenReportingData(request.FormType, request.PtId, request.SinDate, request.RaiinNo, odrKouiKbns, request.PrintNoOdr);
        return await RenderPdf(sijisenData, ReportType.Common);
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
