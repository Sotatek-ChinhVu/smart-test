namespace Domain.Models.KarteInf;

public class SaveFileInfModel
{
    public SaveFileInfModel(bool isSchema, string fileName)
    {
        IsSchema = isSchema;
        FileName = fileName;
    }

    public bool IsSchema { get; private set; }

    public string FileName { get; private set; }
}
