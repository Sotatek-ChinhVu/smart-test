using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "tag_grp_mst")]
    public class TagGrpMst : EmrCloneable<TagGrpMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 付箋分類番号
        /// 0-分類なし、99-"チームカルテ"
        /// </summary>
        
        [Column("tag_grp_no", Order = 2)]
        public int TagGrpNo { get; set; }

        /// <summary>
        /// 付箋分類名称
        /// "TAG_GRP_NO=0は、""（分類なし）""固定
        /// TAG_GRP_NO=99は、""チームカルテ""固定"
        /// </summary>
        [Column("tag_grp_name")]
        [MaxLength(20)]
        public string? TagGrpName { get; set; } = string.Empty;

        /// <summary>
        /// 分類色
        /// </summary>
        [Column("grp_color")]
        [MaxLength(8)]
        public string? GrpColor { get; set; } = string.Empty;

        /// <summary>
        /// 順番
        /// </summary>
        [Column("sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("create_machine")]
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
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
