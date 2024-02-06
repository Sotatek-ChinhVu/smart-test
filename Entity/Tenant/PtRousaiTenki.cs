using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// （患者労災転帰事由情報）
	/// </summary>
	[Table("pt_rousai_tenki")]
    [Index(nameof(HpId), nameof(PtId), nameof(HokenId), nameof(EndDate), nameof(IsDeleted), Name = "pt_rousai_tenki_idx01")]
    public class PtRousaiTenki : EmrCloneable<PtRousaiTenki>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 保険ID
        ///		患者別に保険情報を識別するための固有の番号
        /// </summary>
        
        [Column("hoken_id", Order = 3)]
        public int HokenId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("seq_no", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
		/// 適用終了日
		///		yyyymmdd
		/// </summary>
		[Column("end_date")]
        [CustomAttribute.DefaultValue(999999)]
        public int EndDate { get; set; }

        /// <summary>
		/// 新継再別
		/// </summary>
        [Required]
        [Column("sinkei")]
        public int Sinkei { get; set; }

        /// <summary>
		/// 転帰事由
		/// </summary>
        /// 
        [Required]
        [Column("tenki")]
        public int Tenki { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("is_deleted")]
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
