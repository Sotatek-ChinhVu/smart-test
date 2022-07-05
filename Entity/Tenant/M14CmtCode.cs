using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M14_CMT_CODE")]
    public class M14CmtCode : EmrCloneable<M14CmtCode>
    {
        /// <summary>
        /// 注意コメントコード
        /// 
        /// </summary>
        [Key]
        [Column("ATTENTION_CMT_CD", Order = 1)]
        [MaxLength(7)]
        public string AttentionCmtCd { get; set; }

        /// <summary>
        /// 注意コメント
        /// 
        /// </summary>
        [Column("ATTENTION_CMT")]
        [MaxLength(500)]
        public string AttentionCmt { get; set; }

    }
}
