using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_AR_DISCON_CODE")]
    public class M34ArDisconCode : EmrCloneable<M34ArDisconCode>
    {
        /// <summary>
        /// 副作用コード
        /// 
        /// </summary>
        [Key]
        [Column("FUKUSAYO_CD", Order = 1)]
        [MaxLength(6)]
        public string FukusayoCd { get; set; } = string.Empty;

        /// <summary>
        /// 副作用コメント
        /// 
        /// </summary>
        [Column("FUKUSAYO_CMT")]
        [MaxLength(200)]
        public string FukusayoCmt { get; set; } = string.Empty;

    }
}
