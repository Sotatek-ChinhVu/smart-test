using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// 予約枠マスタ
	/// </summary>
	[Table("rsv_frame_mst")]
    public class RsvFrameMst : EmrCloneable<RsvFrameMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 予約分類ID
        /// </summary>
        [Column("rsv_grp_id")]
        public int RsvGrpId { get; set; }

        /// <summary>
        /// 予約枠ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("rsv_frame_id", Order = 2)]
        public int RsvFrameId { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_key")]
        public int SortKey { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("rsv_frame_name")]
        [MaxLength(60)]
        public string? RsvFrameName { get; set; } = string.Empty;

        /// <summary>
        /// 担当医ID
        /// </summary>
        [Column("tanto_id")]
        [CustomAttribute.DefaultValue(0)]
        public int TantoId { get; set; }

        /// <summary>
        /// 診療科ID
        /// </summary>
        [Column("ka_id")]
        [CustomAttribute.DefaultValue(0)]
        public int KaId { get; set; }

        /// <summary>
        /// 来院作成区分
        ///     1:作成
        /// </summary>
        [Column("make_raiin")]
        [CustomAttribute.DefaultValue(0)]
        public int MakeRaiin { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末	
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時	
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末	
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
