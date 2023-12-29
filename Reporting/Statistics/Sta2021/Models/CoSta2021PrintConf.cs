namespace Reporting.Statistics.Sta2021.Models
{
    public class CoSta2021PrintConf
    {
        public CoSta2021PrintConf(int menuId)
        {
            MenuId = menuId;
        }

        public CoSta2021PrintConf()
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
        ///     -1:未指定
        /// </summary>
        public int StartSinYm { get; set; }

        /// <summary>
        /// 診療年月End
        ///     -1:未指定
        /// </summary>
        public int EndSinYm { get; set; }

        /// <summary>
        /// 対象データ
        ///     0:算定 1:オーダー
        /// </summary>
        public int DataKind { get; set; }

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
        /// ソート順１
        ///     1:項目種別 2:レセプト識別 3:項目名 4:金額 5:数量
        /// </summary>
        public int SortOrder1 { get; set; }

        /// <summary>
        /// ソート順１オプション
        ///     0:昇順 1:降順
        /// </summary>
        public int SortOpt1 { get; set; }

        /// <summary>
        /// ソート順２
        ///     1:項目種別 2:レセプト識別 3:項目名 4:金額 5:数量
        /// </summary>
        public int SortOrder2 { get; set; }

        /// <summary>
        /// ソート順２オプション
        ///     0:昇順 1:降順
        /// </summary>
        public int SortOpt2 { get; set; }

        /// <summary>
        /// ソート順３
        ///     1:項目種別 2:レセプト識別 3:項目名 4:金額 5:数量
        /// </summary>
        public int SortOrder3 { get; set; }

        /// <summary>
        /// ソート順３オプション
        ///     0:昇順 1:降順
        /// </summary>
        public int SortOpt3 { get; set; }

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
        /// レセプト識別
        /// </summary>
        public List<string> SinIds { get; set; } = new();

        /// <summary>
        /// 項目種別
        /// </summary>
        public List<string> SinKouiKbns { get; set; } = new();

        /// <summary>
        /// 院内院外区分
        /// </summary>
        public List<int> InoutKbns { get; set; } = new();

        /// <summary>
        /// 毒薬区分
        /// </summary>
        public List<int> MadokuKbns { get; set; } = new();

        /// <summary>
        /// 向精神薬区分
        /// </summary>
        public List<int> KouseisinKbns { get; set; } = new();

        /// <summary>
        /// 後発医薬品区分
        /// </summary>
        public List<int> KohatuKbns { get; set; } = new();

        /// <summary>
        /// 採用区分
        /// </summary>
        public List<int> IsAdopteds { get; set; } = new();

        /// <summary>
        /// 検索項目
        /// </summary>
        public List<string> ItemCds { get; set; } = new();

        /// <summary>
        /// 検索項目の検索オプション
        ///     0:or 1:and
        /// </summary>
        public int ItemSearchOpt { get; set; }

        /// <summary>
        /// 検索ワード
        /// </summary>
        public string SearchWord { get; set; }

        /// <summary>
        /// 検索ワードの検索オプション
        ///     0:or 1:and
        /// </summary>
        public int SearchOpt { get; set; }
    }
}
