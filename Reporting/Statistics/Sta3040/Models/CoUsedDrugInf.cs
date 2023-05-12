using Helper.Common;

namespace Reporting.Statistics.Sta3040.Models;

public class CoUsedDrugInf
{
    public CoUsedDrugInf()
    {
        SinYm = 0;
        SuryoKaisu = 0;
        Suryo = 0;
        Price = 0;
        TermVal = 0;
        CnvVal = 0;
    }

    /// <summary>
    /// 診療年月
    /// </summary>
    public int SinYm { get; set; }

    /// <summary>
    /// 診療年月(yyyy/mm)
    /// </summary>
    public string FmtSinYm
    {
        get => CIUtil.SMonthToShowSMonth(SinYm);
    }

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public string ItemCd { get; set; }

    /// <summary>
    /// 請求名称
    /// </summary>
    public string ReceName { get; set; }

    /// <summary>
    /// 数量回数
    /// </summary>
    public double SuryoKaisu { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public double Suryo { get; set; }

    /// <summary>
    /// レセ単位名称
    /// </summary>
    public string ReceUnitName { get; set; }

    /// <summary>
    /// 薬価
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// 区分
    /// </summary>
    public string Kbn { get; set; }

    /// <summary>
    /// 単位
    /// </summary>
    public string UnitName { get; set; }

    /// <summary>
    /// 単位換算値
    /// </summary>
    public double TermVal { get; set; }

    /// <summary>
    /// 換算係数
    /// </summary>
    public double CnvVal { get; set; }

    /// <summary>
    /// 換算有無
    /// </summary>
    public string ExistCnvVal { get; set; }
}