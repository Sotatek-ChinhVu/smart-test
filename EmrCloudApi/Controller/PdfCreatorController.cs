using DinkToPdf;
using DinkToPdf.Contracts;
using KarteReport.Utility;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Controller
{
    [Route("api/ExportKarte1")]
    [ApiController]
    public class PdfCreatorController : ControllerBase
    {
        private IConverter _converter;
        public PdfCreatorController(IConverter converter)
        {
            _converter = converter;
        }
        [HttpGet]
        public IActionResult ReturnStream()
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Yu Gothic", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Yu Gothic", FontSize = 9, Line = true, Center = "Report Footer" }
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
