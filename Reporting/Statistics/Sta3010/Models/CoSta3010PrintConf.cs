using Helper.Common;

namespace Reporting.Statistics.Sta3010.Models;

public class CoSta3010PrintConf
{
    public CoSta3010PrintConf(int menuId)
    {
        MenuId = menuId;
        StdDate = CIUtil.DateTimeToInt(DateTime.Today);
        TgtData = 0;
        SearchOpt = 0;
        OtherItemOpt = 0;
    }

    public CoSta3010PrintConf()
    {
        MenuId = 0;
        StdDate = CIUtil.DateTimeToInt(DateTime.Today);
        TgtData = 0;
        SearchOpt = 0;
        OtherItemOpt = 0;
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
    /// 基準日
    /// </summary>
    public int StdDate { get; set; }

    /// <summary>
    /// 改ページ１
    ///     1: セット区分
    /// </summary>
    public int PageBreak1 { get; set; }

    /// <summary>
    /// セット区分
    /// </summary>
    public List<List<int>> SetKbns
    {
        get
        {
            return new List<List<int>> { Set1, Set2, Set3, Set4, Set5, Set6, Set7, Set8, Set9, Set10 };
        }
    }

    /// <summary>
    /// セット区分１
    ///     0: セット1-1
    ///     1: セット1-2
    ///     2: セット1-3
    ///     3: セット1-4
    ///     4: セット1-5
    ///     5: セット1-6
    /// </summary>
    public List<int> Set1 { get; set; } = new();

    /// <summary>
    /// セット区分２
    ///     0: セット2-1
    ///     1: セット2-2
    ///     2: セット2-3
    ///     3: セット2-4
    ///     4: セット2-5
    ///     5: セット2-6
    /// </summary>
    public List<int> Set2 { get; set; } = new();

    /// <summary>
    /// セット区分３
    ///     0: セット3-1
    ///     1: セット3-2
    ///     2: セット3-3
    ///     3: セット3-4
    ///     4: セット3-5
    ///     5: セット3-6
    /// </summary>
    public List<int> Set3 { get; set; } = new();

    /// <summary>
    /// セット区分４
    ///     0: セット4-1
    ///     1: セット4-2
    ///     2: セット4-3
    ///     3: セット4-4
    ///     4: セット4-5
    ///     5: セット4-6
    /// </summary>
    public List<int> Set4 { get; set; } = new();

    /// <summary>
    /// セット区分５
    ///     0: セット5-1
    ///     1: セット5-2
    ///     2: セット5-3
    ///     3: セット5-4
    ///     4: セット5-5
    ///     5: セット5-6
    /// </summary>
    public List<int> Set5 { get; set; } = new();

    /// <summary>
    /// セット区分６
    ///     0: セット6-1
    ///     1: セット6-2
    ///     2: セット6-3
    ///     3: セット6-4
    ///     4: セット6-5
    ///     5: セット6-6
    /// </summary>
    public List<int> Set6 { get; set; } = new();

    /// <summary>
    /// セット区分７
    ///     0: セット7-1
    ///     1: セット7-2
    ///     2: セット7-3
    ///     3: セット7-4
    ///     4: セット7-5
    ///     5: セット7-6
    /// </summary>
    public List<int> Set7 { get; set; } = new();

    /// <summary>
    /// セット区分８
    ///     0: セット8-1
    ///     1: セット8-2
    ///     2: セット8-3
    ///     3: セット8-4
    ///     4: セット8-5
    ///     5: セット8-6
    /// </summary>
    public List<int> Set8 { get; set; } = new();

    /// <summary>
    /// セット区分９
    ///     0: セット9-1
    ///     1: セット9-2
    ///     2: セット9-3
    ///     3: セット9-4
    ///     4: セット9-5
    ///     5: セット9-6
    /// </summary>
    public List<int> Set9 { get; set; } = new();

    /// <summary>
    /// セット区分１０
    ///     0: セット10-1
    ///     1: セット10-2
    ///     2: セット10-3
    ///     3: セット10-4
    ///     4: セット10-5
    ///     5: セット10-6
    /// </summary>
    public List<int> Set10 { get; set; } = new();

    /// <summary>
    /// 対象データ
    ///     0:すべて
    ///     1: 期限切れ項目
    ///     2: 項目選択
    ///     3: フリーコメント
    ///     4: 部位
    /// </summary>
    public int TgtData { get; set; }

    /// <summary>
    /// セット内の他の項目
    ///     0:含まない 
    ///     1: 含む
    /// </summary>
    public int OtherItemOpt { get; set; }

    /// <summary>
    /// 検索項目
    /// </summary>
    public List<string> ItemCds { get; set; } = new();

    /// <summary>
    /// 検索項目の検索オプション
    ///     0:or 1:and
    /// </summary>
    public int ItemSearchOpt { get; set; }

    /// <summary>
    /// 検索ワード
    /// </summary>
    public string SearchWord { get; set; } = string.Empty;

    /// <summary>
    /// 検索ワードリスト
    /// </summary>
    public List<string> ListSearchWord
    {
        get
        {
            List<string> searchWords = new List<string>();
            if (SearchWord != null)
            {
                //スペース区切りでキーワードを分解
                string[] values = SearchWord.Replace("　", " ").Split(' ');
                searchWords.AddRange(values);
            }
            return searchWords;
        }
    }

    /// <summary>
    /// 検索ワードの検索オプション
    ///     0:or 
    ///     1:and
    /// </summary>
    public int SearchOpt { get; set; }

}
