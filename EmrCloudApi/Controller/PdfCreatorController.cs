using EmrCloudApi.Requests.ExportPDF;
using Helper.Enum;
using Microsoft.AspNetCore.Mvc;
using Reporting.Interface;
using System.Text;
using System.Text.Json;

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

        [HttpGet("ExportKarte1")]
        public async Task<IActionResult> GenerateKarte1Report([FromQuery] Karte1ExportRequest request)
        {
            var karte1Data = _reportService.GetKarte1ReportingData(request.HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei, request.SyuByomei);

            return await RenderPdf(karte1Data, ReportType.Karte1);
        }

        [HttpGet("ExportDrugInfo")]
        public async Task<IActionResult> GenerateDrugInfReport([FromQuery] DrugInfoExportRequest request)
        {
            var drugInfo = _drugInfoCoReportService.SetOrderInfo(request.HpId, request.PtId, request.SinDate, request.RaiinNo);
            var oMycustomclassname = Newtonsoft.Json.JsonConvert.SerializeObject(drugInfo.Item2);
            return await RenderPdf(drugInfo.Item2, drugInfo.Item1);
        }

        private async Task<IActionResult> RenderPdf(object data, ReportType reportType)
        {
            StringContent jsonContent = new StringContent(JsonSerializer.Serialize(data),Encoding.UTF8,"application/json");

            string basePath = _configuration.GetSection("RenderPdf")["BasePath"]!;

            string functionName = reportType switch
            {
                ReportType.Karte1 => "reporting-fm-karte1",
                ReportType.DrgInfType2_1 => "frmDrgInfType2_1",
                ReportType.DrgInfType2_2 => "frmDrgInfType2_2",
                ReportType.DrgInfType2_3 => "frmDrgInfType2_3",
                ReportType.DrgInf1 => "frmDrgInf1",
                ReportType.DrgInf2 => "reporting-fm-drugInfo",
                ReportType.DrgInf3 => "frmDrgInf3",

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
