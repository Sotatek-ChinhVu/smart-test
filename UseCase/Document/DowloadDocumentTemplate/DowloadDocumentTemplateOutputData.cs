using UseCase.Core.Sync.Core;

namespace UseCase.Document.DowloadDocumentTemplate;

public class DowloadDocumentTemplateOutputData : IOutputData
{
    public DowloadDocumentTemplateOutputData(MemoryStream outputStream, DowloadDocumentTemplateStatus status)
    {
        OutputStream = outputStream;
        Status = status;
    }

    public DowloadDocumentTemplateOutputData(DowloadDocumentTemplateStatus status)
    {
        OutputStream = new MemoryStream();
        Status = status;
    }

    public MemoryStream OutputStream { get; private set; }

    public DowloadDocumentTemplateStatus Status { get; private set; }
}
