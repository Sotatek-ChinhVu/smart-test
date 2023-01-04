namespace UseCase.KarteInf.GetList;

public class KarteFileOutputItem
{
    public KarteFileOutputItem(bool isSchema, string fileLink)
    {
        IsSchema = isSchema;
        FileLink = fileLink;
    }

    public bool IsSchema { get; private set; }

    public string FileLink { get; private set; }
}
