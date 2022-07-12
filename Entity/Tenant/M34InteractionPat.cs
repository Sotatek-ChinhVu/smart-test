using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_INTERACTION_PAT")]
    public class M34InteractionPat : EmrCloneable<M34InteractionPat>
    {
        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>
        [Key]
        [Column("YJ_CD", Order = 1)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 2)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 相互作用コード
        /// 
        /// </summary>
        [Column("INTERACTION_PAT_CD")]
        public string InteractionPatCd { get; set; } = string.Empty;

    }
}