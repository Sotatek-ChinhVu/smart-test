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
            return await RenderPdf(drugInfo.Item2, drugInfo.Item1);
        }

        private async Task<IActionResult> RenderPdf(object data, ReportType reportType)
        {
            StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");

            string basePath = _configuration.GetSection("RenderPdf")["BasePath"]!;
            string functionName = string.Empty;
            switch (reportType)
            {
                case ReportType.Karte1:
                    functionName = "reporting-fm-karte1";
                    break;
                case ReportType.DrgInfType2_1:
                    functionName = "frmDrgInfType2_1";
                    break;
                case ReportType.DrgInfType2_2:
                    functionName = "frmDrgInfType2_2";
                    break;
                case ReportType.DrgInfType2_3:
                    functionName = "frmDrgInfType2_3";
                    break;
                case ReportType.DrgInf1:
                    functionName = "frmDrgInf1";
                    break;
                case ReportType.DrgInf2:
                    functionName = "frmDrgInf2";
                    break;
                case ReportType.DrgInf3:
                    functionName = "frmDrgInf3";
                    break;
                default:
                    throw new NotImplementedException("The reportType is incorrect: " + reportType.ToString());
            }

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
