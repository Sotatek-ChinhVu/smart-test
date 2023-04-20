namespace EmrCloudApi.Requests.ExportPDF;

public class PeriodReceiptRequestItem
{
    public long PtId { get; set; }

    public int HokenId { get; set; }

    public int HokenKbn { get; set; }

    public List<long> RaiinNos { get; set; }
}
