using UseCase.Core.Sync.Core;

namespace UseCase.ExportPDF.ExportKarte1;

public class ExportKarte1OutputData : IOutputData
{
    public ExportKarte1OutputData(string url, ExportKarte1Status status)
    {
        Url = url;
        Status = status;
    }

    public ExportKarte1OutputData(ExportKarte1Status status)
    {
        Url = string.Empty;
        Status = status;
    }

    public string Url { get; private set; }

    public ExportKarte1Status Status { get; private set; }

}
