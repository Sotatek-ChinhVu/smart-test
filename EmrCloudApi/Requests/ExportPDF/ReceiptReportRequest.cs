namespace EmrCloudApi.Requests.ExportPDF;

public class ReceiptReportRequest
{
    public long PtId { get; set; }

    public int StartDate { get; set; }

    public int EndDate { get; set; }

    public List<long> RaiinNos { get; set; } = new();

    public int HokenId { get; set; }

    public int MiseisanKbn { get; set; }

    public int SaiKbn { get; set; }

    public int MisyuKbn { get; set; }

    public int SeikyuKbn { get; set; }

    public int HokenKbn { get; set; }

    public bool HokenSeikyu { get; set; }

    public bool JihiSeikyu { get; set; }

    public bool NyukinBase { get; set; }

    public int HakkoDay { get; set; }

    public string Memo { get; set; } = string.Empty;

    public int PrintType { get; set; }

    public string FormFileName { get; set; } = string.Empty;
}
