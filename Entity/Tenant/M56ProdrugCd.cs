using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m56_prodrug_cd")]
    public class M56ProdrugCd : EmrCloneable<M56ProdrugCd>
    {
        /// <summary>
        /// 成分コード
        /// 
        /// </summary>
        
        [Column("seibun_cd", Order = 1)]
        [MaxLength(9)]
        public string SeibunCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 2)]
        [MaxLength(2)]
        public string SeqNo { get; set; } = string.Empty;

        /// <summary>
        /// 活性体成分コード
        /// 
        /// </summary>
        [Column("kasseitai_cd")]
        [MaxLength(9)]
        public string? KasseitaiCd { get; set; } = string.Empty;

    }
}
