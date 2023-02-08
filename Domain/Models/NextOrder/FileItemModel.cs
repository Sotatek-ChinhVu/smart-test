namespace Domain.Models.NextOrder;

public class FileItemModel
{
    public FileItemModel(bool isUpdateFile, List<string> listFileItems)
    {
        IsUpdateFile = isUpdateFile;
        ListFileItems = listFileItems;
    }

    public FileItemModel()
    {
        IsUpdateFile = false;
        ListFileItems = new();
    }

    public bool IsUpdateFile { get; private set; }

    public List<string> ListFileItems { get; private set; }
}
