using UseCase.Core.Sync.Core;

namespace UseCase.Document.ReplaceParamTemplate;

public class ReplaceParamTemplateOutputData : IOutputData
{
    public ReplaceParamTemplateOutputData(ReplaceParamTemplateStatus status)
    {
        OutputStream = new MemoryStream();
        Status = status;
    }

    public ReplaceParamTemplateOutputData(MemoryStream outputStream, ReplaceParamTemplateStatus status)
    {
        OutputStream = outputStream;
        Status = status;
    }

    public MemoryStream OutputStream { get; private set; }

    public ReplaceParamTemplateStatus Status { get; private set; }
}
