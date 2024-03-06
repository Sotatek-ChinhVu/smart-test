using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "audit_trail_log_detail")]
    public class AuditTrailLogDetail : EmrCloneable<AuditTrailLogDetail>
    {
        /// <summary>
        /// ログID
        /// AUDIT_TRAILINC_LOG.LOG_ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("log_id", Order = 1)]
        public long LogId { get; set; }

        /// <summary>
        /// 補足
        /// 
        /// </summary>
        [Column("hosoku")]
        public string? Hosoku { get; set; } = string.Empty;

    }
    
}
