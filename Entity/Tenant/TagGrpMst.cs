using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "TAG_GRP_MST")]
    public class TagGrpMst : EmrCloneable<TagGrpMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 付箋分類番号
        /// 0-分類なし、99-"チームカルテ"
        /// </summary>
        [Key]
        [Column("TAG_GRP_NO", Order = 2)]
        public int TagGrpNo { get; set; }

        /// <summary>
        /// 付箋分類名称
        /// "TAG_GRP_NO=0は、""（分類なし）""固定
        /// TAG_GRP_NO=99は、""チームカルテ""固定"
        /// </summary>
        [Column("TAG_GRP_NAME")]
        [MaxLength(20)]
        public string TagGrpName { get; set; }

        /// <summary>
        /// 分類色
        /// </summary>
        [Column("GRP_COLOR")]
        [MaxLength(8)]
        public string GrpColor { get; set; }

        /// <summary>
        /// 順番
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }

    }
}
