namespace Reporting.Statistics.Sta1010.Models;

public class CoSta1010PrintConf
{
    public CoSta1010PrintConf(int menuId)
    {
        MenuId = menuId;
    }

    public CoSta1010PrintConf()
    {
        MenuId = 0;
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
    /// 診療日From
    /// </summary>
    public int StartSinDate { get; set; }

    /// <summary>
    /// 診療日To
    /// </summary>
    public int EndSinDate { get; set; }

    /// <summary>
    /// 改ページ条件１
    ///     1:診療科 2:担当医
    /// </summary>
    public int PageBreak1 { get; set; }

    /// <summary>
    /// 改ページ条件２
    ///     1:診療科 2:担当医
    /// </summary>
    public int PageBreak2 { get; set; }

    /// <summary>
    /// ソート順１
    ///     1:氏名 2:患者番号 3:未収額 4:最終来院日 5:診療日
    /// </summary>
    public int SortOrder1 { get; set; }

    /// <summary>
    /// ソート順１オプション
    ///     0:昇順 1:降順
    /// </summary>
    public int SortOpt1 { get; set; }

    /// <summary>
    /// ソート順２
    ///     1:氏名 2:患者番号 3:未収額 4:最終来院日 5:診療日
    /// </summary>
    public int SortOrder2 { get; set; }

    /// <summary>
    /// ソート順２オプション
    ///     0:昇順 1:降順
    /// </summary>
    public int SortOpt2 { get; set; }

    /// <summary>
    /// ソート順３
    ///     1:氏名 2:患者番号 3:未収額 4:最終来院日 5:診療日
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
    /// 請求金額(新) を請求金額にする
    /// </summary>
    public bool IsNewSeikyu { get; set; }

    /// <summary>
    /// 請求金額に変更がある患者のみ
    /// </summary>
    public bool IsDiffSeikyu { get; set; }

    /// <summary>
    /// 通常未収を未収扱いにする
    /// </summary>
    public bool IncludeMisyu { get; set; }

    /// <summary>
    /// 免除額を未収扱いにする
    /// </summary>
    public bool IncludeMenjyo { get; set; }

    /// <summary>
    /// 調整額を未収扱いにする
    /// </summary>
    public bool IncludeAdjustFutan { get; set; }

    /// <summary>
    /// 未収区分
    ///     1:通常未収 2:免除額 3:調整額
    /// </summary>
    public List<int> MisyuKbns { get; set; }

    /// <summary>
    /// 期間外に入金がある場合は未収としない
    /// </summary>
    public bool IncludeOutRangeNyukin { get; set; }

    /// <summary>
    /// 未精算の来院を未収とする
    /// </summary>
    public bool IncludeUnpaid { get; set; }

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
}
