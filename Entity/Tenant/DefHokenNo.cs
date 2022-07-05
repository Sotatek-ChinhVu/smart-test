using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    /// <summary>
	/// デフォルト保険番号設定
	/// </summary>
	[Table(name: "DEF_HOKEN_NO")]
    [Index(nameof(HpId), nameof(Digit1), nameof(Digit2), nameof(Digit3), nameof(Digit4), nameof(Digit5), nameof(Digit6), nameof(Digit7), nameof(Digit8), nameof(IsDeleted), Name = "DEF_HOKEN_NO_IDX01")]
    public class DefHokenNo : EmrCloneable<DefHokenNo>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column(name: "HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 1桁目
        /// </summary>
        [Key]
        [Column(name: "DIGIT_1", Order = 2)]
        [MaxLength(1)]
        public string Digit1 { get; set; }

        /// <summary>
        /// 2桁目
        /// </summary>
        [Key]
        [Column(name: "DIGIT_2", Order = 3)]
        [MaxLength(1)]
        public string Digit2 { get; set; }

        /// <summary>
        /// 3桁目
        /// </summary>
        [Column(name: "DIGIT_3")]
        [MaxLength(1)]
        public string Digit3 { get; set; }

        /// <summary>
        /// 4桁目
        /// </summary>
        [Column(name: "DIGIT_4")]
        [MaxLength(1)]
        public string Digit4 { get; set; }

        /// <summary>
        /// 5桁目
        /// </summary>
        [Column(name: "DIGIT_5")]
        [MaxLength(1)]
        public string Digit5 { get; set; }

        /// <summary>
        /// 6桁目
        /// </summary>
        [Column(name: "DIGIT_6")]
        [MaxLength(1)]
        public string Digit6 { get; set; }

        /// <summary>
        /// 7桁目
        /// </summary>
        [Column(name: "DIGIT_7")]
        [MaxLength(1)]
        public string Digit7 { get; set; }

        /// <summary>
        /// 8桁目
        /// </summary>
        [Column(name: "DIGIT_8")]
        [MaxLength(1)]
        public string Digit8 { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Key]
        [Column(name: "SEQ_NO", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 保険番号
        /// </summary>
        [Column(name: "HOKEN_NO")]
        [Required]
        public  int HokenNo { get; set; }

        /// <summary>
        /// 保険番号枝番
        /// </summary>
        [Column(name: "HOKEN_EDA_NO")]
        [Required]
        public int HokenEdaNo { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column(name: "SORT_NO")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
		/// 削除区分
		///		1:削除		
		/// </summary>
		[Column(name: "IS_DELETED")]
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
        public string CreateMachine { get; set; }

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
        public string UpdateMachine { get; set; }
    }
}
