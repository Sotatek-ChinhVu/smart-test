using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "AUDIT_TRAIL_LOG")]
    public class AuditTrailLog : EmrCloneable<AuditTrailLog>
    {
        /// <summary>
        /// ログID
        /// 
        /// </summary>
        
        [Column("LOG_ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogId { get; set; }

        /// <summary>
        /// ログ日時
        /// 
        /// </summary>
        [Column("LOG_DATE")]
        public DateTime LogDate { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// ユーザーID
        /// 
        /// </summary>
        [Column("USER_ID")]
        public int UserId { get; set; }

        /// <summary>
        /// イベントコード
        /// EVENT_MST.EVENT_CD
        /// </summary>
        [Column("EVENT_CD")]
        [MaxLength(11)]
        public string? EventCd { get; set; } = string.Empty;

        /// <summary>
        /// 患者番号
        /// 
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("SIN_DAY")]
        public int SinDay { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Column("RAIIN_NO")]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 端末名
        /// 
        /// </summary>
        [Column("MACHINE")]
        [MaxLength(60)]
        public string? Machine { get; set; } = string.Empty;
    }
}
