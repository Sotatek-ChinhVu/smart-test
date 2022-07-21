using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "YOHO_SET_MST")]
    public class YohoSetMst : EmrCloneable<YohoSetMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("HP_ID")]
        //[Index("YOHO_SET_MST_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// セットID
        /// 
        /// </summary>
        [Key]
        [Column("SET_ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SetId { get; set; }

        /// <summary>
        /// ユーザーID
        /// USER_MST.USER_ID
        /// </summary>
        [Column("USER_ID")]
        //[Index("YOHO_SET_MST_IDX01", 2)]
        public int UserId { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 項目コード
        /// TEN_MST.ITEM_CD
        /// </summary>
        [Column("ITEM_CD")]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("IS_DELETED")]
        //[Index("YOHO_SET_MST_IDX01", 3)]
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
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
