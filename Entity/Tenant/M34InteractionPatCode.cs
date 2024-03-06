using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m34_interaction_pat_code")]
    public class M34InteractionPatCode : EmrCloneable<M34InteractionPatCode>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 相互作用コード
        /// 
        /// </summary>

        [Column("interaction_pat_cd", Order = 1)]
        public string InteractionPatCd { get; set; } = string.Empty;

        /// <summary>
        /// 相互作用コメント
        /// 
        /// </summary>
        [Column("interaction_pat_cmt")]
        [MaxLength(200)]
        public string? InteractionPatCmt { get; set; } = string.Empty;

    }
}
