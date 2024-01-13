using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 来院区分コード項目条件設定
    /// </summary>
    [Table("raiin_kbn_item")]
    [Index(nameof(HpId), nameof(GrpCd), nameof(KbnCd), nameof(IsDeleted), Name = "raiin_kbn_item_idx01")]
    public class RaiinKbItem : EmrCloneable<RaiinKbItem>
    {
        /// <summary>
        ///医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類ID
        /// </summary>
        
        [Column("grp_id", Order = 2)]
        public int GrpCd { get; set; }

        /// <summary>
        /// 区分コード
        /// </summary>
        
        [Column("kbn_cd", Order = 3)]
        public int KbnCd { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column(name: "seq_no", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 項目コード
        /// </summary>

        [Column(name: "item_cd")]
        [MaxLength(10)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("is_exclude")]
        [CustomAttribute.DefaultValue(0)]
        public int IsExclude { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除
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

        /// <summary>
        /// 並び順
        /// </summary>
        [Column(name: "sort_no")]
        public int SortNo { get; set; }
    }
}
