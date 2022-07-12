using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_INTERACTION_PAT_CODE")]
    public class M34InteractionPatCode : EmrCloneable<M34InteractionPatCode>
    {
        /// <summary>
        /// 相互作用コード
        /// 
        /// </summary>
        [Key]
        [Column("INTERACTION_PAT_CD", Order = 1)]
        public string InteractionPatCd { get; set; } = string.Empty;

        /// <summary>
        /// 相互作用コメント
        /// 
        /// </summary>
        [Column("INTERACTION_PAT_CMT")]
        [MaxLength(200)]
        public string InteractionPatCmt { get; set; } = string.Empty;

    }
}
