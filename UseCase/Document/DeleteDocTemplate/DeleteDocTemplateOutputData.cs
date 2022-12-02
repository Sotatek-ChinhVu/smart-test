using UseCase.Core.Sync.Core;

namespace UseCase.Document.DeleteDocTemplate;

public class DeleteDocTemplateOutputData : IOutputData
{
    public DeleteDocTemplateOutputData(DeleteDocTemplateStatus status)
    {
        Status = status;
    }

    public DeleteDocTemplateStatus Status { get; private set; }
}
