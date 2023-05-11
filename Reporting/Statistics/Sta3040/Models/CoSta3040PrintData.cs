using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3040.Models;

public class CoSta3040PrintData
{
    public CoSta3040PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 合計行のタイトル
    /// </summary>
    public string TotalKbn { get; set; }

    /// <summary>
    /// 合計行のキャプション
    /// </summary>
    public string TotalCaption { get; set; }

    /// <summary>
    /// 合計行の件数
    /// </summary>
    public string TotalVal { get; set; }

    /// <summary>
    /// 診療年月(yyyy/mm)
    /// </summary>
    public string SinYm { get; set; }

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
    public string SuryoKaisu { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public string Suryo { get; set; }

    /// <summary>
    /// レセ単位名称
    /// </summary>
    public string ReceUnitName { get; set; }

    /// <summary>
    /// 薬価
    /// </summary>
    public string Price { get; set; }

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
    public string TermVal { get; set; }

    /// <summary>
    /// 換算係数
    /// </summary>
    public string CnvVal { get; set; }

    /// <summary>
    /// 換算係数の有無
    /// </summary>
    public string ExistCnvVal { get; set; }

}
