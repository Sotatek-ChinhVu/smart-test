using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "todo_grp_mst_history")]
    public class TodoGrpMstHistory : EmrCloneable<TodoGrpMst>
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
        /// TODO分類番号 
        /// </summary>
        
        [Column("todo_grp_no", Order = 3)]
        public int TodoGrpNo { get; set; }

        /// <summary>
        /// TODO分類名称 
        /// </summary>
        [Column("todo_grp_name")]
        [MaxLength(20)]
        public string? TodoGrpName { get; set; } = string.Empty;

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
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時 
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
