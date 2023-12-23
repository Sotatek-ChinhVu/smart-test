using Helper.Common;

namespace Reporting.Statistics.Sta3071.Models;

public class CoSta3071PrintConf
{
    public CoSta3071PrintConf(int menuId)
    {
        MenuId = menuId;
        RangeFrom = -1;
        RangeTo = -1;
    }

    public CoSta3071PrintConf()
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
    /// 診療日From
    /// </summary>
    public int StartSinYmd
    {
        get => RangeFrom.ToString().Length == 6 ?
               RangeFrom * 100 + 1 :
               RangeFrom;
    }

    /// <summary>
    /// 診療日To
    /// </summary>
    public int EndSinYmd
    {
        get => RangeTo.ToString().Length == 6 ?
               CIUtil.GetLastDateOfMonth(RangeTo * 100 + 1) :
               RangeTo;
    }

    /// <summary>
    /// 集計項目（縦）
    ///     1:診療科別 2:担当医別 3:保険種別 4:日別 5:月別 6:時間外等別 7:年齢区分別 8:性別 2X:患者グループX
    /// </summary>
    public int ReportKbnV { get; set; }

    /// <summary>
    /// 集計項目（横）
    ///     1:診療科別 2:担当医別 3:保険種別 4:日別 5:月別 6:時間外等別 7:年齢区分別 8:性別 2X:患者グループX
    /// </summary>
    public int ReportKbnH { get; set; }

    /// <summary>
    /// 診療科ID
    /// </summary>
    public List<int> KaIds { get; set; } = new();

    /// <summary>
    /// 担当医ID
    /// </summary>
    public List<int> TantoIds { get; set; } = new();

}
