namespace UseCase.SuperSetDetail.SaveSuperSetDetail;

public class FileItemInputItem
{
    public FileItemInputItem(bool isUpdateFile, List<string> listFileItems)
    {
        IsUpdateFile = isUpdateFile;
        ListFileItems = listFileItems;
    }

    public bool IsUpdateFile { get; private set; }

    public List<string> ListFileItems { get; private set; }
}
