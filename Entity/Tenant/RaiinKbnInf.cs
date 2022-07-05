using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 来院区分情報
	/// </summary>
	[Table("RAIIN_KBN_INF")]
	[Index(nameof(HpId), nameof(PtId), nameof(SinDate), nameof(RaiinNo), nameof(GrpId), nameof(IsDelete), Name = "RAIIN_KBN_INF_IDX01")]
	public class RaiinKbnInf : EmrCloneable<RaiinKbnInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        [Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        [Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        [Key]
        [Column("RAIIN_NO", Order = 3)]
		public long RaiinNo { get; set; }

        /// <summary>
        /// コメント区分
        /// </summary>
        [Key]
        [Column("GRP_ID", Order = 4)]
		public int GrpId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Key]
        [Column("SEQ_NO", Order = 5)]
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