using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "EVENT_MST")]
    public class EventMst : EmrCloneable<EventMst>
    {
        /// <summary>
        /// イベントコード
        /// 
        /// </summary>
        
        [Column("EVENT_CD", Order = 1)]
        [MaxLength(11)]
        public string EventCd { get; set; } = string.Empty;

        /// <summary>
        /// イベント名
        /// 
        /// </summary>
        [Column("EVENT_NAME")]
        [MaxLength(100)]
        public string? EventName { get; set; } = string.Empty;

        /// <summary>
        /// 監査証跡
        /// 1: AUDIT_TRAILING_LOG出力対象イベント
        /// </summary>
        [Column("AUDIT_TRAILING")]
        [CustomAttribute.DefaultValue(0)]
        public int AuditTrailing { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
    }
}
