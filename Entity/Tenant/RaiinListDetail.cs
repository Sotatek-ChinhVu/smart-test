using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RAIIN_LIST_DETAIL")]
    [Index(nameof(HpId), nameof(GrpId), nameof(KbnCd), nameof(IsDeleted), Name = "RAIIN_LIST_DETAIL_IDX01")]
    public class RaiinListDetail : EmrCloneable<RaiinListDetail>
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
        public int GrpId { get; set; }

        /// <summary>
        /// 区分コード
        /// 
        /// </summary>
        [Key]
        [Column("KBN_CD", Order = 3)]
        public int KbnCd { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 区分名称
        /// 
        /// </summary>
        [Column("KBN_NAME")]
        [MaxLength(20)]
        public string? KbnName { get; set; } = string.Empty;

        /// <summary>
        /// 配色
        /// 
        /// </summary>
        [Column("COLOR_CD")]
        [MaxLength(8)]
        public string? ColorCd { get; set; } = string.Empty;

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
