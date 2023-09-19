using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.KensaIraiReport;

public class KensaIraiReportOutputData : IOutputData
{
    public KensaIraiReportOutputData(string pdfFile, string datFile, KensaIraiReportStatus status)
    {
        PdfFile = pdfFile;
        DatFile = datFile;
        Status = status;
    }

    public string PdfFile { get; private set; }

    public string DatFile { get; private set; }

    public KensaIraiReportStatus Status { get; private set; }
}
