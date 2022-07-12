using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 来院コメント情報
	/// </summary>
	[Table(name: "RAIIN_CMT_INF")]
	[Index(nameof(HpId), nameof(RaiinNo), nameof(CmtKbn), nameof(IsDelete), Name = "RAIIN_CMT_INF_IDX01")]
	public class RaiinCmtInf : EmrCloneable<RaiinCmtInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

		/// <summary>
		/// 患者ID
		///		患者を識別するためのシステム固有の番号						
		/// </summary>
		[Column("PT_ID")]
        public long PtId { get; set; }

		/// <summary>
		/// 診療日
		///		yyyymmdd	
		/// </summary>
		[Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        //[Key]
        [Column("RAIIN_NO", Order = 2)]
		public long RaiinNo { get; set; }

		/// <summary>
		/// コメント区分
		///		1:来院コメント 
		///		9:備考			
		/// </summary>
		//[Key]
        [Column("CMT_KBN", Order = 3)]
		public int CmtKbn { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long SeqNo { get; set; }

        /// <summary>
        /// テキスト
        /// </summary>
        [MaxLength(200)]
        [Column("TEXT")]
        public string Text { get; set; } = string.Empty;

		/// <summary>
		/// 削除区分
		///		1:削除
		/// </summary>
		[Column("IS_DELETE")]
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
		public string CreateMachine { get; set; } = string.Empty;

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
		public string UpdateMachine { get; set; }  = string.Empty;
	}
}