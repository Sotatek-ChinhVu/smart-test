using UseCase.Core.Sync.Core;

namespace UseCase.Document.DeleteDocInf;

public class DeleteDocInfOutputData : IOutputData
{
    public DeleteDocInfOutputData(DeleteDocInfStatus status)
    {
        Status = status;
    }

    public DeleteDocInfStatus Status { get; private set; }
}
