using UseCase.Core.Sync.Core;

namespace UseCase.Document.ReplaceParamTemplate;

public class ReplaceParamTemplateOutputData : IOutputData
{
    public ReplaceParamTemplateOutputData(ReplaceParamTemplateStatus status)
    {
        Status = status;
    }

    public ReplaceParamTemplateStatus Status { get; set; }
}
