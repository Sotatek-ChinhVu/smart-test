namespace Reporting.Statistics.Sta2011.Models
{
    public class CoSta2011PrintConf
    {
        public CoSta2011PrintConf(int menuId)
        {
            MenuId = menuId;
        }

        public CoSta2011PrintConf()
        {
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
        /// 診療科ID
        /// </summary>
        public List<int> KaIds { get; set; }

        /// <summary>
        /// 担当医ID
        /// </summary>
        public List<int> TantoIds { get; set; }

        /// <summary>
        /// 対象レセプト
        ///     0:通常レセプト
        ///     1:月遅れレセプト
        ///     2:返戻レセプト
        ///     3:オンライン返戻
        ///     9:紙請求レセプト
        /// </summary>
        public List<int> SeikyuTypes { get; set; }

        /// <summary>
        /// 在宅患者を別枠に集計する
        /// </summary>
        public bool IsZaitaku { get; set; }

        /// <summary>
        /// 在宅患者扱いするオーダー項目
        /// </summary>
        public List<string> ZaitakuItems { get; set; }

        /// <summary>
        /// 内訳を表示する
        /// </summary>
        public bool IsUchiwake { get; set; }
    }
}
