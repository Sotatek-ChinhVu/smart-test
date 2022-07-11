using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{    
	/// <summary>
	/// 患者分類情報
	/// </summary>
	[Table(name: "PT_GRP_INF")]
	[Index(nameof(HpId), nameof(PtId), nameof(GroupId), nameof(IsDeleted), Name = "PT_GRP_INF_IDX01")]
	public class PtGrpInf : EmrCloneable<PtGrpInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column(name: "HP_ID", Order = 1)]
        public int HpId { get; set; }

		/// <summary>
		/// 患者ID
		///		患者を識別するためのシステム固有の番号
		/// </summary>
		//[Key]
        [Column(name: "PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 分類番号
        /// </summary>
        //[Key]
        [Column(name: "GRP_ID", Order = 3)]
        public int GroupId { get; set; }

		/// <summary>
		/// 連番
		/// </summary>
		//[Key]
		[Column(name: "SEQ_NO", Order = 4)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long SeqNo { get; set; }

		/// <summary>
		/// 並び順
		/// </summary>
		[Column(name: "SORT_NO")]
		[CustomAttribute.DefaultValue(1)]
		public int SortNo { get; set; }

		/// <summary>
		/// 分類項目コード
		/// </summary>
		[Column(name: "GRP_CODE")]
        [MaxLength(4)]
        public string GroupCode { get; set; }

        /// <summary>
        /// 削除区分
        /// </summary>
        [Column(name: "IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
		public int IsDeleted { get; set; }

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
		public string CreateMachine { get; set; }

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
		public string UpdateMachine { get; set; }
	}
}