using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 受付種別マスタ
    /// </summary>
    [Table(name: "UKETUKE_SBT_MST")]
    public class UketukeSbtMst : EmrCloneable<UketukeSbtMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        //[Key]
        [Column("HP_ID", Order = 1)]
        //[Index("UKETUKE_SBT_MST_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 区分ID
        /// </summary>
        //[Key]
        [Column("KBN_ID", Order = 2)]
        //[Index("UKETUKE_SBT_MST_IDX01", 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KbnId { get; set; }

        /// <summary>
        /// 区分名称
        /// </summary>
        [Column(name: "KBN_NAME")]
        [MaxLength(20)]
        [Required]
        public string? KbnName { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("IS_DELETED")]
        //[Index("UKETUKE_SBT_MST_IDX01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column(name: "SORT_NO")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

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