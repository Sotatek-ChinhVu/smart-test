using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 来院区分情報
	/// </summary>
	[Table("Z_RAIIN_KBN_INF")]
	public class ZRaiinKbnInf : EmrCloneable<ZRaiinKbnInf>
	{
        [Key]
        [Column("OP_ID", Order = 1)]
		public long OpId { get; set; }

		[Column("OP_TYPE")]
		[MaxLength(10)]
		public string OpType { get; set; } = string.Empty;

		[Column("OP_TIME")]
		public DateTime OpTime { get; set; }

		[Column("OP_ADDR")]
		[MaxLength(100)]
		public string OpAddr { get; set; } = string.Empty;

		[Column("OP_HOSTNAME")]
		[MaxLength(100)]
		public string OpHostName { get; set; } = string.Empty;

		/// <summary>
		/// 医療機関識別ID
		/// </summary>
		[Column("HP_ID")]
		//[Index("RAIIN_KBN_INF_IDX01", 1)]
		public int HpId { get; set; }

		/// <summary>
		/// 患者ID
		/// </summary>
		[Column("PT_ID")]
		//[Index("RAIIN_KBN_INF_IDX01", 2)]
		public long PtId { get; set; }

		/// <summary>
		/// 診療日
		/// </summary>
		[Column("SIN_DATE")]
		//[Index("RAIIN_KBN_INF_IDX01", 3)]
		public int SinDate { get; set; }

		/// <summary>
		/// 来院番号
		/// </summary>
		[Column("RAIIN_NO")]
		//[Index("RAIIN_KBN_INF_IDX01", 4)]
		public long RaiinNo { get; set; }

		/// <summary>
		/// コメント区分
		/// </summary>
		[Column("GRP_ID")]
		//[Index("RAIIN_KBN_INF_IDX01", 5)]
		public int GrpId { get; set; }

		/// <summary>
		/// 連番
		/// </summary>
		[Column("SEQ_NO")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long SeqNo { get; set; }

		/// <summary>
		/// 区分コード
		/// </summary>
		[Column("KBN_CD")]
		public int KbnCd { get; set; }

		/// <summary>
		/// 削除区分
		///		1:削除
		/// </summary>
		/// 
		[Column("IS_DELETE")]
		//[Index("RAIIN_KBN_INF_IDX01", 6)]
		[CustomAttribute.DefaultValue(0)]
		public int IsDelete { get; set; }

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