namespace Reporting.Statistics.Sta3001.Models
{
    public class CoAdpDrugsModel
    {
        /// <summary>
        /// コード
        /// </summary>
        public string ItemCd { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// カナ１
        /// </summary>
        public string KanaName1 { get; set; }

        /// <summary>
        /// カナ２
        /// </summary>
        public string KanaName2 { get; set; }

        /// <summary>
        /// カナ３
        /// </summary>
        public string KanaName3 { get; set; }

        /// <summary>
        /// カナ４
        /// </summary>
        public string KanaName4 { get; set; }

        /// <summary>
        /// カナ５
        /// </summary>
        public string KanaName5 { get; set; }

        /// <summary>
        /// カナ６
        /// </summary>
        public string KanaName6 { get; set; }

        /// <summary>
        /// カナ７
        /// </summary>
        public string KanaName7 { get; set; }

        /// <summary>
        /// 既定数量
        /// </summary>
        public double DefaultVal { get; set; }

        /// <summary>
        /// 基本単位
        /// </summary>
        public string OdrUnitName { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// レセ単位
        /// </summary>
        public string ReceUnitName { get; set; }

        /// <summary>
        /// 開始日
        /// </summary>
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日
        /// </summary>
        public int EndDate { get; set; }
        /// <summary>
        /// 薬剤区分
        /// </summary>
        public int DrugKbn { get; set; }

        /// <summary>
        /// 麻毒区分
        /// </summary>
        public int MadokuKbn { get; set; }

        /// <summary>
        /// 向精神薬区分
        /// </summary>
        public int KouseisinKbn { get; set; }

        /// <summary>
        /// 後発区分
        /// </summary>
        public int KohatuKbn { get; set; }

        /// <summary>
        /// 一般名
        /// </summary>
        public string IpnName { get; set; }

        /// <summary>
        /// レセ名称
        /// </summary>
        public string ReceName { get; set; }

        /// <summary>
        /// YJコード
        /// </summary>
        public string YjCd { get; set; }
    }
}
