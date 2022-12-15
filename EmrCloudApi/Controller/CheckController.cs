using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EmrCloudApi.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        var link = @"https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/templateCheck.docx";
        using (var client = new WebClient())
        {
            var content = client.DownloadData(link);
            using (var stream = new MemoryStream(content))
            {
                using (var word = WordprocessingDocument.Open(stream, true))
                {
                    var element = word.MainDocumentPart.Document.Body.Descendants<SdtElement>()
                                                .FirstOrDefault(sdt => sdt.SdtProperties.GetFirstChild<Tag>()?.Val == "<<氏名>>");

                    element.Descendants<Text>().First().Text = "hello_vuquynh_anh患者番号";
                    element.Descendants<Text>().Skip(1).ToList().ForEach(t => t.Remove());

                    element = word.MainDocumentPart.Document.Body.Descendants<SdtElement>()
                                                .FirstOrDefault(sdt => sdt.SdtProperties.GetFirstChild<Tag>()?.Val == "<<保険/特記事項１>>");

                    element.Descendants<Text>().First().Text = "___check+_hello_vuquynh_anh患者番号";
                    element.Descendants<Text>().Skip(1).ToList().ForEach(t => t.Remove());
                }
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "file_01.docx");
            }
        }
    }
}