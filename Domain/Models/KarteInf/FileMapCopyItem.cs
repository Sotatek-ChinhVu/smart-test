namespace Domain.Models.KarteInf;

public class FileMapCopyItem
{
    public FileMapCopyItem(string oldFileName, string newFileName)
    {
        OldFileName = oldFileName;
        NewFileName = newFileName;
    }

    public string OldFileName { get;private set; }

    public string NewFileName { get;private set; }
}
