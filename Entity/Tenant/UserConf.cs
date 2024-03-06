using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// ユーザー設定
	/// </summary>
	[Table(name: "user_conf")]
    public class UserConf : EmrCloneable<UserConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// ユーザーID
        ///     USER_MST.USER_ID
        /// </summary>
        
        [Column("user_id", Order = 2)]
        public int UserId { get; set; }

        /// <summary>
        /// 分類コード
        /// </summary>
        
        [Column("grp_cd", Order = 3)]
        public int GrpCd { get; set; }

        /// <summary>
        /// 分類項目コード
        /// </summary>
        
        [Column("grp_item_cd", Order = 4)]
        public int GrpItemCd { get; set; }

        /// <summary>
        /// 分類項目枝番
        /// </summary>
        
        [Column("grp_item_eda_no", Order = 5)]
        public int GrpItemEdaNo { get; set; }

        /// <summary>
        /// 設定値
        /// </summary>
        [Column(name: "val")]
        public int Val { get; set; }

        /// <summary>
        /// パラメータ
        /// </summary>
        [Column(name: "param")]
        [MaxLength(300)]
        public string? Param { get; set; } = string.Empty;

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
