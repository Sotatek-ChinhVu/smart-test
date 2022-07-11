using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M56_PRODRUG_CD")]
    public class M56ProdrugCd : EmrCloneable<M56ProdrugCd>
    {
        /// <summary>
        /// 成分コード
        /// 
        /// </summary>
        [Key]
        [Column("SEIBUN_CD", Order = 1)]
        [MaxLength(9)]
        public string SeibunCd { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 2)]
        [MaxLength(2)]
        public string SeqNo { get; set; }

        /// <summary>
        /// 活性体成分コード
        /// 
        /// </summary>
        [Column("KASSEITAI_CD")]
        [MaxLength(9)]
        public string KasseitaiCd { get; set; }

    }
}
