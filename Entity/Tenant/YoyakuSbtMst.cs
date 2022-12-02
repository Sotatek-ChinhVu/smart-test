using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 予約種別マスタ
    /// </summary>
    [Table(name: "YOYAKU_SBT_MST")]
    public class YoyakuSbtMst
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column(name: "HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 予約種別
        /// </summary>
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("YOYAKU_SBT", Order = 2)]
        public int YoyakuSbt { get; set; }

        /// <summary>
        /// 種別名
        /// </summary>
        [Required]
        [Column("SBT_NAME")]
        [MaxLength(120)]
        public string? SbtName { get; set; } = string.Empty;

        /// <summary>
        /// 初期コメント
        /// </summary>
        [Column("DEFAULT_CMT")]
        [MaxLength(120)]
        public string? DefaultCmt { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        [Required]
        [Column(name: "SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("IS_DELETED")]
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
