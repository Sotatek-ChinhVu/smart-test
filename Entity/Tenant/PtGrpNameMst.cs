using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者分類名称マスタ
    /// </summary>
    [Table(name: "PT_GRP_NAME_MST")]
    [Index(nameof(HpId), nameof(GrpId), nameof(IsDeleted), Name = "PT_GRP_NAME_IDX01")]
    public class PtGrpNameMst : EmrCloneable<PtGrpNameMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column(name: "HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類番号
        /// </summary>
        [Key]
        [Column(name: "GRP_ID", Order = 2)]
        public int GrpId { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column(name: "SORT_NO")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 分類名
        /// </summary>
        [Column(name: "GRP_NAME")]
        [MaxLength(20)]
        public string? GrpName { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///		1:削除	
        /// </summary>
        [Column(name: "IS_DELETED")]
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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}