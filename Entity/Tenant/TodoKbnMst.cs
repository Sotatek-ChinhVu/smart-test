using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "TODO_KBN_MST")]
    public class TodoKbnMst : EmrCloneable<TodoKbnMst>
    {
        /// <summary>
        /// 医療機関識別ID 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// TODO区分番号 
        /// </summary>
        //[Key]
        [Column("TODO_KBN_NO", Order = 2)]
        public int TodoKbnNo { get; set; }

        /// <summary>
        /// TODO区分名称 
        /// </summary>
        [Column("TODO_KBN_NAME")]
        [MaxLength(20)]
        public string TodoKbnName { get; set; }

        /// <summary>
        /// 動作コード 
        /// </summary>
        [Column("ACT_CD")]
        public int ActCd { get; set; }

        /// <summary>
        /// 作成日時 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }

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
        public string UpdateMachine { get; set; }

    }
}
