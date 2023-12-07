namespace Reporting.Statistics.Sta2003.Models;

public class CoSta2003PrintConf
{
    public CoSta2003PrintConf(int menuId)
    {
        MenuId = menuId;
    }

    public CoSta2003PrintConf()
    {
    }

    /// <summary>
    /// STA_MENU.MENU_ID
    /// </summary>
    public int MenuId { get; }

    /// <summary>
    /// フォームファイル名
    /// </summary>
    public string FormFileName { get; set; } = string.Empty;

    /// <summary>
    /// 帳票タイトル
    /// </summary>
    public string ReportName { get; set; } = string.Empty;

    /// <summary>
    /// テスト患者の有無
    /// </summary>
    public bool IsTester { get; set; }

    /// <summary>
    /// 入金月From
    /// </summary>
    public int StartNyukinYm { get; set; }

    /// <summary>
    /// 入金月To
    /// </summary>
    public int EndNyukinYm { get; set; }

    /// <summary>
    /// 未精算の来院
    ///     true:含まない false:含む
    /// </summary>
    public bool IsExcludeUnpaid { get; set; }

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
    ///     1:患者番号 2:氏名
    /// </summary>
    public int SortOrder1 { get; set; }

    /// <summary>
    /// ソート順１オプション
    ///     0:昇順 1:降順
    /// </summary>
    public int SortOpt1 { get; set; }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public List<int> KaIds { get; set; } = new();

    /// <summary>
    /// 担当医ID
    /// </summary>
    public List<int> TantoIds { get; set; } = new();

    /// <summary>
    /// 保険種別
    ///     0:すべて
    ///     1:社保
    ///     2:公費
    ///     3:国保
    ///     4:退職
    ///     5:後期
    ///     10:労災
    ///     11:自賠
    ///     12:自費
    ///     13:自レ
    /// </summary>
    public List<int> HokenSbts { get; set; } = new();

    /// <summary>
    /// 診療点数
    ///     0:すべて 1:"=0" 2:"!=0"
    /// </summary>
    public int IsTensu { get; set; }

    /// <summary>
    /// 保険外金額
    ///     0:すべて 1:"=0" 2:"!=0"
    /// </summary>
    public int IsJihiFutan { get; set; }
}
