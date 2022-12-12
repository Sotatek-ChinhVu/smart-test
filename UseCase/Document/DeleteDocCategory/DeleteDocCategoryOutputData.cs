using UseCase.Core.Sync.Core;

namespace UseCase.Document.DeleteDocCategory;

public class DeleteDocCategoryOutputData : IOutputData
{
    public DeleteDocCategoryOutputData(DeleteDocCategoryStatus status)
    {
        Status = status;
    }

    public DeleteDocCategoryStatus Status { get; private set; }
}
