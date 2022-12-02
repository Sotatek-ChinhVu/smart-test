using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// ユーザー設定
	/// </summary>
	[Table(name: "USER_CONF")]
    public class UserConf : EmrCloneable<UserConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// ユーザーID
        ///     USER_MST.USER_ID
        /// </summary>
        
        [Column("USER_ID", Order = 2)]
        public int UserId { get; set; }

        /// <summary>
        /// 分類コード
        /// </summary>
        
        [Column("GRP_CD", Order = 3)]
        public int GrpCd { get; set; }

        /// <summary>
        /// 分類項目コード
        /// </summary>
        
        [Column("GRP_ITEM_CD", Order = 4)]
        public int GrpItemCd { get; set; }

        /// <summary>
        /// 分類項目枝番
        /// </summary>
        
        [Column("GRP_ITEM_EDA_NO", Order = 5)]
        public int GrpItemEdaNo { get; set; }

        /// <summary>
        /// 設定値
        /// </summary>
        [Column(name: "VAL")]
        public int Val { get; set; }

        /// <summary>
        /// パラメータ
        /// </summary>
        [Column(name: "PARAM")]
        [MaxLength(300)]
        public string? Param { get; set; } = string.Empty;

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
