using UseCase.Core.Sync.Core;

namespace UseCase.Document.AddTemplateToCategory;

public class AddTemplateToCategoryOutputData : IOutputData
{
    public AddTemplateToCategoryOutputData(AddTemplateToCategoryStatus status)
    {
        Status = status;
    }

    public AddTemplateToCategoryStatus Status { get; private set; }
}
