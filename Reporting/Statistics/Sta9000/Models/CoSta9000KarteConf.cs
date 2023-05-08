namespace Reporting.Statistics.Sta9000.Models;

public class CoSta9000KarteConf
{
    /// <summary>
    /// カルテ区分
    /// </summary>
    public List<int> KarteKbns { get; set; } = new();

    /// <summary>
    /// 文字列検索
    /// </summary>
    public List<string> SearchWords { get; set; } = new();

    /// <summary>
    /// 検索ワードの検索オプション
    ///     0:or 1:and
    /// </summary>
    public int WordOpt { get; set; }
}
