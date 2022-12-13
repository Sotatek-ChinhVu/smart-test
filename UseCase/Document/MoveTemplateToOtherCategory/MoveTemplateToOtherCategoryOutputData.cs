using UseCase.Core.Sync.Core;

namespace UseCase.Document.MoveTemplateToOtherCategory;

public class MoveTemplateToOtherCategoryOutputData : IOutputData
{
    public MoveTemplateToOtherCategoryOutputData(MoveTemplateToOtherCategoryStatus status)
    {
        Status = status;
    }

    public MoveTemplateToOtherCategoryStatus Status { get; private set; }
}
