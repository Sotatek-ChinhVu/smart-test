namespace UseCase.MedicalExamination.UpsertTodayOrd;

public class FileItemInputItem
{
    public FileItemInputItem(bool isUpdateFile, List<string> listFileItems)
    {
        IsUpdateFile = isUpdateFile;
        ListFileItems = listFileItems;
    }

    public FileItemInputItem()
    {
        ListFileItems = new();
    }

    public bool IsUpdateFile { get; private set; }

    public List<string> ListFileItems { get; private set; }
}
