namespace EmrCloudApi.Requests.ExportPDF;

public class ReceiptReportRequest
{
    public ReceiptReportRequest(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, bool hokenSeikyu, bool jihiSeikyu, bool nyukinBase, int hakkoDay, string memo, int printType, string formFileName)
    {
        HpId = hpId;
        PtId = ptId;
        StartDate = startDate;
        EndDate = endDate;
        RaiinNos = raiinNos;
        HokenId = hokenId;
        MiseisanKbn = miseisanKbn;
        SaiKbn = saiKbn;
        MisyuKbn = misyuKbn;
        SeikyuKbn = seikyuKbn;
        HokenKbn = hokenKbn;
        HokenSeikyu = hokenSeikyu;
        JihiSeikyu = jihiSeikyu;
        NyukinBase = nyukinBase;
        HakkoDay = hakkoDay;
        Memo = memo;
        PrintType = printType;
        FormFileName = formFileName;
    }

    public int HpId { get; set; }

    public long PtId { get; set; }

    public int StartDate { get; set; }

    public int EndDate { get; set; }

    public List<long> RaiinNos { get; set; }

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

    public string Memo { get; set; }

    public int PrintType { get; set; }

    public string FormFileName { get; set; }
}
