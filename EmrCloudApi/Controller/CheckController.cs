using Aspose.Words;
using EmrCloudApi.Constants;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class CheckController : ControllerBase
{
    [HttpGet(ApiPath.ReplaceParamTemplate)]
    public IActionResult ReplaceParamTemplate()
    {
        using (WordDocument document = new WordDocument())
        {
            //Opens the input Word document.
            Stream docStream = File.OpenRead(Path.GetFullPath(@"../../../Template.docx"));
            document.Open(docStream, FormatType.Docx);
            docStream.Dispose();
            //Finds all occurrences of a misspelled word and replaces with properly spelled word.
            document.Replace("Cyles", "Cycles", true, true);
            //Saves the resultant file in the given path.
            docStream = File.Create(Path.GetFullPath(@"Result.docx"));
            document.Save(docStream, FormatType.Docx);
            docStream.Dispose();
        }
        //string path = "https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/file_01.docx";
        //    Document document = new Document(path);
        //    document.Range.Replace("<<name>>", "vu quynh anh");
        //    var stream = new MemoryStream();
        //    document.Save(stream, SaveFormat.Docx);
        //    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

    }
}