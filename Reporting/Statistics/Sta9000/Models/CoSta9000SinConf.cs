namespace Reporting.Statistics.Sta9000.Models;

public class CoSta9000SinConf
{
    /// <summary>
    /// 対象データ
    ///     0:算定 1:オーダー
    /// </summary>
    public int DataKind { get; set; }

    /// <summary>
    /// 検索項目
    ///     ItemCd,回数下限,回数上限,..
    /// </summary>
    public List<string> ItemCds { get; set; } = new();

    /// <summary>
    /// 検索項目（未コード／コメント）
    /// </summary>
    public List<string> ItemCmts { get; set; } = new();

    /// <summary>
    /// 検索項目のオプション
    ///     0:or 1:and
    /// </summary>
    public int ItemCdOpt { get; set; }

    /// <summary>
    /// 検索ワード
    /// </summary>
    public string SearchWord { get; set; } = string.Empty;

    /// <summary>
    /// 検索ワードの検索オプション
    ///     0:or 1:and
    /// </summary>
    public int WordOpt { get; set; }
}
