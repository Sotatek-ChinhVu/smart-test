using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta3020.Models
{
    public class CoSta3020PrintData
    {
        public CoSta3020PrintData(RowType rowType = RowType.Data)
        {
            RowType = rowType;
        }

        /// <summary>
        /// 行タイプ
        /// </summary>
        public RowType RowType { get; set; }

        /// <summary>
        /// セット区分
        /// </summary>
        public int SetKbn { get; set; }

        /// <summary>
        /// セット区分名称
        /// </summary>
        public string SetKbnName { get; set; } = string.Empty;

        /// <summary>
        /// 階層１
        /// </summary>
        public string Level1 { get; set; } = string.Empty;

        /// <summary>
        /// 階層２
        /// </summary>
        public string Level2 { get; set; } = string.Empty;

        /// <summary>
        /// 階層３
        /// </summary>
        public string Level3 { get; set; } = string.Empty;

        /// <summary>
        /// 階層４
        /// </summary>
        public string Level4 { get; set; } = string.Empty;

        /// <summary>
        /// 階層５
        /// </summary>
        public string Level5 { get; set; } = string.Empty;

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
        public string IsTitle { get; set; } = string.Empty;

        /// <summary>
        /// 選択方式
        /// </summary>
        public int SelectType { get; set; }

        /// <summary>
        /// 選択方式名称
        /// </summary>
        public string SelectTypeName
        {
            get
            {
                switch (SelectType)
                {
                    case 0: return "選択不可";
                    case 1: return "選択可";
                    case 2: return "親も選択";
                    case 3: return "子も選択";
                    case 4: return "親子両方";
                }
                return "";
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public string Suryo { get; set; } = string.Empty;

        /// <summary>
        /// 単位
        /// </summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 検査項目コード
        /// </summary>
        public string KensaItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 外注検査項目コード
        /// </summary>
        public string CenterItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 有効期限
        /// </summary>
        public int EndDate { get; set; }

        /// <summary>
        /// 有効期限 (YYYY(GEE)/MM/dd)
        /// </summary>
        public string EndDateFmt
        {
            get => CIUtil.SDateToShowSWDate(EndDate, 0);
        }

        /// <summary>
        /// 期限切れ
        /// </summary>
        public string Expired { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// </summary>
        public int RenNo { get; set; }


    }
}
