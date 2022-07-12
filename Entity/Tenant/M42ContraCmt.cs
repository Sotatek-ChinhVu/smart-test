using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M42_CONTRA_CMT")]
    public class M42ContraCmt : EmrCloneable<M42ContraCmt>
    {
        /// <summary>
        /// コメントコード
        /// CM または KJ で始まり5桁の数字が続く
        /// </summary>
        [Key]
        [Column("CMT_CD", Order = 1)]
        [MaxLength(7)]
        public string CmtCd { get; set; } = string.Empty;

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        [Column("CMT")]
        [MaxLength(400)]
        public string Cmt { get; set; } = string.Empty;

    }
}
