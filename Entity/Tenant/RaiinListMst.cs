using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RAIIN_LIST_MST")]
    [Index(nameof(HpId), nameof(GrpId), nameof(IsDeleted), Name = "RAIIN_LIST_MST_IDX01")]
    public class RaiinListMst : EmrCloneable<RaiinListMst>
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
        /// 分類ID
        /// 
        /// </summary>
        [Key]
        [Column("GRP_ID", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GrpId { get; set; }

        /// <summary>
        /// 分類名称
        /// 
        /// </summary>
        [Column("GRP_NAME")]
        [MaxLength(20)]
        public string? GrpName { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除区分
        /// 
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
