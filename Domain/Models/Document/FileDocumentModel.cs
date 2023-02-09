namespace Domain.Models.Document;

public class FileDocumentModel
{
    public FileDocumentModel(int categoryId, string fileName, string fileLink)
    {
        CategoryId = categoryId;
        FileName = fileName;
        FileLink = fileLink;
    }

    public int CategoryId { get; private set; }

    public string FileName { get; private set; }

    public string FileLink { get; private set; }
}
