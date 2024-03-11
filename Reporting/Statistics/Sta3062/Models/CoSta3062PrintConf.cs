namespace Reporting.Statistics.Sta3062.Models;

public class CoSta3062PrintConf
{
    public CoSta3062PrintConf(int menuId)
    {
        MenuId = menuId;
    }

    public CoSta3062PrintConf()
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
    /// 診療年月Start
    /// </summary>
    public int StartSinYm { get; set; }

    /// <summary>
    /// 診療年月End
    /// </summary>
    public int EndSinYm { get; set; }

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

    /// <summary>
    /// 患者分類
    /// </summary>
    public struct PtGrp
    {
        public int GrpId;
        public string GrpCode;

        public PtGrp(int grpId, string grpCode)
        {
            GrpId = grpId;
            GrpCode = grpCode;
        }
    };

    /// <summary>
    /// 患者分類
    /// </summary>
    public List<PtGrp> PtGrps { get; set; } = new();
}
