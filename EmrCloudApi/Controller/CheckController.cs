using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EmrCloudApi.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class CheckController : ControllerBase
{
    //[HttpGet(ApiPath.ReplaceParamTemplate)]
    //public IActionResult ReplaceParamTemplate()
    //{
    //    var stream = new MemoryStream();
    //    var path = @"https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/file_01.docx";

    //    SearchAndReplace(@"C:\Users\DELL\OneDrive\Desktop\Demofile.docx");
    //    //Document document = new Document(path);
    //    //document.Range.Replace("<<name>>", "vu quynh anh");
    //    //document.Save(stream, SaveFormat.Docx);
    //    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
    //}

    [HttpGet(ApiPath.ReplaceParamTemplate)]
    public IActionResult ExportEmployee()
    {
        var link = @"https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/file_01.docx";
        using (var client = new WebClient())
        {
            var content = client.DownloadData(link);
            using (var stream = new MemoryStream(content))
            {
                using (var word = WordprocessingDocument.Open(stream, true))
                {
                    if (word.MainDocumentPart != null && word.MainDocumentPart.RootElement != null)
                    {
                        var name = word.MainDocumentPart.RootElement.Descendants<Text>().FirstOrDefault(c => c.Text.Contains("<<name>>"));
                        if (name != null)
                        {
                            name.Text = "vuquynhanh";
                        }
                    }
                }
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "file_01.docx");
            }
        }
    }
}