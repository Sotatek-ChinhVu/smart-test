namespace UseCase.Schema.SaveListFileTodayOrder;

public class FileItem
{
    public FileItem(string fileName, bool isSchema, MemoryStream streamImage)
    {
        FileName = fileName;
        IsSchema = isSchema;
        StreamImage = streamImage;
    }

    public string FileName { get; private set; }

    public bool IsSchema { get; private set; }

    public MemoryStream StreamImage { get; private set; }
}
