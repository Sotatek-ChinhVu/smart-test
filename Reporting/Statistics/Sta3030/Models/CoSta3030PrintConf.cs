namespace Reporting.Statistics.Sta3030.Models;

public class CoSta3030PrintConf
{
    public CoSta3030PrintConf(int menuId)
    {
        MenuId = menuId;
        StartDateFrom = -1;
        StartDateTo = -1;
        TenkiDateFrom = -1;
        TenkiDateTo = -1;
        EnableRangeFrom = -1;
        EnableRangeTo = -1;
        SinDateFrom = -1;
        SinDateTo = -1;
    }

    public CoSta3030PrintConf()
    {
        StartDateFrom = -1;
        StartDateTo = -1;
        TenkiDateFrom = -1;
        TenkiDateTo = -1;
        EnableRangeFrom = -1;
        EnableRangeTo = -1;
        SinDateFrom = -1;
        SinDateTo = -1;
    }

    /// <summary>
    /// STA_MENU.MENU_ID
    /// </summary>
    public int MenuId { get; }

    /// <summary>
    /// 帳票タイトル
    /// </summary>
    public string ReportName { get; set; } = string.Empty;

    /// <summary>
    /// フォームファイル名
    /// </summary>
    public string FormFileName { get; set; } = string.Empty;

    /// <summary>
    /// 改ページ１
    ///     1:患者番号
    /// </summary>
    public int PageBreak1 { get; set; }

    /// <summary>
    /// 改ページ２
    ///     1:患者番号
    /// </summary>
    public int PageBreak2 { get; set; }

    /// <summary>
    /// 改ページ３
    ///     1:患者番号
    /// </summary>
    public int PageBreak3 { get; set; }

    /// <summary>
    /// ソート順１
    ///     1:患者番号 2:病名 3:開始日 4:カナ氏名 5:転帰区分 6:転帰日
    /// </summary>
    public int SortOrder1 { get; set; }

    /// <summary>
    /// ソート順１
    ///     0:昇順 1:降順
    /// </summary>
    public int SortOpt1 { get; set; }

    /// <summary>
    /// ソート順２
    ///    1:患者番号 2:病名 3:開始日 4:カナ氏名 5:転帰区分 6:転帰日
    /// </summary>
    public int SortOrder2 { get; set; }

    /// <summary>
    /// ソート順２
    ///     0:昇順 1:降順
    /// </summary>
    public int SortOpt2 { get; set; }

    /// <summary>
    /// ソート順３
    ///     1:患者番号 2:病名 3:開始日 4:カナ氏名 5:転帰区分 6:転帰日
    /// </summary>
    public int SortOrder3 { get; set; }

    /// <summary>
    /// ソート順３
    ///     0:昇順 1:降順
    /// </summary>
    public int SortOpt3 { get; set; }

    /// <summary>
    /// テスト患者の有無
    /// </summary>
    public bool IsTester { get; set; }

    /// <summary>
    /// 開始日From
    ///     -1:未指定
    /// </summary>
    public int StartDateFrom { get; set; }

    /// <summary>
    /// 開始日To
    ///     -1:未指定
    /// </summary>
    public int StartDateTo { get; set; }

    /// <summary>
    /// 転帰日From
    ///     -1:未指定
    /// </summary>
    public int TenkiDateFrom { get; set; }

    /// <summary>
    /// 転帰日To
    ///     -1:未指定
    /// </summary>
    public int TenkiDateTo { get; set; }

    /// <summary>
    /// 有効期間From
    ///     -1:未指定
    /// </summary>
    public int EnableRangeFrom { get; set; }

    /// <summary>
    /// 有効期間To
    ///     -1:未指定
    /// </summary>
    public int EnableRangeTo { get; set; }

    /// <summary>
    /// 転帰区分
    ///     1:治ゆ 2:中止 3:死亡 9:その他
    /// </summary>
    public List<int> TenkiKbns { get; set; } = new();

    /// <summary>
    /// 主病名
    ///     0:主病名以外 1:主病名
    /// </summary>
    public List<int> SyubyoKbns { get; set; } = new();

    /// <summary>
    /// 疑い
    ///     0:疑い以外 1:疑い
    /// </summary>
    public List<int> DoubtKbns { get; set; } = new();

    /// <summary>
    /// 患者番号
    /// </summary>
    public List<long> PtIds { get; set; } = new();

    /// <summary>
    /// 来院日From
    ///     -1:未指定
    /// </summary>
    public int SinDateFrom { get; set; }

    /// <summary>
    /// 来院日To
    ///     -1:未指定
    /// </summary>
    public int SinDateTo { get; set; }

    /// <summary>
    /// 病名検索ワード
    /// </summary>
    public string ByomeiWords { get; set; } = string.Empty;

    /// <summary>
    /// 病名検索ワードの検索オプション（病名毎）
    ///     0:or 1:and
    /// </summary>
    public int ByomeiWordOpt { get; set; }

    /// <summary>
    /// 検索病名
    /// </summary>
    public List<string> ByomeiCds { get; set; } = new();

    /// <summary>
    /// 未コード化傷病名
    /// </summary>
    public List<string> FreeByomeis { get; set; } = new();

    /// <summary>
    /// 検索病名の検索オプション（患者毎）
    ///     0:or 1:and
    /// </summary>
    public int ByomeiCdOpt { get; set; }

    /// <summary>
    /// 算定/オーダー
    ///     0:算定 1:オーダー
    /// </summary>
    public int SanteiOrder { get; set; }

    /// <summary>
    /// 項目検索ワード
    /// </summary>
    public string ItemWords { get; set; } = string.Empty;

    /// <summary>
    /// 項目検索ワードの検索オプション（項目毎）
    ///     0:or 1:and
    /// </summary>
    public int ItemWordOpt { get; set; }

    /// <summary>
    /// 検索項目
    /// </summary>
    public List<string> ItemCds { get; set; } = new();

    /// <summary>
    /// 検索項目の検索オプション（患者毎）
    ///     0:or 1:and
    /// </summary>
    public int ItemCdOpt { get; set; }

    /// <summary>
    /// 来院情報の条件の有無
    /// </summary>
    public bool IsRaiinConf
    {
        get => SinDateFrom > 0 || SinDateTo > 0;
    }

    /// <summary>
    /// 診療情報の条件の有無
    /// </summary>
    public bool IsSinConf
    {
        get => ItemWords != string.Empty || ItemCds?.Count > 0;
    }
}