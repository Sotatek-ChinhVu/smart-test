namespace Reporting.Accounting.Model;

public class CoAccountingParamModel
{
    public CoAccountingParamModel(long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId = 0,
        int miseisanKbn = 0, int saiKbn = 0, int misyuKbn = 0, int seikyuKbn = 1, int hokenKbn = 0,
        bool hokenSeikyu = false, bool jihiSeikyu = false, bool nyukinBase = false,
        int hakkoDay = 0, string memo = "",
        int printType = 0,
        string formFileDirectory = "", string formFileName = "")
    {
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
        HakkoDate = hakkoDay;
        Memo = memo;
        PrintType = printType;
        FormFileDirectory = formFileDirectory;
        FormFileName = formFileName;
    }
    public long PtId { get; set; } = 0;
    public int StartDate { get; set; }
    public int EndDate { get; set; }
    public List<long> RaiinNos { get; set; }
    public int HokenId { get; set; } = 0;
    public int MiseisanKbn { get; set; } = 0;
    public int SaiKbn { get; set; } = 0;
    public int MisyuKbn { get; set; } = 0;
    public int SeikyuKbn { get; set; } = 0;
    public int HokenKbn { get; set; } = 0;
    public bool HokenSeikyu { get; set; } = false;
    public bool JihiSeikyu { get; set; } = false;
    public bool NyukinBase { get; set; } = false;
    public int HakkoDate { get; set; } = 0;
    public string Memo { get; set; }
    public int PrintType { get; set; } = 0;
    public string FormFileDirectory { get; set; }
    public string FormFileName { get; set; }
}
