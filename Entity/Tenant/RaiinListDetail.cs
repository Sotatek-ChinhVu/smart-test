using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "raiin_list_detail")]
    [Index(nameof(HpId), nameof(GrpId), nameof(KbnCd), nameof(IsDeleted), Name = "raiin_list_detail_idx01")]
    [Index(nameof(HpId), nameof(IsDeleted), Name = "raiin_list_detail_idx02")]

    public class RaiinListDetail : EmrCloneable<RaiinListDetail>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類ID
        /// 
        /// </summary>
        
        [Column("grp_id", Order = 2)]
        public int GrpId { get; set; }

        /// <summary>
        /// 区分コード
        /// 
        /// </summary>
        
        [Column("kbn_cd", Order = 3)]
        public int KbnCd { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// 区分名称
        /// 
        /// </summary>
        [Column("kbn_name")]
        [MaxLength(20)]
        public string? KbnName { get; set; } = string.Empty;

        /// <summary>
        /// 配色
        /// 
        /// </summary>
        [Column("color_cd")]
        [MaxLength(8)]
        public string? ColorCd { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
