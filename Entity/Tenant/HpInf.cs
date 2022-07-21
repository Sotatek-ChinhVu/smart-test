using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 医療機関情報
	/// </summary>
	[Table(name: "HP_INF")]
	public class HpInf : EmrCloneable<HpInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column(name: "HP_ID", Order = 1)]
		public int HpId { get; set; }

		/// <summary>
		/// 開始日
		///     yyyymmdd
		/// </summary>
		//[Key]
		[Column(name: "START_DATE", Order = 2)]
		public int StartDate { get; set; }

		/// <summary>
		/// 医療機関コード			
		/// </summary>
		[Column(name: "HP_CD")]
		[MaxLength(7)]
		public string HpCd { get; set; } = string.Empty;

		/// <summary>
		/// 労災医療機関コード			
		/// </summary>
		[Column(name: "ROUSAI_HP_CD")]
		[MaxLength(7)]
		public string RousaiHpCd { get; set; } = string.Empty;

		/// <summary>
		/// 医療機関名			
		/// </summary>
		[Column(name: "HP_NAME")]
		[MaxLength(80)]
		public string HpName { get; set; } = string.Empty;

		/// <summary>
		/// レセ医療機関名			
		/// </summary>
		[Column(name: "RECE_HP_NAME")]
		[MaxLength(80)]
		public string ReceHpName { get; set; } = string.Empty;

		/// <summary>
		/// 開設者氏名			
		/// </summary>
		[Column(name: "KAISETU_NAME")]
		[MaxLength(40)]
		public string KaisetuName { get; set; } = string.Empty;

		/// <summary>
		/// 郵便番号			
		/// </summary>
		[Column(name: "POST_CD")]
		[MaxLength(7)]
		public string PostCd { get; set; } = string.Empty;

		/// <summary>
		/// 都道府県番号			
		/// </summary>
		[Column(name: "PREF_NO")]
		public int PrefNo { get; set; }

		/// <summary>
		/// 医療機関所在地１			
		/// </summary>
		[Column(name: "ADDRESS1")]
		[MaxLength(100)]
		public string Address1 { get; set; } = string.Empty;

		/// <summary>
		/// 医療機関所在地２			
		/// </summary>
		[Column(name: "ADDRESS2")]
		[MaxLength(100)]
		public string Address2 { get; set; } = string.Empty;

		/// <summary>
		/// 電話番号			
		/// </summary>
		[Column(name: "TEL")]
		[MaxLength(15)]
		public string Tel { get; set; } = string.Empty;

		/// <summary>
		/// FAX番号			
		/// </summary>
		[Column(name: "FAX_NO")]
        [MaxLength(15)]
        public string FaxNo { get; set; } = string.Empty;

		/// <summary>
		/// その他連絡先
		/// </summary>
		[Column(name: "OTHER_CONTACTS")]
        [MaxLength(100)]
        public string OtherContacts { get; set; } = string.Empty;

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
		public string? UpdateMachine { get; set; }  = string.Empty;
	}
}