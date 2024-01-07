namespace Reporting.Statistics.Sta3001.Models
{
    public class CoSta3001PrintConf
    {
        public CoSta3001PrintConf(int menuId)
        {
            MenuId = menuId;
            StartDateFrom = -1;
            StartDateTo = -1;
            EndDateFrom = -1;
            EndDateTo = -1;
        }

        /// <summary>
        /// STA_MENU.MENU_ID
        /// </summary>
        public int MenuId { get; }

        /// <summary>
        /// 帳票タイトル
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// フォームファイル名
        /// </summary>
        public string FormFileName { get; set; }

        /// <summary>
        /// 基準日
        /// </summary>
        public int StdDate { get; set; }

        /// <summary>
        /// 改ページ１
        ///     1: 薬剤区分
        /// </summary>
        public int PageBreak1 { get; set; }

        /// <summary>
        /// ソート順１
        ///     1:薬剤区分 2:名称 3:項目コード
        /// </summary>
        public int SortOrder1 { get; set; }

        /// <summary>
        /// ソート順１オプション
        ///     0:昇順 1:降順
        /// </summary>
        public int SortOpt1 { get; set; }

        /// <summary>
        /// ソート順２
        ///     1:薬剤区分 2:名称 3:項目コード
        /// </summary>
        public int SortOrder2 { get; set; }

        /// <summary>
        /// ソート順２オプション
        ///     0:昇順 1:降順
        /// </summary>
        public int SortOpt2 { get; set; }

        /// <summary>
        /// ソート順３
        ///     1:薬剤区分 2:名称 3:項目コード
        /// </summary>
        public int SortOrder3 { get; set; }

        /// <summary>
        /// ソート順３オプション
        ///     0:昇順 1:降順
        /// </summary>
        public int SortOpt3 { get; set; }

        /// <summary>
        /// 薬剤区分
        /// </summary>
        public List<int> DrugKbns { get; set; }

        /// <summary>
        /// 麻毒区分
        /// </summary>
        public List<int> MadokuKbns { get; set; }

        /// <summary>
        /// 向精神薬区分
        /// </summary>
        public List<int> KouseisinKbns { get; set; }

        /// <summary>
        /// 後発フラグ
        /// </summary>
        public List<int> KohatuKbns { get; set; }

        /// <summary>
        /// 開始日From
        ///     -1:未指定
        /// </summary>
        public int StartDateFrom { get; set; }

        /// <summary>
        /// 開始日To
        ///     -1:未指定
        /// </summary>
        public int StartDateTo { get; set; }

        /// <summary>
        /// 終了日From
        ///     -1:未指定
        /// </summary>
        public int EndDateFrom { get; set; }

        /// <summary>
        /// 終了日To
        ///     -1:未指定
        /// </summary>
        public int EndDateTo { get; set; }

        /// <summary>
        /// 一般名オプション
        ///     0:印字しない 1:印字する
        /// </summary>
        public int IpnNameOpt { get; set; }

        /// <summary>
        /// レセプト名称オプション
        ///     0:印字しない 1:印字する
        /// </summary>
        public int ReceNameOpt { get; set; }

    }
}
