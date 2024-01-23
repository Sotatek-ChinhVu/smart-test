using Entity.Tenant;
using Helper.Extension;

namespace Reporting.Statistics.Sta3041.Models;

public class CoKouseisinInf
{
    public PtInf PtInf { get; }

    public OdrInf OdrInf { get; }

    public OdrInfDetail OdrInfDetail { get; }

    public TenMst TenMst { get; }

    public YakkaSyusaiMst YakkaSyusaiMst { get; }

    public DrugUnitConv DrugUnitConv { get; }

    public CoKouseisinInf(PtInf ptInf, int sinDate, TenMst tenMst, int drugCount)
    {
        PtInf = ptInf;
        SinDate = sinDate;
        TenMst = tenMst;
        DrugCount = drugCount;
    }

    /// <summary>
    /// 診療年月
    /// </summary>
    public int SinYm
    {
        get => SinDate / 100;
    }

    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum
    {
        get => PtInf.PtNum.AsLong();
    }

    /// <summary>
    /// カナ氏名
    /// </summary>
    public string KanaName
    {
        get => PtInf.KanaName ?? string.Empty;
    }

    /// <summary>
    /// 漢字氏名
    /// </summary>
    public string PtName
    {
        get => PtInf.Name ?? string.Empty;
    }

    /// <summary>
    /// 診療日
    /// </summary>
    public int SinDate { get; set; }


    /// <summary>
    /// 向精神薬区分コード
    /// 1:抗不安薬 2:睡眠薬 3:抗うつ薬 4:抗精神病薬
    /// </summary>
    public int KouseisinKbnCd
    {
        get => TenMst.KouseisinKbn;
    }

    /// <summary>
    /// 向精神薬区分
    /// </summary>
    public string KouseisinKbn
    {
        get
        {
            switch (TenMst.KouseisinKbn)
            {
                case 1: return "抗不安薬";
                case 2: return "睡眠薬";
                case 3: return "抗うつ薬";
                case 4: return "抗精神病薬";
            }
            return "";
        }
    }

    /// <summary>
    /// 薬価基準コード
    /// </summary>
    public string YakkaCd
    {
        get => TenMst.YakkaCd ?? string.Empty;
    }

    /// <summary>
    /// 薬価基準コード前7桁
    /// </summary>
    public string YakkaCd7
    {
        get => TenMst.YakkaCd ?? string.Empty.Substring(0, 7);
    }

    /// <summary>
    /// 種類数
    /// </summary>
    public int DrugCount { get; set; }

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public string ItemCd
    {
        get => TenMst.ItemCd;
    }

    /// <summary>
    /// 医薬品名称
    /// </summary>
    public string DrugName
    {
        get => TenMst.Name ?? string.Empty;
    }

}
