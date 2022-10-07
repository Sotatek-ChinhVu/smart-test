using UseCase.Core.Sync.Core;

namespace UseCase.ExportPDF.ExportKarte1;

public class ExportKarte1OutputData : IOutputData
{
    public ExportKarte1OutputData(ExportKarte1Status status)
    {
        Status = status;
    }


    public ExportKarte1Status Status { get; private set; }

}
