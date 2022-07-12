using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "USER_PERMISSION")]
    public class UserPermission : EmrCloneable<UserPermission>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// ユーザーID
        /// 
        /// </summary>
        //[Key]
        [Column("USER_ID", Order = 2)]
        public int UserId { get; set; }

        /// <summary>
        /// 機能コード
        /// FUNCTION_MST.FUNCTION_CD
        /// </summary>
        //[Key]
        [Column("FUNCTION_CD", Order = 3)]
        [MaxLength(8)]
        public string FunctionCd { get; set; } = string.Empty;

        /// <summary>
        /// 許可区分
        /// "0: 制限なし
        /// 1: 参照権限
        /// 99:使用不可"
        /// </summary>
        [Column("PERMISSION")]
        [CustomAttribute.DefaultValue(0)]
        public int Permission { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }  = string.Empty;

    }
}
