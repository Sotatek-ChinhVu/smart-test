using Domain.Models.Document;

namespace EmrCloudApi.Tenant.Responses.Document;

public class FileDocumentDto
{
    public FileDocumentDto(FileDocumentModel model)
    {
        FileName = model.FileName;
        FileLink = model.FileLink;
    }

    public string FileName { get; private set; }
    public string FileLink { get; private set; }
}
