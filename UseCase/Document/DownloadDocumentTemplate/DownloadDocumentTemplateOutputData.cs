using UseCase.Core.Sync.Core;

namespace UseCase.Document.DownloadDocumentTemplate;

public class DownloadDocumentTemplateOutputData : IOutputData
{
    public DownloadDocumentTemplateOutputData(MemoryStream outputStream, DownloadDocumentTemplateStatus status)
    {
        OutputStream = outputStream;
        Status = status;
    }

    public DownloadDocumentTemplateOutputData(DownloadDocumentTemplateStatus status)
    {
        OutputStream = new MemoryStream();
        Status = status;
    }

    public MemoryStream OutputStream { get; private set; }

    public DownloadDocumentTemplateStatus Status { get; private set; }
}
