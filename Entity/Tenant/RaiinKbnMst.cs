using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 来院区分情報
	/// </summary>
	[Table(name: "RAIIN_KBN_MST")]
	[Index(nameof(HpId), nameof(GrpCd), nameof(IsDeleted), Name = "RAIIN_KBN_MST_IDX01")]
	public class RaiinKbnMst : EmrCloneable<RaiinKbnMst>
    {
        /// <summary>
        ///医療機関識別ID
        /// </summary>
        //[Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類ID
        /// </summary>
        //[Key]
        [Column("GRP_ID", Order = 2)]
        public int GrpCd { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column(name: "SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 分類名称
        /// </summary>
        [Column("GRP_NAME")]
        [MaxLength(20)]
        public string GrpName { get; set; } = string.Empty;

		/// <summary>
		/// 削除区分
		///		1:削除
		/// </summary>
		[Column("IS_DELETED")]
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
		public string? UpdateMachine { get; set; } = string.Empty;
	}
}