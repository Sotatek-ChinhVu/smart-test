using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3001.Models
{
    public class CoSta3001PrintData
    {
        public CoSta3001PrintData(RowType rowType = RowType.Data)
        {
            RowType = rowType;
            KohatuKbn = -1;
        }

        /// <summary>
        /// 行タイプ
        /// </summary>
        public RowType RowType { get; set; }

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
        public string Suryo { get; set; }

        /// <summary>
        /// 基本単位
        /// </summary>
        public string OdrUnitName { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// レセ単位
        /// </summary>
        public string ReceUnitName { get; set; }

        /// <summary>
        /// レセ単位（/レセ単位）
        /// </summary>
        public string ReceUnitNameFmt
        {
            get => string.Format("/{0}", ReceUnitName) == "/" ? "" : string.Format("/{0}", ReceUnitName);
        }

        /// <summary>
        /// 開始日
        /// </summary>
        public int StartDate { get; set; }

        /// <summary>
        /// 開始日 (YYYY(GYY)/MM/dd)
        /// </summary>
        public string StartDateFmt
        {
            get => CIUtil.SDateToShowSWDate(StartDate, 0, 1);
        }

        /// <summary>
        /// 終了日
        /// </summary>
        public int EndDate { get; set; }

        /// <summary>
        /// 終了日 (YYYY(GYY)/MM/dd)
        /// </summary>
        public string EndDateFmt
        {
            get => CIUtil.SDateToShowSWDate(EndDate, 0, 1);
        }

        /// <summary>
        /// 薬剤区分
        /// </summary>
        public int DrugKbn { get; set; }

        /// <summary>
        /// 薬剤区分略称
        /// </summary>
        public string DrugKbnSname
        {
            get
            {
                switch (DrugKbn)
                {
                    case 1: return "内";
                    case 3: return "他";
                    case 4: return "注";
                    case 6: return "外";
                    case 8: return "歯";
                }
                return "";
            }
        }

        /// <summary>
        /// 薬剤区分名称
        /// </summary>
        public string DrugKbnName
        {
            get
            {
                switch (DrugKbn)
                {
                    case 1: return "内用薬";
                    case 3: return "その他";
                    case 4: return "注射薬";
                    case 6: return "外用薬";
                    case 8: return "歯科用薬剤";
                }
                return "";
            }
        }

        /// <summary>
        /// 麻毒区分
        /// </summary>
        public int MadokuKbn { get; set; }

        /// <summary>
        /// 麻毒区分略称
        /// </summary>
        public string MadokuKbnSname
        {
            get
            {
                switch (MadokuKbn)
                {
                    case 1: return "麻";
                    case 2: return "毒";
                    case 3: return "覚";
                    case 5: return "向";
                }
                return "";
            }
        }

        /// <summary>
        /// 麻毒区分名称
        /// </summary>
        public string MadokuKbnName
        {
            get
            {
                switch (MadokuKbn)
                {
                    case 1: return "麻薬";
                    case 2: return "毒薬";
                    case 3: return "覚せい剤原料";
                    case 5: return "向精神薬";
                }
                return "";
            }
        }

        /// <summary>
        /// 向精神薬区分
        /// </summary>
        public int KouseisinKbn { get; set; }

        /// <summary>
        /// 向精神薬区分略称
        /// </summary>
        public string KouseisinKbnSname
        {
            get
            {
                switch (KouseisinKbn)
                {
                    case 1: return "不";
                    case 2: return "睡";
                    case 3: return "う";
                    case 4: return "精";
                }
                return "";
            }
        }

        /// <summary>
        /// 向精神薬区分名称
        /// </summary>
        public string KouseisinKbnName
        {
            get
            {
                switch (KouseisinKbn)
                {
                    case 1: return "抗不安薬";
                    case 2: return "睡眠薬";
                    case 3: return "抗うつ薬";
                    case 4: return "抗精神病薬";
                }
                return "";
            }
        }

        /// <summary>
        /// 後発区分
        /// </summary>
        public int KohatuKbn { get; set; }

        /// <summary>
        /// 後発名称
        /// </summary>
        public string KohatuKbnName
        {
            get
            {
                switch (KohatuKbn)
                {
                    case 0: return "先無";
                    case 1: return "後発";
                    case 2: return "先有";
                }
                return "";
            }
        }

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
