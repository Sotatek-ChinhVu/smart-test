using Helper.Common;

namespace Reporting.Statistics.Sta3041.Models;

public class CoSta3041PrintConf
{
    public CoSta3041PrintConf(int menuId)
    {
        MenuId = menuId;
        FromYm = CIUtil.DateTimeToInt(DateTime.Today.AddMonths(-3)) / 100;
        ToYm = CIUtil.DateTimeToInt(DateTime.Today.AddMonths(-1)) / 100;
    }

    public CoSta3041PrintConf()
    {
        MenuId = 0;
        FromYm = CIUtil.DateTimeToInt(DateTime.Today.AddMonths(-3)) / 100;
        ToYm = CIUtil.DateTimeToInt(DateTime.Today.AddMonths(-1)) / 100;
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
    /// 期間From(YYYYMM)
    /// </summary>
    public int FromYm { get; set; }

    /// <summary>
    /// 期間To(YYYYMM)
    /// </summary>
    public int ToYm { get; set; }

    /// <summary>
    /// テスト患者の有無
    /// </summary>
    public bool IsTester { get; set; }
}
