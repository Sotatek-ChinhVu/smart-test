namespace Domain.Models.NextOrder;

public class NextOrderFileModel
{
    public NextOrderFileModel(long id, string fileName)
    {
        Id = id;
        FileName = fileName;
    }

    public long Id { get; private set; }

    public string FileName { get; private set; }
}
