using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 特記事項マスタ
	/// </summary>
	[Table(name: "TOKKI_MST")]
    public class TokkiMst : EmrCloneable<TokkiMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        //[Index("TOKKI_MST_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 特記事項コード
        /// </summary>
        //[Key]
        [Column(name: "TOKKI_CD", Order = 2)]
        //[Index("TOKKI_MST_IDX01", 2)]
        [MaxLength(2)]
        public string TokkiCd { get; set; }

        /// <summary>
        /// 特記事項名
        /// </summary>
        [Column(name: "TOKKI_NAME")]
        [MaxLength(20)]
        [Required]
        public string TokkiName { get; set; }

        /// <summary>
        /// 使用開始日
        /// </summary>
        [Column(name: "START_DATE")]
        //[Index("TOKKI_MST_IDX01", 3)]
        public int StartDate { get; set; }

        /// <summary>
        /// 使用終了日
        /// </summary>
        [Column(name: "END_DATE")]
        //[Index("TOKKI_MST_IDX01", 4)]
        public int EndDate { get; set; }

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