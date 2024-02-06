using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者分類項目
    /// </summary>
    [Table(name: "pt_grp_item")]
    [Index(nameof(HpId), nameof(GrpId), nameof(GrpCode), nameof(IsDeleted), Name = "pt_grp_item_idx01")]
    public class PtGrpItem : EmrCloneable<PtGrpItem>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column(name: "hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類番号
        /// </summary>
        
        [Column(name: "grp_id", Order = 2)]
        public int GrpId { get; set; }

        /// <summary>
        /// 分類項目コード
        /// </summary>
        
        [Column(name: "grp_code", Order = 3)]
        [MaxLength(2)]
        public string GrpCode { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column(name: "seq_no", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 分類項目名称
        /// </summary>
        [Column(name: "grp_code_name")]
        [MaxLength(30)]
        public string? GrpCodeName { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        [Column(name: "sort_no")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除		
        /// </summary>
        [Column(name: "is_deleted")]
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