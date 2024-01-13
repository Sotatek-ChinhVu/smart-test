using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "odr_inf_cmt")]
    public class OdrInfCmt : EmrCloneable<OdrInfCmt>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 診療日
        /// yyyymmdd
        /// </summary>
        [Column("sin_date")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        
        [Column("raiin_no", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 剤番号
        /// ODR_INF_DETAIL.RP_NO
        /// </summary>
        
        [Column("rp_no", Order = 3)]
        public long RpNo { get; set; }

        /// <summary>
        /// 剤枝番
        /// ODR_INF_DETAIL.RP_EDA_NO
        /// </summary>
        
        [Column("rp_eda_no", Order = 4)]
        public long RpEdaNo { get; set; }

        /// <summary>
        /// 行番号
        /// ODR_INF_DETAIL.ROW_NO
        /// </summary>
        
        [Column("row_no", Order = 5)]
        public int RowNo { get; set; }

        /// <summary>
        /// 枝番
        /// ※2018/11/29現在、1項目につき、最大3つまで
        /// </summary>
        
        [Column("eda_no", Order = 6)]
        public int EdaNo { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// 文字色
        /// 
        /// </summary>
        [Column("font_color")]
        public int FontColor { get; set; }

        /// <summary>
        /// コメントコード
        /// 当該診療行為に対するコメントコード
        /// </summary>
        [Column("cmt_cd")]
        [MaxLength(10)]
        public string? CmtCd { get; set; } = string.Empty;

        /// <summary>
        /// コメント名称
        /// コメントコードの名称
        /// </summary>
        [Column("cmt_name")]
        [MaxLength(32)]
        public string? CmtName { get; set; } = string.Empty;

        /// <summary>
        /// コメント文
        /// コメントコードの定型文に組み合わせる文字情報
        /// </summary>
        [Column("cmt_opt")]
        [MaxLength(38)]
        public string? CmtOpt { get; set; } = string.Empty;
    }
}
