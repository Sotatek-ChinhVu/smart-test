using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EmrCloudApi.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class CheckController : ControllerBase
{
    [HttpGet(ApiPath.DowloadDocumentTemplate)]
    public IActionResult ReplaceParamTemplate()
    {
        var link = @"https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/templateCheck.xlsx";



        using (var client = new WebClient())
        {
            var content = client.DownloadData(link);

            using (var stream = new MemoryStream(content))
            {
                ProcessTemplate(stream, "hello <japan>>");
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Path.GetFileName(link));
            }
        }
    }

    private static void ProcessTemplate(Stream template, string replacementText)
    {
        using (var workbook = SpreadsheetDocument.Open(template, true, new OpenSettings { AutoSave = true }))
        {
            WorkbookPart workbookPart = workbook.WorkbookPart;
            WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
            SharedStringTablePart stringTablePart = workbookPart.SharedStringTablePart;
            SharedStringTable stringTable = stringTablePart.SharedStringTable;
            IEnumerable<SharedStringItem> values = stringTable.Elements<SharedStringItem>();
            foreach (var item in values)
            {
                if (item.InnerText.Contains("<<作成日(西暦K)>>"))
                {
                    item.Text = new Text("Мобильный телефон");
                }
                if (item.InnerText.Contains("<<name>>"))
                {
                    item.Text = new Text("Мобильный телефон");
                }
            }

            // Replace shared strings
            SharedStringTablePart sharedStringsPart = workbook.WorkbookPart.SharedStringTablePart;
            var sharedStringTextElements = sharedStringsPart.SharedStringTable.Descendants<Text>();
            DoReplace(sharedStringTextElements, replacementText);

            // Replace inline strings
            var worksheetParts = workbook.GetPartsOfType<WorksheetPart>();
            foreach (var worksheet in worksheetParts)
            {
                var allTextElements = worksheet.Worksheet.Descendants<Text>();
                DoReplace(allTextElements, replacementText);
            }

        } // AutoSave enabled
    }

    private static void DoReplace(IEnumerable<Text> textElements, string replacementText)
    {
        //for (int i = 0; i < textElements.Count(); i++)
        //{
        //    var text = textElements[i]
        //    if (.Text.con)
        //    {

        //    }
        //}
        foreach (var text in textElements)
        {
            if (text.Text.Contains("<<check>>"))
            {
                text.Text = text.Text.Replace("<<check>>", replacementText);
            }
            if (text.Text.Contains("<<作成日(西暦K)>>"))
            {
                text.Text = text.Text.Replace("<<", string.Empty);
                text.Text = text.Text.Replace("作成日(西暦K)", "Мобильный телефон");
                text.Text = text.Text.Replace(">>", string.Empty);
            }
        }
    }
}