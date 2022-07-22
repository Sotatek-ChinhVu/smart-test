using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "TODO_GRP_MST_HISTORY")]
    public class TodoGrpMstHistory : EmrCloneable<TodoGrpMst>
    {
        /// <summary>
        /// 履歴番号
        ///     変更していく旅に増えていく
        /// </summary>
        [Key]
        [Column(name: "REVISION", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Revision { get; set; }

        /// <summary>
        /// 医療機関識別ID 
        /// </summary>
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// TODO分類番号 
        /// </summary>
        //[Key]
        [Column("TODO_GRP_NO", Order = 3)]
        public int TodoGrpNo { get; set; }

        /// <summary>
        /// TODO分類名称 
        /// </summary>
        [Column("TODO_GRP_NAME")]
        [MaxLength(20)]
        public string TodoGrpName { get; set; } = string.Empty;

        /// <summary>
        /// 分類色 
        /// </summary>
        [Column("GRP_COLOR")]
        [MaxLength(8)]
        public string GrpColor { get; set; } = string.Empty;

        /// <summary>
        /// 順番 
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除フラグ 
        /// </summary>
        [Column("IS_DELETED")]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時 
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
        /// </summary>
        [Column("CREATE_MACHINE")]
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
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

        /// <summary>
        /// Update type: 
        /// Insert: 挿入
        /// Update: 更新
        /// Delete: 削除
        /// </summary>
        [Column(name: "UPDATE_TYPE")]
        [MaxLength(6)]
        public string UpdateType { get; set; } = string.Empty;

    }
}
