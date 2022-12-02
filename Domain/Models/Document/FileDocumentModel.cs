namespace Domain.Models.Document;

public class FileDocumentModel
{
    public FileDocumentModel(string fileName, string fileLink)
    {
        FileName = fileName;
        FileLink = fileLink;
    }

    public string FileName { get; private set; }
    public string FileLink { get; private set; }
}
