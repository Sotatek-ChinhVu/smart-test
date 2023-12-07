using Helper.Common;

namespace Reporting.Statistics.Sta3040.Models;

public class CoSta3040PrintConf
{
    public CoSta3040PrintConf(int menuId)
    {
        MenuId = menuId;
        FromYm = CIUtil.DateTimeToInt(DateTime.Today) / 100;
        ToYm = FromYm;
    }

    public CoSta3040PrintConf()
    {
        MenuId = 0;
        FromYm = CIUtil.DateTimeToInt(DateTime.Today) / 100;
        ToYm = FromYm;
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
    /// ソート順１
    ///     1:名称 2:項目コード
    /// </summary>
    public int SortOrder1 { get; set; }

    /// <summary>
    /// ソート順１オプション
    ///     0:昇順 1:降順
    /// </summary>
    public int SortOpt1 { get; set; }

    /// <summary>
    /// ソート順２
    ///     1:名称 2:項目コード
    /// </summary>
    public int SortOrder2 { get; set; }

    /// <summary>
    /// ソート順２オプション
    ///     0:昇順 1:降順
    /// </summary>
    public int SortOpt2 { get; set; }

    /// <summary>
    /// テスト患者の有無
    /// </summary>
    public bool IsTester { get; set; }

    /// <summary>
    /// 診療識別
    ///     1:在宅 2:処方 3:注射 4:指導 5:処置 6:手術 7:検査 8:画像診断 9:その他
    /// </summary>
    public List<int> SinryoSbt { get; set; } = new();
}
