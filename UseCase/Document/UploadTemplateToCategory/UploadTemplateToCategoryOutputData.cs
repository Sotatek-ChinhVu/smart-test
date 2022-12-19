using UseCase.Core.Sync.Core;

namespace UseCase.Document.UploadTemplateToCategory;

public class UploadTemplateToCategoryOutputData : IOutputData
{
    public UploadTemplateToCategoryOutputData(UploadTemplateToCategoryStatus status)
    {
        Status = status;
    }

    public UploadTemplateToCategoryStatus Status { get; private set; }
}
