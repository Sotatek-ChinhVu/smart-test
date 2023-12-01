namespace Reporting.Statistics.Sta2010.Models
{
    public class CoSta2010PrintConf
    {
        public CoSta2010PrintConf(int menuId)
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
        public string FormFileName { get; set; }=string.Empty;

        /// <summary>
        /// 帳票タイトル
        /// </summary>
        public string ReportName { get; set; } = string.Empty;

        /// <summary>
        /// テスト患者の有無
        /// </summary>
        public bool IsTester { get; set; }

        /// <summary>
        /// 請求年月
        /// </summary>
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 改ページ条件１
        ///     1:在医総 2:診療科 3:担当医
        /// </summary>
        public int PageBreak1 { get; set; }

        /// <summary>
        /// 改ページ条件２
        ///     1:在医総 2:診療科 3:担当医
        /// </summary>
        public int PageBreak2 { get; set; }

        /// <summary>
        /// 改ページ条件３
        ///     1:在医総 2:診療科 3:担当医
        /// </summary>
        public int PageBreak3 { get; set; }

        /// <summary>
        /// 国保保険者番号の政令指定都市まとめ有無
        /// </summary>
        public bool MainHokensyaNo { get; set; }

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
        ///     1:社保 2:国保 3:後期
        ///     10:労災 11:自賠 12:自費レセ
        /// </summary>
        public List<int> HokenSbts { get; set; } = new();

        /// <summary>
        /// 対象レセプト
        ///     0:通常レセプト
        ///     1:月遅れレセプト
        ///     2:返戻レセプト
        ///     3:オンライン返戻
        ///     9:紙請求レセプト
        /// </summary>
        public List<int> SeikyuTypes { get; set; } = new();
    }
}
