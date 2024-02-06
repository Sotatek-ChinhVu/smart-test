using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 診療科マスタ
	/// </summary>
	[Table(name: "kantoku_mst")]
    public class KantokuMst : EmrCloneable<KantokuMst>
    {
        /// <summary>
        /// 労働局コード	
        /// </summary>
        
        [Column(name: "roudou_cd", Order = 1)]
		[MaxLength(2)]
        public string RoudouCd { get; set; } = string.Empty;

        /// <summary>
        /// 監督署コード
        /// </summary>
        
        [Column(name: "kantoku_cd", Order = 2)]
		[MaxLength(2)]
        public string KantokuCd { get; set; } = string.Empty;

        /// <summary>
        /// 監督署名
        /// </summary>
        [Column(name: "kantoku_name")]
        [MaxLength(60)]
        [Required]
        public string? KantokuName { get; set; } = string.Empty;

        /// <summary>
        /// 登録日時		
        /// </summary>
        [Column("create_date")]
		[CustomAttribute.DefaultValueSql("current_timestamp")]
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// 更新日時			
		/// </summary>
		[Column("update_date")]
		public DateTime UpdateDate { get; set; }
	}
}