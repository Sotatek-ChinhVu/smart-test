namespace EmrCloudApi.Requests.ExportPDF;

public class ReceiptCheckRequest
{
    public List<long> PtIds { get; set; } = new();

    public int SeikyuYm { get; set; }
}
