using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// （来院区分コード予約枠条件設定）
	/// </summary>
	[Table(name: "raiin_kbn_yoyaku")]
    [Index(nameof(HpId), nameof(GrpId), nameof(KbnCd), nameof(IsDeleted), Name = "raiin_kbn_yoyaku_idx01")]
    public class RaiinKbnYayoku : EmrCloneable<RaiinKbnYayoku>
    {
        /// <summary>
        ///医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類ID
        /// </summary>
        
        [Column("grp_id", Order = 2)]
        public int GrpId { get; set; }

        /// <summary>
        /// 分類ID
        /// </summary>
        
        [Column("kbn_cd", Order = 3)]
        public int KbnCd { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column(name: "seq_no", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }



        /// <summary>
        /// 予約枠コード
        /// </summary>
        [Column(name: "yoyaku_cd")]
        public int YoyakuCd { get; set; }

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
