namespace Reporting.Statistics.Sta1002.Models;

public class CoSta1002PrintConf
{
    public CoSta1002PrintConf(int menuId)
    {
        MenuId = menuId;
        StartNyukinTime = -1;
        EndNyukinTime = -1;
    }

    public CoSta1002PrintConf()
    {
        StartNyukinTime = -1;
        EndNyukinTime = -1;
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
    /// 入金日From
    /// </summary>
    public int StartNyukinDate { get; set; }

    /// <summary>
    /// 入金日To
    /// </summary>
    public int EndNyukinDate { get; set; }

    /// <summary>
    /// 入金時間From(HHmm)
    ///     -1:未指定
    /// </summary>
    public int StartNyukinTime { get; set; }

    /// <summary>
    /// 入金時間To(HHmm)
    ///     -1:未指定
    /// </summary>
    public int EndNyukinTime { get; set; }

    /// <summary>
    /// 未精算の来院
    ///     true:含まない false:含む
    /// </summary>
    public bool IsExcludeUnpaid { get; set; }

    /// <summary>
    /// 改ページ条件１
    ///     1:受付種別 2:診療科 3:担当医
    /// </summary>
    public int PageBreak1 { get; set; }

    /// <summary>
    /// 改ページ条件２
    ///     1:受付種別 2:診療科 3:担当医
    /// </summary>
    public int PageBreak2 { get; set; }

    /// <summary>
    /// 改ページ条件３
    ///     1:受付種別 2:診療科 3:担当医
    /// </summary>
    public int PageBreak3 { get; set; }

    /// <summary>
    /// 受付種別ID
    /// </summary>
    public List<int> UketukeSbtIds { get; set; }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public List<int> KaIds { get; set; }

    /// <summary>
    /// 担当医ID
    /// </summary>
    public List<int> TantoIds { get; set; }

    /// <summary>
    /// 支払区分コード
    /// </summary>
    public List<int> PaymentMethodCds { get; set; }
}
