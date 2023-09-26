namespace EmrCloudApi.Responses.MainMenu;

public class KensaIraiReportResponse
{
    public KensaIraiReportResponse(string pdfFile, string datFile)
    {
        PdfFile = pdfFile;
        DatFile = datFile;
    }

    public string PdfFile { get; private set; }

    public string DatFile { get; private set; }
}
