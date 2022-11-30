using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "AUDIT_TRAIL_LOG_DETAIL")]
    public class AuditTrailLogDetail : EmrCloneable<AuditTrailLogDetail>
    {
        /// <summary>
        /// ログID
        /// AUDIT_TRAILINC_LOG.LOG_ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("LOG_ID", Order = 1)]
        public long LogId { get; set; }

        /// <summary>
        /// 補足
        /// 
        /// </summary>
        [Column("HOSOKU")]
        public string? Hosoku { get; set; } = string.Empty;

    }
}
