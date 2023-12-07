using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta1002.Models;

public class CoSta1002PrintData
{
    public CoSta1002PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 入金日
    /// </summary>
    public int NyukinDate { get; set; }

    /// <summary>
    /// 入金日 (yyyy/MM/dd)
    /// </summary>
    public string NyukinDateFmt
    {
        get => CIUtil.SDateToShowSDate(NyukinDate);
    }

    /// <summary>
    /// 受付種別
    /// </summary>
    public string UketukeSbt { get; set; } = string.Empty;

    /// <summary>
    /// 受付種別名称
    /// </summary>
    public string UketukeSbtName { get; set; } = string.Empty;

    /// <summary>
    /// 診療科ID
    /// </summary>
    public string KaId { get; set; } = string.Empty;

    /// <summary>
    /// 診療科略称
    /// </summary>
    public string KaSname { get; set; } = string.Empty;

    /// <summary>
    /// 担当医ID
    /// </summary>
    public string TantoId { get; set; } = string.Empty;

    /// <summary>
    /// 担当医略称
    /// </summary>
    public string TantoSname { get; set; } = string.Empty;

    /// <summary>
    /// 保険種別名
    /// </summary>
    public string HokenSbtName { get; set; } = string.Empty;

    /// <summary>
    /// 初診件数
    /// </summary>
    public string SyosinCount { get; set; } = string.Empty;

    /// <summary>
    /// 再診件数
    /// </summary>
    public string SaisinCount { get; set; } = string.Empty;

    /// <summary>
    /// 合計件数
    /// </summary>
    public string Count { get; set; } = string.Empty;

    /// <summary>
    /// 合計点数
    /// </summary>
    public string Tensu { get; set; } = string.Empty;

    /// <summary>
    /// 合計点数(新)
    /// </summary>
    public string NewTensu { get; set; } = string.Empty;

    /// <summary>
    /// 負担金額
    /// </summary>
    public string PtFutan { get; set; } = string.Empty;

    /// <summary>
    /// 保険外金額
    /// </summary>
    public string JihiFutan { get; set; } = string.Empty;

    /// <summary>
    /// 消費税
    /// </summary>
    public string JihiTax { get; set; } = string.Empty;

    /// <summary>
    /// 調整額
    /// </summary>
    public string AdjustFutan { get; set; } = string.Empty;

    /// <summary>
    /// 合計請求額
    /// </summary>
    public string SeikyuGaku { get; set; } = string.Empty;

    /// <summary>
    /// 合計請求額(新)
    /// </summary>
    public string NewSeikyuGaku { get; set; } = string.Empty;

    /// <summary>
    /// 免除額
    /// </summary>
    public string MenjyoGaku { get; set; } = string.Empty;

    /// <summary>
    /// 入金額
    /// </summary>
    public string NyukinGaku { get; set; } = string.Empty;

    /// <summary>
    /// 未収額
    /// </summary>
    public string MisyuGaku { get; set; } = string.Empty;

    /// <summary>
    /// 期間外入金額
    /// </summary>
    public string PostNyukinGaku { get; set; } = string.Empty;

    /// <summary>
    /// 期間外調整額
    /// </summary>
    public string PostAdjustFutan { get; set; } = string.Empty;

    /// <summary>
    /// 保険種別ごとの金額
    /// </summary>
    public List<string> JihiSbtFutans { get; set; } = new();
}
