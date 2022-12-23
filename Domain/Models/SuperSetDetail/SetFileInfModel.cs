namespace Domain.Models.SuperSetDetail;

public class SetFileInfModel
{
    public SetFileInfModel(bool isSchema, string linkFile)
    {
        IsSchema = isSchema;
        LinkFile = linkFile;
    }

    public bool IsSchema { get; private set; }

    public string LinkFile { get; private set; }
}
