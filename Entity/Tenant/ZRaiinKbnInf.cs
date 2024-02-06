using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 来院区分情報
	/// </summary>
	[Table("z_raiin_kbn_inf")]
	public class ZRaiinKbnInf : EmrCloneable<ZRaiinKbnInf>
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
		//[Index("raiin_kbn_inf_idx01", 1)]
		public int HpId { get; set; }

		/// <summary>
		/// 患者ID
		/// </summary>
		[Column("pt_id")]
		//[Index("raiin_kbn_inf_idx01", 2)]
		public long PtId { get; set; }

		/// <summary>
		/// 診療日
		/// </summary>
		[Column("sin_date")]
		//[Index("raiin_kbn_inf_idx01", 3)]
		public int SinDate { get; set; }

		/// <summary>
		/// 来院番号
		/// </summary>
		[Column("raiin_no")]
		//[Index("raiin_kbn_inf_idx01", 4)]
		public long RaiinNo { get; set; }

		/// <summary>
		/// コメント区分
		/// </summary>
		[Column("grp_id")]
		//[Index("raiin_kbn_inf_idx01", 5)]
		public int GrpId { get; set; }

		/// <summary>
		/// 連番
		/// </summary>
		[Column("seq_no")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long SeqNo { get; set; }

		/// <summary>
		/// 区分コード
		/// </summary>
		[Column("kbn_cd")]
		public int KbnCd { get; set; }

		/// <summary>
		/// 削除区分
		///		1:削除
		/// </summary>
		/// 
		[Column("is_delete")]
		//[Index("raiin_kbn_inf_idx01", 6)]
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