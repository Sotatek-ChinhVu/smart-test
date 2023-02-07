using DinkToPdf;
using DinkToPdf.Contracts;
using EmrCloudApi.Requests.ExportPDF;
using KarteReport.Interface;
using KarteReport.Utility;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Controller
{
    [Route("api/ExportKarte1")]
    [ApiController]
    public class PdfCreatorController : ControllerBase
    {
        private IConverter _converter;
        private IReportServices _reportService;
        public PdfCreatorController(IConverter converter, IReportServices reportService)
        {
            _converter = converter;
            _reportService = reportService;
        }
        [HttpGet]
        public IActionResult ReturnStream([FromQuery] Karte1ExportRequest request)
        {
            var templateGenerator = new TemplateGenerator(_reportService);
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 0, Bottom = 0, Left = 0, Right = 0 },
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateGenerator.GetHTMLString(1, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei, request.SyuByomei),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                //HeaderSettings = { FontName = "DejaVu Sans", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                //FooterSettings = { FontName = "DejaVu Sans", FontSize = 9, Line = true, Center = "Report Footer" }

            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            MemoryStream ms = new MemoryStream(_converter.Convert(pdf));
            return File(ms, "application/pdf");
        }
    }
}
