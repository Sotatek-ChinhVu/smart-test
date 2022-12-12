using UseCase.Document.ReplaceParamTemplate;

namespace Interactor.Document;

public class ReplaceParamTemplateInteractor : IReplaceParamTemplateInputPort
{
    public ReplaceParamTemplateOutputData Handle(ReplaceParamTemplateInputData inputData)
    {
        try
        {
            //string text = File.ReadAllText(@"https://develop-smartkarte-images-bucket.s3.ap-southeast-1.amazonaws.com/ClinicID/reference/files/11/file_01.docx");
            //text = text.Replace("some text", "new value");
            //File.WriteAllText("test.txt", text);
            return new ReplaceParamTemplateOutputData(ReplaceParamTemplateStatus.Successed);
        }
        catch (Exception)
        {
            return new ReplaceParamTemplateOutputData(ReplaceParamTemplateStatus.Failed);
        }
    }
}
