using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "session_inf")]
    public class SessionInf : EmrCloneable<SessionInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 端末名
        /// 
        /// </summary>
        
        [Column("machine", Order = 2)]
        public string Machine { get; set; } = string.Empty;

        /// <summary>
        /// ユーザーID
        /// 
        /// </summary>
        [Column("user_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UserId { get; set; }

        /// <summary>
        /// ログイン日時
        /// 
        /// </summary>
        [Column("login_date")]
        public DateTime LoginDate { get; set; }
    }
}
