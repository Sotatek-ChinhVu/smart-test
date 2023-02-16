using DocumentFormat.OpenXml.Wordprocessing;
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
        public PdfCreatorController(IReportService reportService, IConfiguration configuration)
        {
            _reportService = reportService;
            _configuration = configuration;
        }

        [HttpGet("ExportKarte1")]
        public async Task<IActionResult> GenerateKarte1Report([FromQuery] Karte1ExportRequest request)
        {
            var karte1Data = _reportService.GetKarte1ReportingData(1, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei, request.SyuByomei);

            return await RenderPdf(karte1Data, ReportType.Karte1);
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
                    functionName = "reporting-fmKarte1";
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
