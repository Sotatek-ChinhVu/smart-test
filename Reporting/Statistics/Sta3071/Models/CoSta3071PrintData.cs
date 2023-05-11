using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3071.Models;

public class CoSta3071PrintData
{
    public CoSta3071PrintData(RowType rowType = RowType.Data)
    {
        RowType = rowType;
    }

    /// <summary>
    /// 行タイプ
    /// </summary>
    public RowType RowType { get; set; }

    /// <summary>
    /// 行タイトル
    /// </summary>
    public string RowTitle { get; set; } = string.Empty;

    /// <summary>
    /// 初再診見出し
    /// </summary>
    public string SyosaiMidasi { get; set; } = string.Empty;

    /// <summary>
    /// 集計
    /// </summary>
    public List<string> MainVals { get; set; } = new();

}
