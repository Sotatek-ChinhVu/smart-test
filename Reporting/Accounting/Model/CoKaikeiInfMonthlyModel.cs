namespace Reporting.Accounting.Model;

public class CoKaikeiInfMonthlyModel
{
    public CoKaikeiInfMonthlyModel(int hpId, long ptId, int sinYm, int seikyuGaku, int nyukinKonkai, int nyukinZenkai, int ptFutan, int adjustFutan, int adjustRound, int jihiFutan, int jihiOuttax, int jihiTax, int tensu, int totalNyukin)
    {
        HpId = hpId;
        PtId = ptId;
        SinYm = sinYm;
        SeikyuGaku = seikyuGaku;
        NyukinKonkai = nyukinKonkai;
        NyukinZenkai = nyukinZenkai;
        PtFutan = ptFutan;
        AdjustFutan = adjustFutan;
        AdjustRound = adjustRound;
        JihiFutan = jihiFutan;
        JihiOuttax = jihiOuttax;
        JihiTax = jihiTax;
        Tensu = tensu;
        TotalNyukin = totalNyukin;
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
    /// 診療月
    /// </summary>
    public int SinYm { get; set; }
    /// <summary>
    /// 請求額
    /// </summary>
    public int SeikyuGaku { get; set; }
    /// <summary>
    /// 今回入金額
    /// </summary>
    public int NyukinKonkai { get; set; }
    /// <summary>
    /// 前回入金額
    /// </summary>
    public int NyukinZenkai { get; set; }
    /// <summary>
    /// 患者負担額
    /// </summary>
    public int PtFutan { get; set; }
    public int AdjustFutan { get; set; }
    public int AdjustRound { get; set; }
    /// <summary>
    /// 自費負担額
    /// </summary>
    public int JihiFutan { get; set; }
    /// <summary>
    /// 自費項目金額合計
    /// </summary>
    public int JihiKoumokuAll { get; set; }
    /// <summary>
    /// 自費外税
    /// </summary>
    public int JihiOuttax { get; set; }
    /// <summary>
    /// 自費内税
    /// </summary>
    public int JihiTax { get; set; }
    /// <summary>
    /// 診療点数
    /// </summary>
    public int Tensu { get; set; }
    /// <summary>
    /// 入金額
    /// </summary>
    public int TotalNyukin { get; set; }
}
