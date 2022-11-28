using UseCase.Core.Sync.Core;

namespace UseCase.Document.SortDocCategory;

public class SortDocCategoryOutputData : IOutputData
{
    public SortDocCategoryOutputData(SortDocCategoryStatus status)
    {
        Status = status;
    }

    public SortDocCategoryStatus Status { get; private set; }
}
