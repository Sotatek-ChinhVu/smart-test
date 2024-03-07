using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "user_permission")]
    public class UserPermission : EmrCloneable<UserPermission>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// ユーザーID
        /// 
        /// </summary>
        
        [Column("user_id", Order = 2)]
        public int UserId { get; set; }

        /// <summary>
        /// 機能コード
        /// FUNCTION_MST.FUNCTION_CD
        /// </summary>
        
        [Column("function_cd", Order = 3)]
        [MaxLength(8)]
        public string FunctionCd { get; set; } = string.Empty;

        /// <summary>
        /// 許可区分
        /// "0: 制限なし
        /// 1: 参照権限
        /// 99:使用不可"
        /// </summary>
        [Column("permission")]
        [CustomAttribute.DefaultValue(0)]
        public int Permission { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
