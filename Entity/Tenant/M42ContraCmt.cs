using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m42_contra_cmt")]
    public class M42ContraCmt : EmrCloneable<M42ContraCmt>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// コメントコード
        /// CM または KJ で始まり5桁の数字が続く
        /// </summary>

        [Column("cmt_cd", Order = 1)]
        [MaxLength(7)]
        public string CmtCd { get; set; } = string.Empty;

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("cmt")]
        [MaxLength(400)]
        public string? Cmt { get; set; } = string.Empty;

    }
}
