using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entity.Tenant
{
    /// <summary>
    /// パス設定	
    /// </summary>
    [Table("PATH_CONF")]
    [Index(nameof(HpId), nameof(GrpCd), nameof(GrpEdaNo), nameof(Machine), nameof(IsInvalid), Name = "PT_PATH_CONF_IDX01")]
    [Index(nameof(HpId), nameof(GrpCd), nameof(GrpEdaNo), nameof(SeqNo), Name = "PT_PATH_CONF_PKEY")]
    public class PathConf : EmrCloneable<PathConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類コード
        /// </summary>
        
        [Column("GRP_CD", Order = 2)]
        public int GrpCd { get; set; }

        /// <summary>
        /// 分類枝番
        /// </summary>
        
        [Column("GRP_EDA_NO", Order = 3)]
        public int GrpEdaNo { get; set; }

        /// <summary>
        /// 連番 
        /// </summary
        
        [Column("SEQ_NO", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 端末名 
        /// </summary
        [Column("MACHINE")]
        [MaxLength(60)]
        public string? Machine { get; set; } = string.Empty;

        /// <summary>
        /// パス 
        /// </summary
        [Column("PATH")]
        [MaxLength(300)]
        public string? Path { get; set; } = string.Empty;

        /// <summary>
        /// パラメーター 
        /// </summary
        [Column("PARAM")]
        [MaxLength(1000)]
        public string? Param { get; set; } = string.Empty;

        /// <summary>
        /// 備考 
        /// </summary
        [Column("BIKO")]
        [MaxLength(200)]
        public string? Biko { get; set; } = string.Empty;

        /// <summary>
        /// 文字コード 
        ///     0:UTF-8 
        ///     1:S-JIS
        /// </summary
        [Column("CHAR_CD")]
        public int CharCd { get; set; }

        /// <summary>
        /// 無効区分 
        ///     1:無効
        /// </summary
        [Column("IS_INVALID")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
