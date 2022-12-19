namespace UseCase.Schema.SaveListFileTodayOrder;

public class FileItem
{
    public FileItem(string fileName, MemoryStream streamImage)
    {
        FileName = fileName;
        StreamImage = streamImage;
    }

    public string FileName { get; private set; }

    public MemoryStream StreamImage { get; private set; }
}
