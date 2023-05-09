namespace Reporting.Statistics.Sta9000.Models;

public class CoSta9000KensaConf
{
    /// <summary>
    /// 依頼日Start
    /// </summary>
    public int StartIraiDate { get; set; }

    /// <summary>
    /// 依頼日End
    /// </summary>
    public int EndIraiDate { get; set; }

    /// <summary>
    /// 検査項目
    ///     項目コード,結果値下限,結果値上限,異常値<1:H 2:L 3:HorL>,… の繰り返し
    /// </summary>
    public List<string> ItemCds { get; set; } = new();

    /// <summary>
    /// 検査項目の検索オプション
    ///     0:or 1:and
    /// </summary>
    public int ItemCdOpt { get; set; }
}
