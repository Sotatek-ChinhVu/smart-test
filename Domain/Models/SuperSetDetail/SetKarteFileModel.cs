namespace Domain.Models.SuperSetDetail;

public class SetKarteFileModel
{
    public SetKarteFileModel(long id, string fileName)
    {
        Id = id;
        FileName = fileName;
    }

    public long Id { get; private set; }

    public string FileName { get; private set; }
}
