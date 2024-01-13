using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 医療機関情報
	/// </summary>
	[Table(name: "hp_inf")]
	public class HpInf : EmrCloneable<HpInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column(name: "hp_id", Order = 1)]
		public int HpId { get; set; }

		/// <summary>
		/// 開始日
		///     yyyymmdd
		/// </summary>
		
		[Column(name: "start_date", Order = 2)]
		public int StartDate { get; set; }

		/// <summary>
		/// 医療機関コード			
		/// </summary>
		[Column(name: "hp_cd")]
		[MaxLength(7)]
		public string? HpCd { get; set; } = string.Empty;

		/// <summary>
		/// 労災医療機関コード			
		/// </summary>
		[Column(name: "rousai_hp_cd")]
		[MaxLength(7)]
		public string? RousaiHpCd { get; set; } = string.Empty;

		/// <summary>
		/// 医療機関名			
		/// </summary>
		[Column(name: "hp_name")]
		[MaxLength(80)]
		public string? HpName { get; set; } = string.Empty;

		/// <summary>
		/// レセ医療機関名			
		/// </summary>
		[Column(name: "rece_hp_name")]
		[MaxLength(80)]
		public string? ReceHpName { get; set; } = string.Empty;

		/// <summary>
		/// 開設者氏名			
		/// </summary>
		[Column(name: "kaisetu_name")]
		[MaxLength(40)]
		public string? KaisetuName { get; set; } = string.Empty;

		/// <summary>
		/// 郵便番号			
		/// </summary>
		[Column(name: "post_cd")]
		[MaxLength(7)]
		public string? PostCd { get; set; } = string.Empty;

		/// <summary>
		/// 都道府県番号			
		/// </summary>
		[Column(name: "pref_no")]
		public int PrefNo { get; set; }

		/// <summary>
		/// 医療機関所在地１			
		/// </summary>
		[Column(name: "address1")]
		[MaxLength(100)]
		public string? Address1 { get; set; } = string.Empty;

		/// <summary>
		/// 医療機関所在地２			
		/// </summary>
		[Column(name: "address2")]
		[MaxLength(100)]
		public string? Address2 { get; set; } = string.Empty;

		/// <summary>
		/// 電話番号			
		/// </summary>
		[Column(name: "tel")]
		[MaxLength(15)]
		public string? Tel { get; set; } = string.Empty;

		/// <summary>
		/// FAX番号			
		/// </summary>
		[Column(name: "fax_no")]
        [MaxLength(15)]
        public string? FaxNo { get; set; } = string.Empty;

		/// <summary>
		/// その他連絡先
		/// </summary>
		[Column(name: "other_contacts")]
        [MaxLength(100)]
        public string? OtherContacts { get; set; } = string.Empty;

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
		public string? UpdateMachine { get; set; }  = string.Empty;
	}
}