namespace UseCase.KarteInf.GetList;

public class KarteFileOutputItem
{
    public KarteFileOutputItem(long id, string fileName)
    {
        Id = id;
        FileName = fileName;
    }

    public long Id { get; private set; }

    public string FileName { get; private set; }
}
