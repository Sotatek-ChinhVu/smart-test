using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SESSION_INF")]
    public class SessionInf : EmrCloneable<SessionInf>
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
        /// 端末名
        /// 
        /// </summary>
        //[Key]
        [Column("MACHINE", Order = 2)]
        public string Machine { get; set; } = string.Empty;

        /// <summary>
        /// ユーザーID
        /// 
        /// </summary>
        [Column("USER_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UserId { get; set; }

        /// <summary>
        /// ログイン日時
        /// 
        /// </summary>
        [Column("LOGIN_DATE")]
        public DateTime LoginDate { get; set; }
    }
}
