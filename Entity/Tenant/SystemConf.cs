using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// システム設定
	/// </summary>
	[Table("SYSTEM_CONF")]
    public class SystemConf : EmrCloneable<SystemConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 分類コード
        /// </summary>
        //[Key]
        [Column("GRP_CD", Order = 2)]
        public int GrpCd { get; set; }

        /// <summary>
        /// 分類枝番
        /// </summary>
        //[Key]
        [Column("GRP_EDA_NO", Order = 3)]
        public int GrpEdaNo { get; set; }

        /// <summary>
        /// 設定値
        /// </summary>
        [Column("VAL")]
        public double Val { get; set; }

        /// <summary>
        /// パラメーター
        /// </summary>
        [Column("PARAM")]
        [MaxLength(300)]
		public string Param { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("BIKO")]
        [MaxLength(200)]
		public string Biko { get; set; }

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