using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m38_ing_code")]
    public class M38IngCode : EmrCloneable<M38IngCode>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 成分コード
        /// 英数字7桁
        /// </summary>

        [Column("seibun_cd", Order = 1)]
        [MaxLength(7)]
        public string SeibunCd { get; set; } = string.Empty;

        /// <summary>
        /// 成分
        /// 
        /// </summary>
        [Column("seibun")]
        [MaxLength(200)]
        public string? Seibun { get; set; } = string.Empty;
    }
}
