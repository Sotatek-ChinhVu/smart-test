using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entity.Tenant
{
    /// <summary>
    /// パス設定	
    /// </summary>
    [Table("path_conf")]
    [Index(nameof(HpId), nameof(GrpCd), nameof(GrpEdaNo), nameof(Machine), nameof(IsInvalid), Name = "pt_path_conf_idx01")]
    [Index(nameof(HpId), nameof(GrpCd), nameof(GrpEdaNo), nameof(SeqNo), Name = "pt_path_conf_pkey")]
    public class PathConf : EmrCloneable<PathConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類コード
        /// </summary>
        
        [Column("grp_cd", Order = 2)]
        public int GrpCd { get; set; }

        /// <summary>
        /// 分類枝番
        /// </summary>
        
        [Column("grp_eda_no", Order = 3)]
        public int GrpEdaNo { get; set; }

        /// <summary>
        /// 連番 
        /// </summary
        
        [Column("seq_no", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 端末名 
        /// </summary
        [Column("machine")]
        [MaxLength(60)]
        public string? Machine { get; set; } = string.Empty;

        /// <summary>
        /// パス 
        /// </summary
        [Column("path")]
        [MaxLength(300)]
        public string? Path { get; set; } = string.Empty;

        /// <summary>
        /// パラメーター 
        /// </summary
        [Column("param")]
        [MaxLength(1000)]
        public string? Param { get; set; } = string.Empty;

        /// <summary>
        /// 備考 
        /// </summary
        [Column("biko")]
        [MaxLength(200)]
        public string? Biko { get; set; } = string.Empty;

        /// <summary>
        /// 文字コード 
        ///     0:UTF-8 
        ///     1:S-JIS
        /// </summary
        [Column("char_cd")]
        public int CharCd { get; set; }

        /// <summary>
        /// 無効区分 
        ///     1:無効
        /// </summary
        [Column("is_invalid")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
