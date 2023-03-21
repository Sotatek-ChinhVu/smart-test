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

        [HttpPost("ExportKarte2")]
        public async Task<IActionResult> GenerateKarte2Report([FromForm] double marginTop,
                                                              [FromForm] double marginBottom,
                                                              [FromForm] double marginLeft,
                                                              [FromForm] double marginRight,
                                                              [FromForm] IFormFile files,
                                                              [FromForm] double paperWidth,
                                                              [FromForm] double paperHeight,
                                                              [FromForm] string waitForExpression)
        {
            var streamImage = new MemoryStream();
            files.CopyTo(streamImage);

            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                files.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new StringContent(marginTop.ToString()), "marginTop");
            form.Add(new StringContent(marginBottom.ToString()), "marginBottom");
            form.Add(new StringContent(marginLeft.ToString()), "marginLeft");
            form.Add(new StringContent(marginRight.ToString()), "marginRight");
            form.Add(new StringContent(paperWidth.ToString()), "paperWidth");
            form.Add(new StringContent(paperHeight.ToString()), "paperHeight");
            form.Add(new StringContent(waitForExpression), "waitForExpression");
            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "files", files.FileName);

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
