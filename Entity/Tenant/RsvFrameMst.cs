using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// 予約枠マスタ
	/// </summary>
	[Table("RSV_FRAME_MST")]
    public class RsvFrameMst : EmrCloneable<RsvFrameMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        //[Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 予約分類ID
        /// </summary>
        [Column("RSV_GRP_ID")]
        public int RsvGrpId { get; set; }

        /// <summary>
        /// 予約枠ID
        /// </summary>
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RSV_FRAME_ID", Order = 2)]
        public int RsvFrameId { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("SORT_KEY")]
        public int SortKey { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("RSV_FRAME_NAME")]
        [MaxLength(60)]
        public string RsvFrameName { get; set; } = string.Empty;

        /// <summary>
        /// 担当医ID
        /// </summary>
        [Column("TANTO_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int TantoId { get; set; }

        /// <summary>
        /// 診療科ID
        /// </summary>
        [Column("KA_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int KaId { get; set; }

        /// <summary>
        /// 来院作成区分
        ///     1:作成
        /// </summary>
        [Column("MAKE_RAIIN")]
        [CustomAttribute.DefaultValue(0)]
        public int MakeRaiin { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末	
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時	
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末	
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
