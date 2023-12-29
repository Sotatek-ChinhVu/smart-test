using Helper.Common;
using Reporting.Statistics.Enums;

namespace Reporting.Statistics.Sta2010.Models
{
    public class CoSta2010PrintData
    {
        public CoSta2010PrintData(RowType rowType = RowType.Data)
        {
            RowType = rowType;
        }

        /// <summary>
        /// 行タイプ
        /// </summary>
        public RowType RowType { get; set; }

        /// <summary>
        /// 区切り線
        /// </summary>
        public bool DrawLine { get; set; }

        /// <summary>
        /// 請求年月
        /// </summary>
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 請求年月 (yyyy/MM)
        /// </summary>
        public string SeikyuYmFmt
        {
            get => CIUtil.SMonthToShowSMonth(SeikyuYm);
        }

        /// <summary>
        /// 在医総
        /// </summary>
        public string IsZaiiso { get; set; }

        /// <summary>
        /// 診療科ID
        /// </summary>
        public string KaId { get; set; }

        /// <summary>
        /// 診療科略称
        /// </summary>
        public string KaSname { get; set; }

        /// <summary>
        /// 担当医ID
        /// </summary>
        public string TantoId { get; set; }

        /// <summary>
        /// 担当医略称
        /// </summary>
        public string TantoSname { get; set; }

        /// <summary>
        /// 保険種別（階層１）
        /// </summary>
        public string HokenSbt1 { get; set; }

        /// <summary>
        /// 保険種別（階層２）
        /// </summary>
        public string HokenSbt2 { get; set; }

        /// <summary>
        /// 保険種別（階層３）
        /// </summary>
        public string HokenSbt3 { get; set; }

        /// <summary>
        /// 保険種別（階層４）
        /// </summary>
        public string HokenSbt4 { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// 実日数
        /// </summary>
        public string Nissu { get; set; }

        /// <summary>
        /// 点数
        /// </summary>
        public string Tensu { get; set; }

        /// <summary>
        /// 一部負担金
        /// </summary>
        public string Futan { get; set; }

        /// <summary>
        /// 件数(公費併用分)
        /// </summary>
        public string KohiCount { get; set; }

        /// <summary>
        /// 実日数(公費併用分)
        /// </summary>
        public string KohiNissu { get; set; }

        /// <summary>
        /// 点数(公費併用分)
        /// </summary>
        public string KohiTensu { get; set; }

        /// <summary>
        /// 一部負担金(公費併用分)
        /// </summary>
        public string KohiFutan { get; set; }

        /// <summary>
        /// 窓口負担
        /// </summary>
        public string PtFutan { get; set; }

        /// <summary>
        /// 振込予定額
        /// </summary>
        public string Furikomi { get; set; }
    }
}
