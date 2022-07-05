using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 診療科マスタ
	/// </summary>
	[Table(name: "KANTOKU_MST")]
    public class KantokuMst : EmrCloneable<KantokuMst>
    {
		/// <summary>
		/// 労働局コード	
		/// </summary>
		[Key]
        [Column(name: "ROUDOU_CD", Order = 1)]
		[MaxLength(2)]
        public string RoudouCd { get; set; }

        /// <summary>
        /// 監督署コード
        /// </summary>
        [Key]
        [Column(name: "KANTOKU_CD", Order = 2)]
		[MaxLength(2)]
        public string KantokuCd { get; set; }

        /// <summary>
        /// 監督署名
        /// </summary>
        [Column(name: "KANTOKU_NAME")]
        [MaxLength(60)]
        [Required]
        public string KantokuName { get; set; }

		/// <summary>
		/// 登録日時		
		/// </summary>
		[Column("CREATE_DATE")]
		[CustomAttribute.DefaultValueSql("current_timestamp")]
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// 更新日時			
		/// </summary>
		[Column("UPDATE_DATE")]
		public DateTime UpdateDate { get; set; }
	}
}