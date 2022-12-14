using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using System.Net;
using UseCase.Document.ReplaceParamTemplate;

namespace Interactor.Document;

public class ReplaceParamTemplateInteractor : IReplaceParamTemplateInputPort
{
    public ReplaceParamTemplateOutputData Handle(ReplaceParamTemplateInputData inputData)
    {
        try
        {
            var path = @"C:\Users\DELL\OneDrive\Desktop\checkFile.docx";
            var link = @"https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/file_01.docx";

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(link, path);
            }

            using (var word = WordprocessingDocument.Open(path, true))
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

            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
                using (var memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    fileStream.Flush();
                    fileStream.Close();
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
            }
            return new ReplaceParamTemplateOutputData(ReplaceParamTemplateStatus.Successed);
        }
        catch (Exception)
        {
            return new ReplaceParamTemplateOutputData(ReplaceParamTemplateStatus.Failed);
        }
    }
}
