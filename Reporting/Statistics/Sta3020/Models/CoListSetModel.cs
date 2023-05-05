namespace Reporting.Statistics.Sta3020.Models
{
    public class CoListSetModel
    {
        /// <summary>
        /// セット区分コード
        /// </summary>
        public int SetKbn { get; set; }

        /// <summary>
        /// セット区分名称
        /// </summary>
        public string SetKbnName
        {
            get
            {
                switch (SetKbn)
                {
                    case 13: return "医学管理";
                    case 14: return "在宅";
                    case 20: return "処方";
                    case 21: return "用法";
                    case 30: return "注射";
                    case 31: return "注射手技";
                    case 40: return "処置";
                    case 50: return "手術";
                    case 60: return "検査";
                    case 70: return "画像";
                    case 80: return "その他";
                    case 95: return "自費";
                }
                return "病名";
            }
        }

        /// <summary>
        /// 階層１
        /// </summary>
        public int Level1 { get; set; }

        /// <summary>
        /// 階層２
        /// </summary>
        public int Level2 { get; set; }

        /// <summary>
        /// 階層３
        /// </summary>
        public int Level3 { get; set; }

        /// <summary>
        /// 階層４
        /// </summary>
        public int Level4 { get; set; }

        /// <summary>
        /// 階層５
        /// </summary>
        public int Level5 { get; set; }

        /// <summary>
        /// セットコード
        /// </summary>
        public int SetCd { get; set; }

        /// <summary>
        /// 診療行為コード
        /// </summary>
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 診療行為名称
        /// </summary>
        public string SetName { get; set; } = string.Empty;

        /// <summary>
        /// タイトル
        /// </summary>
        public int IsTitle { get; set; }

        /// <summary>
        /// 選択方式
        /// </summary>
        public int SelectType { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public double Suryo { get; set; }

        /// <summary>
        /// 単位
        /// </summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 検査項目コード
        /// </summary>
        public string KensaItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 外注検査項目コード１
        /// </summary>
        public string CenterItemCd1 { get; set; } = string.Empty;

        /// <summary>
        /// 外注検査項目コード２
        /// </summary>
        public string CenterItemCd2 { get; set; } = string.Empty;

        /// <summary>
        /// 外注検査項目コード
        /// </summary>
        public string CenterItemCd
        {
            get => CenterItemCd1 + CenterItemCd2;
        }

        /// <summary>
        /// 有効期限
        /// </summary>
        public int MaxEndDate { get; set; }

        /// <summary>
        /// 有効期限
        /// </summary>
        public int EndDate
        {
            get => MaxEndDate <= 0 ? 99999999 : MaxEndDate;
        }

    }
}
