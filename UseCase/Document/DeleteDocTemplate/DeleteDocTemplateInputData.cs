using UseCase.Core.Sync.Core;

namespace UseCase.Document.DeleteDocTemplate;

public class DeleteDocTemplateInputData : IInputData<DeleteDocTemplateOutputData>
{
    public DeleteDocTemplateInputData(int categoryCd, string templateFileName)
    {
        CategoryCd = categoryCd;
        FileTemplateName = templateFileName;
    }

    public int CategoryCd { get; private set; }

    public string FileTemplateName { get; private set; }
}
