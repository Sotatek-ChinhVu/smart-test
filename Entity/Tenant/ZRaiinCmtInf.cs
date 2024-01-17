using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 来院コメント情報
	/// </summary>
	[Table(name: "z_raiin_cmt_inf")]
	public class ZRaiinCmtInf : EmrCloneable<ZRaiinCmtInf>
	{
        
        [Column("op_id", Order = 1)]
		public long OpId { get; set; }

		[Column("op_type")]
		[MaxLength(10)]
		public string? OpType { get; set; } = string.Empty;

		[Column("op_time")]
		public DateTime OpTime { get; set; }

		[Column("op_addr")]
		[MaxLength(100)]
		public string? OpAddr { get; set; } = string.Empty;

		[Column("op_hostname")]
		[MaxLength(100)]
		public string? OpHostName { get; set; } = string.Empty;

		/// <summary>
		/// 医療機関識別ID
		/// </summary>
		[Column("hp_id")]
		//[Index("raiin_cmt_inf_idx01", 1)]
		public int HpId { get; set; }

		/// <summary>
		/// 来院番号
		/// </summary>
		[Column("raiin_no")]
		//[Index("raiin_cmt_inf_idx01", 2)]
		public long RaiinNo { get; set; }

		/// <summary>
		/// コメント区分
		///		1:来院コメント 
		///		9:備考			
		/// </summary>
		[Column("cmt_kbn")]
		//[Index("raiin_cmt_inf_idx01", 3)]
		public int CmtKbn { get; set; }

		/// <summary>
		/// 連番
		/// </summary>
		[Column("seq_no")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long SeqNo { get; set; }

		/// <summary>
		/// 患者ID
		///		患者を識別するためのシステム固有の番号						
		/// </summary>
		[Column("pt_id")]
		public long PtId { get; set; }

		/// <summary>
		/// 診療日
		///		yyyymmdd	
		/// </summary>
		[Column("sin_date")]
		public int SinDate { get; set; }

		/// <summary>
		/// テキスト
		/// </summary>
		[MaxLength(200)]
		[Column("text")]
		public string? Text { get; set; } = string.Empty;

		/// <summary>
		/// 削除区分
		///		1:削除
		/// </summary>
		[Column("is_delete")]
		//[Index("raiin_cmt_inf_idx01", 4)]
		[CustomAttribute.DefaultValue(0)]
		public int IsDelete { get; set; }

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