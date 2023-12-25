namespace EmrCloudApi.Requests.ExportPDF;

public class PeriodReceiptRequest
{
    public int StartDate { get; set; }

    public int EndDate { get; set; }

    public int MiseisanKbn { get; set; }

    public int SaiKbn { get; set; }

    public int MisyuKbn { get; set; }

    public int SeikyuKbn { get; set; }

    public bool HokenSeikyu { get; set; }

    public bool JihiSeikyu { get; set; }

    public bool NyukinBase { get; set; }

    public int HakkoDay { get; set; }

    public string Memo { get; set; } = string.Empty;

    public int PrintType { get; set; }

    public string FormFileName { get; set; } = string.Empty;

    public List<PeriodReceiptRequestItem> PtInfList { get; set; } = new();
}
