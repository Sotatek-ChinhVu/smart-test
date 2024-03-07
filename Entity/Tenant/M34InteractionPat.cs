using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m34_interaction_pat")]
    public class M34InteractionPat : EmrCloneable<M34InteractionPat>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>

        [Column("yj_cd", Order = 1)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 2)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 相互作用コード
        /// 
        /// </summary>
        [Column("interaction_pat_cd")]
        public string? InteractionPatCd { get; set; } = string.Empty;

    }
}