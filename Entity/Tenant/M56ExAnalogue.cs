using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m56_ex_analogue")]
    public class M56ExAnalogue : EmrCloneable<M56ExAnalogue>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

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
        /// 類似成分コード
        /// 
        /// </summary>
        [Column("analogue_cd")]
        [MaxLength(9)]
        public string? AnalogueCd { get; set; } = string.Empty;
    }
}
