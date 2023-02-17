using Domain.Models.Document;

namespace EmrCloudApi.Responses.Document.Dto;

public class FileDocumentDto
{
    public FileDocumentDto(FileDocumentModel model)
    {
        CategoryId = model.CategoryId;
        FileName = model.FileName;
        FileLink = model.FileLink;
    }

    public int CategoryId { get; private set; }

    public string FileName { get; private set; }

    public string FileLink { get; private set; }
}
