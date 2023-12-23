using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta2021.Models
{
    public class CoSta2021PrintData
    {
        public CoSta2021PrintData(RowType rowType = RowType.Data)
        {
            RowType = rowType;
        }

        /// <summary>
        /// 行タイプ
        /// </summary>
        public RowType RowType { get; set; }

        /// <summary>
        /// 合計行のキャプション
        /// </summary>
        public string TotalCaption { get; set; } = string.Empty;

        /// <summary>
        /// 診療年月
        /// </summary>
        public List<int> SinYm { get; set; } = new();

        /// <summary>
        /// 診療年月 (yyyy/MM)
        /// </summary>
        public List<string> SinYmS { get; set; } = new();

        /// <summary>
        /// 診療科ID
        /// </summary>
        public string KaId { get; set; } = string.Empty;

        /// <summary>
        /// 診療科略称
        /// </summary>
        public string KaSname { get; set; } = string.Empty;

        /// <summary>
        /// 担当医ID
        /// </summary>
        public string TantoId { get; set; } = string.Empty;

        /// <summary>
        /// 担当医略称
        /// </summary>
        public string TantoSname { get; set; } = string.Empty;

        /// <summary>
        /// 診療識別
        /// </summary>
        public string SinId { get; set; } = string.Empty;

        /// <summary>
        /// 診療行為区分
        /// </summary>
        public string SinKouiKbn { get; set; } = string.Empty;

        /// <summary>
        /// 診療行為区分名称
        /// </summary>
        public string SinKouiKbnName
        {
            get
            {
                switch (SinKouiKbn)
                {
                    case "11": return "初診";
                    case "12": return "再診";
                    case "13": return "医学管理";
                    case "14": return "在宅";
                    case "20": return "投薬";
                    case "21": return "内用薬";
                    case "23": return "外用薬";
                    case "2x": return "他薬";
                    case "25": return "処方料";
                    case "26": return "麻毒加算";
                    case "27": return "調基";
                    case "28": return "自己注射";
                    case "30": return "注射薬";
                    case "31": return "皮下筋";
                    case "32": return "静脈内";
                    case "33": return "点滴";
                    case "34": return "他注";
                    case "40": return "処置";
                    case "50": return "手術";
                    case "54": return "麻酔";
                    case "60": return "検査";
                    case "61": return "検体検査";
                    case "62": return "生体検査";
                    case "64": return "病理診断";
                    case "70": return "画像診断";
                    case "77": return "フィルム";
                    case "80": return "その他";
                    case "81": return "リハビリ";
                    case "82": return "精神";
                    case "83": return "処方箋料";
                    case "84": return "放射線";
                    case "96": return "保険外";
                    case "99": return "コメント";
                    case "T": return "特材";
                }
                return "";
            }
        }

        /// <summary>
        /// 診療行為コード
        /// </summary>
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 診療行為名称
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 単価
        /// </summary>
        public string Ten { get; set; } = string.Empty;

        /// <summary>
        /// 単価(単位)
        /// </summary>
        public string TenUnit { get; set; } = string.Empty;

        /// <summary>
        /// 数量
        /// </summary>
        public string Suryo { get; set; } = string.Empty;

        /// <summary>
        /// 単位名称
        /// </summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 回数
        /// </summary>
        public List<string> Counts { get; set; } = new();

        /// <summary>
        /// 金額
        /// </summary>
        public List<string> Moneys { get; set; } = new();

        /// <summary>
        /// 院内院外区分
        /// </summary>
        public int InoutKbn { get; set; }

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
        /// 後発医薬品区分
        /// </summary>
        public int KohatuKbn { get; set; }

        /// <summary>
        /// 後発医薬品区分名称
        /// </summary>
        public string KohatuKbnName
        {
            get
            {
                if (new string[] { "21", "23", "2x", "30" }.Contains(SinKouiKbn))
                {
                    switch (KohatuKbn)
                    {
                        case 0: return "先発品(後発なし)";
                        case 1: return "後発品";
                        case 2: return "先発品(後発あり)";
                    }
                }
                return "";
            }
        }

        /// <summary>
        /// 採用区分
        /// </summary>
        public int IsAdopted { get; set; }
    }
}
