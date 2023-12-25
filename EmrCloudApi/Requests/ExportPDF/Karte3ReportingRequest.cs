namespace EmrCloudApi.Requests.ExportPDF;

public class Karte3ReportingRequest
{
    public long PtId { get; set; }

    public int StartSinYm { get; set; }

    public int EndSinYm { get; set; }

    public bool IncludeHoken { get; set; }

    public bool IncludeJihi { get; set; }
}
