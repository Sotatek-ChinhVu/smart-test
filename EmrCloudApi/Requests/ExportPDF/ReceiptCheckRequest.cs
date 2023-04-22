namespace EmrCloudApi.Requests.ExportPDF;

public class ReceiptCheckRequest
{
    public int HpId { get; set; }
    public List<long> PtIds { get; set; } = new();
    public int SeikyuYm { get; set; }
}
