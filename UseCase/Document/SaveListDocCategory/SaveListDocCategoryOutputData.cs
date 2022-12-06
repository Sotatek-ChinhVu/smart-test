using UseCase.Core.Sync.Core;

namespace UseCase.Document.SaveListDocCategory;

public class SaveListDocCategoryOutputData : IOutputData
{
    public SaveListDocCategoryOutputData(SaveListDocCategoryStatus status)
    {
        Status = status;
    }

    public SaveListDocCategoryStatus Status { get; private set; }
}
