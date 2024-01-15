using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "todo_kbn_mst")]
    public class TodoKbnMst : EmrCloneable<TodoKbnMst>
    {
        /// <summary>
        /// 医療機関識別ID 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// TODO区分番号 
        /// </summary>
        
        [Column("todo_kbn_no", Order = 2)]
        public int TodoKbnNo { get; set; }

        /// <summary>
        /// TODO区分名称 
        /// </summary>
        [Column("todo_kbn_name")]
        [MaxLength(20)]
        public string? TodoKbnName { get; set; } = string.Empty;

        /// <summary>
        /// 動作コード 
        /// </summary>
        [Column("act_cd")]
        public int ActCd { get; set; }

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
