using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_AR_CODE")]
    public class M34ArCode : EmrCloneable<M34ArCode>
    {
        /// <summary>
        /// 副作用コード
        /// 
        /// </summary>
        [Key]
        [Column("FUKUSAYO_CD", Order = 1)]
        public string FukusayoCd { get; set; } = string.Empty;

        /// <summary>
        /// 副作用コメント
        /// 
        /// </summary>
        [Column("FUKUSAYO_CMT")]
        [MaxLength(200)]
        public string? FukusayoCmt { get; set; } = string.Empty;

    }
}
