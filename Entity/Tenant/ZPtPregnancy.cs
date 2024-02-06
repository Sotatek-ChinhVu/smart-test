using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "z_pt_pregnancy")]
    public class ZPtPregnancy : EmrCloneable<ZPtPregnancy>
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
        /// 連番
        /// </summary>
        [Column("id")]
        //[Index("ptpregnancy_idx01", 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("seq_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 妊娠開始日
        /// 
        /// </summary>
        [Column("start_date")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 妊娠終了日
        /// 
        /// </summary>
        [Column("end_date")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 月経日
        /// YYYYMMDD(最終月経日)
        /// </summary>
        [Column("period_date")]
        [CustomAttribute.DefaultValue(0)]
        public int PeriodDate { get; set; }

        /// <summary>
        /// 月経予定日
        /// 
        /// </summary>
        [Column("period_due_date")]
        [CustomAttribute.DefaultValue(0)]
        public int PeriodDueDate { get; set; }

        /// <summary>
        /// 排卵日
        /// YYYYMMDD(最終排卵日)
        /// </summary>
        [Column("ovulation_date")]
        [CustomAttribute.DefaultValue(0)]
        public int OvulationDate { get; set; }

        /// <summary>
        /// 排卵予定日
        /// 
        /// </summary>
        [Column("ovulation_due_date")]
        [CustomAttribute.DefaultValue(0)]
        public int OvulationDueDate { get; set; }

        /// <summary>
        /// 削除フラグ
        /// 1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
