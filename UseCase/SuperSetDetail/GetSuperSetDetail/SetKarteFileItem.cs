namespace UseCase.SuperSetDetail.GetSuperSetDetail;

public class SetKarteFileItem
{
    public SetKarteFileItem(long id, string fileName)
    {
        Id = id;
        FileName = fileName;
    }

    public long Id { get; private set; }

    public string FileName { get; private set; }
}
