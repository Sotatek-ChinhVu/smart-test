using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SET_GENERATION_MST")]
    public class SetGenerationMst : EmrCloneable<SetGenerationMst>
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
        /// 世代ID
        /// 
        /// </summary>
        [Key]
        [Column("GENERATION_ID", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GenerationId { get; set; }

        /// <summary>
        /// 使用開始日
        /// 
        /// </summary>
        [Column("START_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 削除区分
        /// 1: 削除
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
