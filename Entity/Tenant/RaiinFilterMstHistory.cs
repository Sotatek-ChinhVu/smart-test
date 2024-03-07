using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table("raiin_filter_mst_history")]
    [Index(nameof(HpId), nameof(FilterId), nameof(IsDeleted), Name = "raiin_filter_mst_idx01")]
    public class RaiinFilterMstHistory : EmrCloneable<RaiinFilterMstHistory>
    {
        /// <summary>
        /// 履歴番号
        ///     変更していく旅に増えていく
        /// </summary>
        
        [Column(name: "revision", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Revision { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 2)]
        public int HpId { get; set; }
        /// <summary>
        /// フィルターID
        /// </summary>
        
        [Column("filter_id", Order = 3)]
        public int FilterId { get; set; }
        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("seq_no", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }
        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        public int SortNo { get; set; }
        /// <summary>
        /// フィルター名称
        /// </summary>
        [Column("filter_name")]
        public string? FilterName { get; set; } = string.Empty;
        /// <summary>
        /// 選択区分
        /// </summary>
        [Column("select_kbn")]
        public int SelectKbn { get; set; }
        /// <summary>
        /// 削除区分
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
		[Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;

        /// <summary>
        /// Update type: 
        /// Insert: 挿入
        /// Update: 更新
        /// Delete: 削除
        /// </summary>
        [Column(name: "update_type")]
        [MaxLength(6)]
        public string? UpdateType { get; set; } = string.Empty;
    }
}
