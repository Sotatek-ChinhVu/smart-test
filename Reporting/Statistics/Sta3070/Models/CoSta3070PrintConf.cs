using Helper.Common;

namespace Reporting.Statistics.Sta3070.Models;

public class CoSta3070PrintConf
{
    public CoSta3070PrintConf(int menuId)
    {
        MenuId = menuId;
        RangeFrom = -1;
        RangeTo = -1;
    }

    public CoSta3070PrintConf()
    {
        RangeFrom = -1;
        RangeTo = -1;
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
    /// 改ページ１
    ///     0:未指定 1:診療年月 2:診療科 3:担当医
    /// </summary>
    public int PgBreak1 { get; set; }

    /// <summary>
    /// 改ページ２
    ///     0:未指定 1:診療年月 2:診療科 3:担当医
    /// </summary>
    public int PgBreak2 { get; set; }

    /// <summary>
    /// 改ページ３
    ///     0:未指定 1:診療年月 2:診療科 3:担当医
    /// </summary>
    public int PgBreak3 { get; set; }

    /// <summary>
    /// テスト患者の有無
    /// </summary>
    public bool IsTester { get; set; }

    /// <summary>
    /// 期間From
    /// </summary>
    ///     -1:未指定
    public int RangeFrom { get; set; }

    /// <summary>
    /// 期間To
    ///     -1:未指定
    /// </summary>
    public int RangeTo { get; set; }

    /// <summary>
    /// 集計区分
    ///     0:診療年月別 1:診療年別 2:診療科別 3:担当医別 4:保険種別 5:年齢区分別 
    /// </summary>
    public int ReportKbn { get; set; }

    /// <summary>
    /// 診療日From
    /// </summary>
    public int StartSinYmd
    {
        get => RangeFrom.ToString().Length == 6 ? RangeFrom * 100 + 1 :
                   RangeFrom.ToString().Length == 4 ? RangeFrom * 10000 + 101 :
                   RangeFrom.ToString().Length == 8 ? RangeFrom :
                   -1;
    }

    /// <summary>
    /// 診療日To
    /// </summary>
    public int EndSinYmd
    {
        get => RangeTo.ToString().Length == 6 ? CIUtil.GetLastDateOfMonth(RangeTo * 100 + 1) :
                   RangeTo.ToString().Length == 4 ? RangeTo * 10000 + 1231 :
                   RangeTo.ToString().Length == 8 ? RangeTo :
                   -1;
    }

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
    ///     0:自費 1:社保 2:国保 3:後期
    ///     10:労災 11:自賠 12:自費レセ
    /// </summary>
    public List<int> HokenSbts { get; set; } = new();


}
