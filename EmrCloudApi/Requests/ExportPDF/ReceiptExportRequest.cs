namespace EmrCloudApi.Requests.ExportPDF;

public class ReceiptExportRequest
{
    public long PtId { get; set; }

    public int PrintType { get; set; }

    public List<long> RaiinNoList { get; set; } = new();

    public List<long> RaiinNoPayList { get; set; } = new();

    public bool IsCalculateProcess { get; set; }
}
