using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m34_ar_code")]
    public class M34ArCode : EmrCloneable<M34ArCode>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 副作用コード
        /// 
        /// </summary>

        [Column("fukusayo_cd", Order = 1)]
        public string FukusayoCd { get; set; } = string.Empty;

        /// <summary>
        /// 副作用コメント
        /// 
        /// </summary>
        [Column("fukusayo_cmt")]
        [MaxLength(200)]
        public string? FukusayoCmt { get; set; } = string.Empty;

    }
}
