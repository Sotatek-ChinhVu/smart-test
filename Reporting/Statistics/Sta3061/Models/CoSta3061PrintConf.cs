namespace Reporting.Statistics.Sta3061.Models;

public class CoSta3061PrintConf
{
    public CoSta3061PrintConf(int menuId)
    {
        MenuId = menuId;
    }

    public CoSta3061PrintConf()
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
    ///     印刷期間指定
    ///         True: 日単位 False: 月単位
    /// </summary>
    public bool IsSinDate
    {
        get => StartSinDate > 0 || EndSinDate > 0;
    }

    /// <summary>
    /// 集計区分
    ///     0:診療日別 1:診療年月別 2:診療科別 3:担当医別 4:保険種別 5:年齢区分別 6:性別
    ///     PT_GRP_INF.GRP_ID + 10:グループ別
    /// </summary>
    public int ReportKbn { get; set; }

    /// <summary>
    /// 患者グループID
    /// </summary>
    public int PtGrpId
    {
        get => ReportKbn > 10 ? ReportKbn - 10 : 0;
    }

    /// <summary>
    /// 改ページ条件１
    ///     1:診療年月(診療日) 2:診療科 3:担当医
    /// </summary>
    public int PageBreak1 { get; set; }

    /// <summary>
    /// 改ページ条件２
    ///     1:診療年月(診療日) 2:診療科 3:担当医
    /// </summary>
    public int PageBreak2 { get; set; }

    /// <summary>
    /// 改ページ条件３
    ///     1:診療年月(診療日) 2:診療科 3:担当医
    /// </summary>
    public int PageBreak3 { get; set; }

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
        public int GrpId { get; set; }
        public string GrpCode { get; set; }

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
