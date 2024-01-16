using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rsvkrt_odr_inf_cmt")]
    public class RsvkrtOdrInfCmt : EmrCloneable<RsvkrtOdrInfCmt>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 予約日
        /// yyyymmdd
        /// </summary>
        [Column("rsv_date")]
        public int RsvDate { get; set; }

        /// <summary>
        /// 予約カルテ番号
        /// 
        /// </summary>
        
        [Column("rsvkrt_no", Order = 3)]
        public long RsvkrtNo { get; set; }

        /// <summary>
        /// 剤番号
        /// ODR_INF_DETAIL.RP_NO
        /// </summary>
        
        [Column("rp_no", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public long RpNo { get; set; }

        /// <summary>
        /// 剤枝番
        /// 
        /// </summary>
        
        [Column("rp_eda_no", Order = 5)]
        public long RpEdaNo { get; set; }

        /// <summary>
        /// 行番号
        /// ODR_INF_DETAIL.ROW_NO
        /// </summary>
        
        [Column("row_no", Order = 6)]
        [CustomAttribute.DefaultValue(1)]
        public int RowNo { get; set; }

        /// <summary>
        /// 枝番
        /// ※2018/11/29現在、1項目につき、最大3つまで
        /// </summary>
        
        [Column("eda_no", Order = 7)]
        [CustomAttribute.DefaultValue(1)]
        public int EdaNo { get; set; }

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
