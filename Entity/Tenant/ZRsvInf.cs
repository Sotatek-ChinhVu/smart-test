using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 文書情報
    /// </summary>
    [Table(name: "z_rsv_inf")]
    public class ZRsvInf : EmrCloneable<ZRsvInf>
    {
        
        [Column("op_id", Order = 1)]
        public long OpId { get; set; }

        [Column("op_type")]
        [MaxLength(10)]
        public string? OpType { get; set; } = string.Empty;

        [Column("op_time")]
        public DateTime OpTime { get; set; }

        [Column("op_addr")]
        [MaxLength(100)]
        public string? OpAddr { get; set; } = string.Empty;

        [Column("op_hostname")]
        [MaxLength(100)]
        public string? OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 予約枠ID
        /// 
        /// </summary>
        [Column("rsv_frame_id")]
        public int RsvFrameId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("sin_date")]
        [CustomAttribute.DefaultValue(0)]
        public int SinDate { get; set; }

        /// <summary>
        /// 開始時間
        /// 
        /// </summary>
        [Column("start_time")]
        [CustomAttribute.DefaultValue(0)]
        public int StartTime { get; set; }

        /// <summary>
        /// 予約番号
        /// 
        /// </summary>
        [Column("raiin_no")]
        [CustomAttribute.DefaultValue(0)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("pt_id")]
        [CustomAttribute.DefaultValue(0)]
        public long PtId { get; set; }

        /// <summary>
        /// 予約種別コード
        /// 
        /// </summary>
        [Column("rsv_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int RsvSbt { get; set; }

        /// <summary>
        /// 担当医師コード
        /// 
        /// </summary>
        [Column("tanto_id")]
        [CustomAttribute.DefaultValue(0)]
        public int TantoId { get; set; }

        /// <summary>
        /// 診療科コード
        /// 
        /// </summary>
        [Column("ka_id")]
        [CustomAttribute.DefaultValue(0)]
        public int KaId { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
