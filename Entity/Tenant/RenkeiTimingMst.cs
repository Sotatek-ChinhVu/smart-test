using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "renkei_timing_mst")]
    public class RenkeiTimingMst : EmrCloneable<RenkeiTimingMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 連携ID
        /// 
        /// </summary>
        
        [Column("renkei_id", Order = 2)]
        public int RenkeiId { get; set; }

        /// <summary>
        /// イベントコード
        /// EVENT_MST.EVENT_CD
        /// </summary>
        
        [Column("event_cd", Order = 3)]
        [MaxLength(11)]
        public string EventCd { get; set; } = string.Empty;

        /// <summary>
        /// 無効区分
        /// 0:有効 1:無効
        /// </summary>
        [Column("is_invalid")]
        [CustomAttribute.DefaultValue(1)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }
    }
}
