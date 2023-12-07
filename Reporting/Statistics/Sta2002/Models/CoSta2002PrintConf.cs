namespace Reporting.Statistics.Sta2002.Models
{
    public class CoSta2002PrintConf
    {
        public CoSta2002PrintConf(int menuId)
        {
            MenuId = menuId;
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
        /// 診療科ID
        /// </summary>
        public List<int> KaIds { get; set; } = new();

        /// <summary>
        /// 担当医ID
        /// </summary>
        public List<int> TantoIds { get; set; } = new();
    }
}
