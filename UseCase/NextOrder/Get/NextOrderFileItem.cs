namespace UseCase.NextOrder.Get;

public class NextOrderFileItem
{
    public NextOrderFileItem(long id, string fileName)
    {
        Id = id;
        FileName = fileName;
    }

    public long Id { get; private set; }

    public string FileName { get; private set; }
}
