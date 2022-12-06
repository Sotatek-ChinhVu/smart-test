namespace UseCase.Schema.SaveListImageTodayOrder;

public class FileItem
{
    public FileItem(string fileName, Stream streamImage)
    {
        FileName = fileName;
        StreamImage = streamImage;
    }

    public string FileName { get; private set; }

    public Stream StreamImage { get; private set; }
}
