using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "yoho_set_mst")]
    public class YohoSetMst : EmrCloneable<YohoSetMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("hp_id")]
        //[Index("yoho_set_mst_idx01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// セットID
        /// 
        /// </summary>
        
        [Column("set_id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SetId { get; set; }

        /// <summary>
        /// ユーザーID
        /// USER_MST.USER_ID
        /// </summary>
        [Column("user_id")]
        //[Index("yoho_set_mst_idx01", 2)]
        public int UserId { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 項目コード
        /// TEN_MST.ITEM_CD
        /// </summary>
        [Column("item_cd")]
        [MaxLength(10)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column("is_deleted")]
        //[Index("yoho_set_mst_idx01", 3)]
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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
