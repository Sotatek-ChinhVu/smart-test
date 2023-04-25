namespace Reporting.Accounting.Model;

public class CoNyukinInfMonthlyModel
{
    public CoNyukinInfMonthlyModel(int hpId, long ptId, int nyukinYm, int nyukinGaku, int totalAdjust)
    {
        HpId = hpId;
        PtId = ptId;
        NyukinYm = nyukinYm;
        NyukinGaku = nyukinGaku;
        TotalAdjust = totalAdjust;
    }
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId { get; set; }
    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId { get; set; }
    /// <summary>
    /// 入金年月
    /// </summary>
    public int NyukinYm { get; set; }
    /// <summary>
    /// 入金額
    /// </summary>
    public int NyukinGaku { get; set; }

    public int TotalAdjust { get; set; }
}
