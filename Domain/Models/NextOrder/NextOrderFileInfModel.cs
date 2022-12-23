namespace Domain.Models.NextOrder;

public class NextOrderFileInfModel
{
    public NextOrderFileInfModel(bool isSchema, string linkFile)
    {
        IsSchema = isSchema;
        LinkFile = linkFile;
    }

    public bool IsSchema { get; private set; }

    public string LinkFile { get; private set; }
}
