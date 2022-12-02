using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RENKEI_TIMING_CONF")]
    public class RenkeiTimingConf : EmrCloneable<RenkeiTimingConf>
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
        [Column("RENKEI_ID", Order = 2)]
        public int RenkeiId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("SEQ_NO", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SeqNo { get; set; }

        /// <summary>
        /// イベントコード
        /// EVENT_MST.EVENT_CD
        /// </summary>
        //[Key]
        [Column("EVENT_CD", Order = 4)]
        [MaxLength(11)]
        public string EventCd { get; set; } = string.Empty;

        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", Order = 5)]
        public long Id { get; set; }

        /// <summary>
        /// 無効区分
        /// 0:有効 1:無効
        /// </summary>
        [Column("IS_INVALID")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
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
        /// 更新者
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
        public string? UpdateMachine { get; set; } = string.Empty;

    }
}
