using Entity.Tenant;

namespace Reporting.AccountingCardList.Model;

public class CoKaikeiInfModel
{
    public KaikeiInf KaikeiInf { get; }
    public List<KaikeiDetail> KaikeiDtls { get; }

    /// <summary>
    /// 会計情報
    /// </summary>
    /// <param name="kaikeiInf"></param>
    /// <param name="kaikeiDtls"></param>
    public CoKaikeiInfModel(
        KaikeiInf kaikeiInf, List<KaikeiDetail> kaikeiDtls)
    {
        KaikeiInf = kaikeiInf;
        KaikeiDtls = kaikeiDtls;
    }

    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    public int HpId
    {
        get => KaikeiInf.HpId;
    }
    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId
    {
        get => KaikeiInf.PtId;
    }
    public int SinDate
    {
        get => KaikeiInf.SinDate;
    }
    /// <summary>
    /// 来院番号
    /// </summary>
    public long RaiinNo
    {
        get => KaikeiInf.RaiinNo;
    }

    /// <summary>
    /// 診療点数
    /// </summary>
    public int Tensu
    {
        get => KaikeiInf?.Tensu ?? 0;
    }

    /// <summary>
    /// 自費負担額
    /// </summary>
    public int JihiFutan
    {
        get => KaikeiInf?.JihiFutan ?? 0;
    }
    /// <summary>
    /// 自費内税
    /// </summary>
    public int JihiTax
    {
        get => KaikeiInf?.JihiTax ?? 0;
    }
    /// <summary>
    /// 自費外税
    /// </summary>
    public int JihiOuttax
    {
        get => KaikeiInf?.JihiOuttax ?? 0;
    }
    /// <summary>
    /// 患者負担額
    /// </summary>
    public int PtFutan
    {
        get => KaikeiInf?.PtFutan ?? 0;
    }
    /// <summary>
    /// 患者負担合計k額
    /// </summary>
    public int TotalPtFutan
    {
        get => KaikeiInf?.TotalPtFutan ?? 0;
    }

    public int Nissu
    {
        get => KaikeiDtls.Sum(p => p.Jitunisu) > 0 ? 1 : 0;
    }
}