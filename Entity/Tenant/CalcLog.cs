using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "CALC_LOG")]
    public class CalcLog : EmrCloneable<CalcLog>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Key]
        [Column("RAIIN_NO", Order = 3)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Key]
        [Column("SEQ_NO", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public int SeqNo { get; set; }

        /// <summary>
        /// ログ種別
        /// 0:通常 1:注意 2:警告
        /// </summary>
        [Column("LOG_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int LogSbt { get; set; }

        /// <summary>
        /// ログ
        /// 
        /// </summary>
        [Column("TEXT")]
        [MaxLength(1000)]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

    }
}
