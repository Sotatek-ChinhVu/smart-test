using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RENKEI_TIMING_MST")]
    public class RenkeiTimingMst : EmrCloneable<RenkeiTimingMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 連携ID
        /// 
        /// </summary>
        //[Key]
        [Column("RENKEI_ID", Order = 2)]
        public int RenkeiId { get; set; }

        /// <summary>
        /// イベントコード
        /// EVENT_MST.EVENT_CD
        /// </summary>
        //[Key]
        [Column("EVENT_CD", Order = 3)]
        [MaxLength(11)]
        public string EventCd { get; set; } = string.Empty;

        /// <summary>
        /// 無効区分
        /// 0:有効 1:無効
        /// </summary>
        [Column("IS_INVALID")]
        [CustomAttribute.DefaultValue(1)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}
