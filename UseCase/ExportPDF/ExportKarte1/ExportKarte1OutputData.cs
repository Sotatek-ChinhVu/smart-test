using UseCase.Core.Sync.Core;

namespace UseCase.ExportPDF.ExportKarte1;

public class ExportKarte1OutputData : IOutputData
{
    public ExportKarte1OutputData(ExportKarte1Status status)
    {
        Status = status;
        Base64String = string.Empty;
    }

    public ExportKarte1OutputData(string base64String, ExportKarte1Status status)
    {
        Base64String = base64String;
        Status = status;
    }

    public string Base64String { get; private set; }
    public ExportKarte1Status Status { get; private set; }

}
