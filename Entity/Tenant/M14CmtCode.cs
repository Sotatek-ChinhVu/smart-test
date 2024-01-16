using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m14_cmt_code")]
    public class M14CmtCode : EmrCloneable<M14CmtCode>
    {
        /// <summary>
        /// 注意コメントコード
        /// 
        /// </summary>
        
        [Column("attention_cmt_cd", Order = 1)]
        [MaxLength(7)]
        public string AttentionCmtCd { get; set; } = string.Empty;

        /// <summary>
        /// 注意コメント
        /// 
        /// </summary>
        [Column("attention_cmt")]
        [MaxLength(500)]
        public string? AttentionCmt { get; set; } = string.Empty;

    }
}
