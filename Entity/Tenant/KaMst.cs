using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 診療科マスタ
	/// </summary>
	[Table(name: "KA_MST")]
    [Index(nameof(KaId), Name = "PT_KA_MST_IDX01")]
    public class KaMst : EmrCloneable<KaMst>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [Column(name: "ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        //[Key]
        [Column(name: "HP_ID", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 診療科ID
        /// </summary>
       
        [Column("KA_ID")]
        public int KaId { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// レセ診療科コード
        /// </summary>
        [Column(name: "RECE_KA_CD")]
        [MaxLength(2)]
        public string ReceKaCd { get; set; } = string.Empty;

        /// <summary>
        /// 診療科略称
        /// </summary>
        [Column(name: "KA_SNAME")]
        [MaxLength(20)]
        public string KaSname { get; set; } = string.Empty;

        /// <summary>
        /// 診療科名称
        /// </summary>
        [Column(name: "KA_NAME")]
        [MaxLength(40)]
        public string KaName { get; set; } = string.Empty;

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
		public string? CreateMachine { get; set; }

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