using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "releasenote_read")]
    public class ReleasenoteRead : EmrCloneable<ReleasenoteRead>
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
        /// バージョン
        /// 
        /// </summary>
        
        [Column("version", Order = 3)]
        [MaxLength(10)]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;
    }
}
