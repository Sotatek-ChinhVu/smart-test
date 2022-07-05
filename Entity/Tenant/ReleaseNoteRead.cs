using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RELEASENOTE_READ")]
    public class ReleasenoteRead : EmrCloneable<ReleasenoteRead>
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
        [Key]
        [Column("USER_ID", Order = 2)]
        public int UserId { get; set; }

        /// <summary>
        /// バージョン
        /// 
        /// </summary>
        [Key]
        [Column("VERSION", Order = 3)]
        [MaxLength(10)]
        public string Version { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }
    }
}
