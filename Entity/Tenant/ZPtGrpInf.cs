using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 患者分類情報
	/// </summary>
	[Table(name: "z_pt_grp_inf")]
	public class ZPtGrpInf : EmrCloneable<ZPtGrpInf>
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
		[Column(name: "hp_id")]
		//[Index("pt_grp_inf_idx01", 1)]
		public int HpId { get; set; }

		/// <summary>
		/// 患者ID
		///		患者を識別するためのシステム固有の番号
		/// </summary>
		[Column(name: "pt_id")]
		//[Index("pt_grp_inf_idx01", 2)]
		public long PtId { get; set; }

		/// <summary>
		/// 分類番号
		/// </summary>
		[Column(name: "grp_id")]
		//[Index("pt_grp_inf_idx01", 3)]
		public int GroupId { get; set; }

		/// <summary>
		/// 連番
		/// </summary>
		[Column(name: "seq_no")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long SeqNo { get; set; }

		/// <summary>
		/// 並び順
		/// </summary>
		[Column(name: "sort_no")]
		[CustomAttribute.DefaultValue(1)]
		public int SortNo { get; set; }

		/// <summary>
		/// 分類項目コード
		/// </summary>
		[Column(name: "grp_code")]
		[MaxLength(4)]
		public string? GroupCode { get; set; } = string.Empty;

		/// <summary>
		/// 削除区分
		/// </summary>
		[Column(name: "is_deleted")]
		//[Index("pt_grp_inf_idx01", 4)]
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
		public string? UpdateMachine { get; set; }  = string.Empty;
	}
}