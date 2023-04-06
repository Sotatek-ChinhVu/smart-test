using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using Helper.Enum;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reporting.Interface;
using System.Text;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfCreatorController : ControllerBase
    {
        private static HttpClient _httpClient = new HttpClient();
        private readonly IReportService _reportService;
        private readonly IConfiguration _configuration;
        private readonly IDrugInfoCoReportService _drugInfoCoReportService;
        public PdfCreatorController(IReportService reportService, IConfiguration configuration, IDrugInfoCoReportService drugInfoCoReportService)
        {
            _reportService = reportService;
            _configuration = configuration;
            _drugInfoCoReportService = drugInfoCoReportService;
        }

        [HttpGet(ApiPath.ExportKarte1)]
        public async Task<IActionResult> GenerateKarte1Report([FromQuery] Karte1ExportRequest request)
        {
            var karte1Data = _reportService.GetKarte1ReportingData(request.HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei, request.SyuByomei);
            return await RenderPdf(karte1Data, ReportType.Karte1);
        }

        [HttpGet(ApiPath.ExportDrugInfo)]
        public async Task<IActionResult> GenerateDrugInfReport([FromQuery] DrugInfoExportRequest request)
        {
            var drugInfo = _drugInfoCoReportService.SetOrderInfo(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
            return await RenderPdf(drugInfo, ReportType.DrugInfo);
        }

        private async Task<IActionResult> RenderPdf(object data, ReportType reportType)
        {
            StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            string basePath = _configuration.GetSection("RenderPdf")["BasePath"]!;

            string functionName = reportType switch
            {
                ReportType.Karte1 => "reporting-fm-karte1",
                ReportType.DrugInfo => "reporting-fm-drugInfo",

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
}
