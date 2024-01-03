using Helper.Common;

namespace Reporting.Statistics.Sta3020.Models
{
    public class CoSta3020PrintConf
    {
        public CoSta3020PrintConf(int menuId)
        {
            MenuId = menuId;
            StdDate = CIUtil.DateTimeToInt(DateTime.Today);
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
        ///     1: セット区分
        /// </summary>
        public int PageBreak1 { get; set; }

        /// <summary>
        /// セット区分-医学管理
        /// </summary>
        public bool SetKbnKanri { get; set; }

        /// <summary>
        /// セット区分-在宅
        /// </summary>
        public bool SetKbnZaitaku { get; set; }

        /// <summary>
        /// セット区分-処方
        /// </summary>
        public bool SetKbnSyoho { get; set; }

        /// <summary>
        /// セット区分-用法
        /// </summary>
        public bool SetKbnYoho { get; set; }

        /// <summary>
        /// セット区分-注射手技
        /// </summary>
        public bool SetKbnChusyaSyugi { get; set; }

        /// <summary>
        /// セット区分-注射
        /// </summary>
        public bool SetKbnChusya { get; set; }

        /// <summary>
        /// セット区分-処置
        /// </summary>
        public bool SetKbnSyochi { get; set; }

        /// <summary>
        /// セット区分-検査
        /// </summary>
        public bool SetKbnKensa { get; set; }

        /// <summary>
        /// セット区分-手術
        /// </summary>
        public bool SetKbnSyujutsu { get; set; }

        /// <summary>
        /// セット区分-画像
        /// </summary>
        public bool SetKbnGazo { get; set; }

        /// <summary>
        /// セット区分-その他
        /// </summary>
        public bool SetKbnSonota { get; set; }

        /// <summary>
        /// セット区分-自費
        /// </summary>
        public bool SetKbnJihi { get; set; }

        /// <summary>
        /// セット区分-病名
        /// </summary>
        public bool SetKbnByomei { get; set; }

        /// <summary>
        /// 対象データ
        ///     0:すべて
        ///     1: 期限切れ項目
        ///     2: 項目選択
        /// </summary>
        public int TgtData { get; set; }

        /// <summary>
        /// 検索ワード
        /// </summary>
        public string SearchWord { get; set; }

        /// <summary>
        /// 検索ワードリスト
        /// </summary>
        public List<string> ListSearchWord
        {
            get
            {
                List<string> SearchWords = new List<string>();
                if (SearchWord != null)
                {
                    //スペース区切りでキーワードを分解
                    string[] values = SearchWord.Replace("　", " ").Split(' ');
                    SearchWords.AddRange(values);
                }
                return SearchWords;
            }
        }

        /// <summary>
        /// 検索ワードの検索オプション
        ///     0:or 
        ///     1:and
        /// </summary>
        public int SearchOpt { get; set; }

        /// <summary>
        /// 検索項目の検索オプション
        ///     0:項目 
        ///     1:病名
        /// </summary>
        public int ItemSearchOpt { get; set; }

        /// <summary>
        /// 検索項目
        /// </summary>
        public List<string> ItemCds { get; set; }
    }
}
