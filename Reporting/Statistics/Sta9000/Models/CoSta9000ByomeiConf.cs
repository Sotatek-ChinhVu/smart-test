namespace Reporting.Statistics.Sta9000.Models;

public class CoSta9000ByomeiConf
{
    /// <summary>
    /// 開始日Start
    /// </summary>
    public int StartStartDate { get; set; }

    /// <summary>
    /// 開始日End
    /// </summary>
    public int EndStartDate { get; set; }

    /// <summary>
    /// 転帰日Start
    /// </summary>
    public int StartTenkiDate { get; set; }

    /// <summary>
    /// 転帰日End
    /// </summary>
    public int EndTenkiDate { get; set; }

    /// <summary>
    /// 転帰区分
    ///     1:継続 2:治ゆ 3:死亡 4:中止
    /// </summary>
    public List<int> TenkiKbns { get; set; } = new();

    /// <summary>
    /// 疾患区分
    ///     3:皮(1) 4:皮(2) 5:特疾 7:てんかん 8:特疾又はてんかん
    /// </summary>
    public List<int> SikkanKbns { get; set; } = new();

    /// <summary>
    /// 難病外来コード
    ///     0:算定対象外 0:難病外来指導管理料算定対象
    /// </summary>
    public List<int> NanbyoCds { get; set; } = new();

    /// <summary>
    /// 疑い病名
    ///     1:疑い 2:疑い以外
    /// </summary>
    public int IsDoubt { get; set; }

    /// <summary>
    /// 検索病名（病名コード）
    /// </summary>
    public List<string> ByomeiCds { get; set; } = new();

    /// <summary>
    /// 検索病名（未コード化病名）
    /// </summary>
    public List<string> Byomeis { get; set; } = new();

    /// <summary>
    /// 検索病名のオプション
    ///     0:or 1:and
    /// </summary>
    public int ByomeiCdOpt { get; set; }

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
