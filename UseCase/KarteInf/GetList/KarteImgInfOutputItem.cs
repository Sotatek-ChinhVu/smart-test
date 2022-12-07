namespace UseCase.KarteInf.GetList;

public class KarteImgInfOutputItem
{
    public KarteImgInfOutputItem(long id, string fileName)
    {
        Id = id;
        FileName = fileName;
    }

    public long Id { get; private set; }

    public string FileName { get; private set; }
}
