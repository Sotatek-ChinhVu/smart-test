using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "event_mst")]
    public class EventMst : EmrCloneable<EventMst>
    {

        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// イベントコード
        /// 
        /// </summary>

        [Column("event_cd", Order = 1)]
        [MaxLength(11)]
        public string EventCd { get; set; } = string.Empty;

        /// <summary>
        /// イベント名
        /// 
        /// </summary>
        [Column("event_name")]
        [MaxLength(100)]
        public string? EventName { get; set; } = string.Empty;

        /// <summary>
        /// 監査証跡
        /// 1: AUDIT_TRAILING_LOG出力対象イベント
        /// </summary>
        [Column("audit_trailing")]
        [CustomAttribute.DefaultValue(0)]
        public int AuditTrailing { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
    }
}
