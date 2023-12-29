namespace Reporting.Statistics.Sta3050.Models;

public class CoSta3050PrintConf
{
    public CoSta3050PrintConf(int menuId)
    {
        MenuId = menuId;
        StartSinDate = -1;
        EndSinDate = -1;
        StartSinYm = -1;
        EndSinYm = -1;
    }

    public CoSta3050PrintConf()
    {
        MenuId = 0;
        StartSinDate = -1;
        EndSinDate = -1;
        StartSinYm = -1;
        EndSinYm = -1;
    }

    /// <summary>
    /// STA_MENU.MENU_ID
    /// </summary>
    public int MenuId { get; }

    /// <summary>
    /// フォームファイル名
    /// </summary>
    public string FormFileName { get; set; }

    /// <summary>
    /// 帳票タイトル
    /// </summary>
    public string ReportName { get; set; }

    /// <summary>
    /// テスト患者の有無
    /// </summary>
    public bool IsTester { get; set; }

    /// <summary>
    /// 診療年月Start
    ///     -1:未指定
    /// </summary>
    public int StartSinYm { get; set; }

    /// <summary>
    /// 診療年月End
    ///     -1:未指定
    /// </summary>
    public int EndSinYm { get; set; }

    /// <summary>
    /// 診療日From
    /// </summary>
    ///     -1:未指定
    public int StartSinDate { get; set; }

    /// <summary>
    /// 診療日End
    ///     -1:未指定
    /// </summary>
    public int EndSinDate { get; set; }

    /// <summary>
    /// 患者番号Start
    ///     -1:未指定
    /// </summary>
    public long StartPtNum { get; set; }

    /// <summary>
    /// 患者番号End
    ///     -1:未指定
    /// </summary>
    public long EndPtNum { get; set; }

    /// <summary>
    /// 対象データ
    ///     0:算定 1:オーダー
    /// </summary>
    public int DataKind { get; set; }

    /// <summary>
    /// 改ページ条件１
    ///     1:診療年月 2:診療科 3:担当医
    /// </summary>
    public int PageBreak1 { get; set; }

    /// <summary>
    /// 改ページ条件２
    ///     1:診療年月 2:診療科 3:担当医
    /// </summary>
    public int PageBreak2 { get; set; }

    /// <summary>
    /// 改ページ条件３
    ///     1:診療年月 2:診療科 3:担当医
    /// </summary>
    public int PageBreak3 { get; set; }

    /// <summary>
    /// ソート順１
    ///     1:診療日 2:患者番号 3:氏名 4:診療行為区分
    /// </summary>
    public int SortOrder1 { get; set; }

    /// <summary>
    /// ソート順１オプション
    ///     0:昇順 1:降順
    /// </summary>
    public int SortOpt1 { get; set; }

    /// <summary>
    /// ソート順２
    ///     1:診療日 2:患者番号 3:氏名 4:診療行為区分
    /// </summary>
    public int SortOrder2 { get; set; }

    /// <summary>
    /// ソート順２オプション
    ///     0:昇順 1:降順
    /// </summary>
    public int SortOpt2 { get; set; }

    /// <summary>
    /// ソート順３
    ///     1:診療日 2:患者番号 3:氏名 4:診療行為区分
    /// </summary>
    public int SortOrder3 { get; set; }

    /// <summary>
    /// ソート順３オプション
    ///     0:昇順 1:降順
    /// </summary>
    public int SortOpt3 { get; set; }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public List<int> KaIds { get; set; }

    /// <summary>
    /// 担当医ID
    /// </summary>
    public List<int> TantoIds { get; set; }

    /// <summary>
    /// 保険種別
    ///     1:社保 2:国保 3:後期
    ///     10:労災 11:自賠 12:自費レセ
    /// </summary>
    public List<int> HokenSbts { get; set; }

    /// <summary>
    /// レセプト識別
    /// </summary>
    public List<string> SinIds { get; set; }

    /// <summary>
    /// 項目種別
    /// </summary>
    public List<string> SinKouiKbns { get; set; }

    /// <summary>
    /// 院内院外区分
    /// </summary>
    public List<int> InoutKbns { get; set; }

    /// <summary>
    /// 毒薬区分
    /// </summary>
    public List<int> MadokuKbns { get; set; }

    /// <summary>
    /// 向精神薬区分
    /// </summary>
    public List<int> KouseisinKbns { get; set; }

    /// <summary>
    /// 後発医薬品区分
    /// </summary>
    public List<int> KohatuKbns { get; set; }

    /// <summary>
    /// 採用区分
    /// </summary>
    public List<int> IsAdopteds { get; set; }

    /// <summary>
    /// 検索項目
    /// </summary>
    public List<string> ItemCds { get; set; }

    /// <summary>
    /// 検索項目の検索オプション
    ///     0:or 1:and
    /// </summary>
    public int ItemSearchOpt { get; set; }

    /// <summary>
    /// 検索ワード
    /// </summary>
    public string SearchWord { get; set; }

    /// <summary>
    /// 検索ワードの検索オプション
    ///     0:or 1:and
    /// </summary>
    public int SearchOpt { get; set; }
}
