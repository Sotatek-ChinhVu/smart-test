using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "audit_trail_log")]
    public class AuditTrailLog : EmrCloneable<AuditTrailLog>
    {
        /// <summary>
        /// ログID
        /// 
        /// </summary>
        
        [Column("log_id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogId { get; set; }

        /// <summary>
        /// ログ日時
        /// 
        /// </summary>
        [Column("log_date")]
        public DateTime LogDate { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// ユーザーID
        /// 
        /// </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// イベントコード
        /// EVENT_MST.EVENT_CD
        /// </summary>
        [Column("event_cd")]
        [MaxLength(11)]
        public string? EventCd { get; set; } = string.Empty;

        /// <summary>
        /// 患者番号
        /// 
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("sin_day")]
        public int SinDay { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        [Column("raiin_no")]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 端末名
        /// 
        /// </summary>
        [Column("machine")]
        [MaxLength(60)]
        public string? Machine { get; set; } = string.Empty;
    }
}
