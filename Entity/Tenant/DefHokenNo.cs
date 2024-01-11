using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// デフォルト保険番号設定
	/// </summary>
	[Table(name: "def_hoken_no")]
    [Index(nameof(HpId), nameof(Digit1), nameof(Digit2), nameof(Digit3), nameof(Digit4), nameof(Digit5), nameof(Digit6), nameof(Digit7), nameof(Digit8), nameof(IsDeleted), Name = "def_hoken_no_idx01")]
    public class DefHokenNo : EmrCloneable<DefHokenNo>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column(name: "hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 1桁目
        /// </summary>
        
        [Column(name: "digit_1", Order = 2)]
        [MaxLength(1)]
        public string Digit1 { get; set; } = string.Empty;

        /// <summary>
        /// 2桁目
        /// </summary>
        
        [Column(name: "digit_2", Order = 3)]
        [MaxLength(1)]
        public string Digit2 { get; set; } = string.Empty;

        /// <summary>
        /// 3桁目
        /// </summary>
        [Column(name: "digit_3")]
        [MaxLength(1)]
        public string? Digit3 { get; set; } = string.Empty;

        /// <summary>
        /// 4桁目
        /// </summary>
        [Column(name: "digit_4")]
        [MaxLength(1)]
        public string? Digit4 { get; set; } = string.Empty;

        /// <summary>
        /// 5桁目
        /// </summary>
        [Column(name: "digit_5")]
        [MaxLength(1)]
        public string? Digit5 { get; set; } = string.Empty;

        /// <summary>
        /// 6桁目
        /// </summary>
        [Column(name: "digit_6")]
        [MaxLength(1)]
        public string? Digit6 { get; set; } = string.Empty;

        /// <summary>
        /// 7桁目
        /// </summary>
        [Column(name: "digit_7")]
        [MaxLength(1)]
        public string? Digit7 { get; set; } = string.Empty;

        /// <summary>
        /// 8桁目
        /// </summary>
        [Column(name: "digit_8")]
        [MaxLength(1)]
        public string? Digit8 { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column(name: "seq_no", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 保険番号
        /// </summary>
        [Column(name: "hoken_no")]
        [Required]
        public int HokenNo { get; set; }

        /// <summary>
        /// 保険番号枝番
        /// </summary>
        [Column(name: "hoken_eda_no")]
        [Required]
        public int HokenEdaNo { get; set; }

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
