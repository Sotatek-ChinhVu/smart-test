using DocumentFormat.OpenXml.Wordprocessing;
using EmrCloudApi.Requests.ExportPDF;
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
        public PdfCreatorController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("ExportKarte1")]
        public async Task<IActionResult> GenerateKarte1Report([FromQuery] Karte1ExportRequest request)
        {
            var karte1Data = _reportService.GetKarte1ReportingData(1, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei, request.SyuByomei);

            return await RenderPdf(karte1Data);
        }

        private async Task<IActionResult> RenderPdf(object data)
        {
            StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");

            using (HttpResponseMessage response = await _httpClient.PostAsync("https://smartkarte-report.sotatek.works/api/reporting-fmKarte1", jsonContent))
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
