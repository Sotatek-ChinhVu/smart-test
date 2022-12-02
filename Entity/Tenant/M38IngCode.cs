using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M38_ING_CODE")]
    public class M38IngCode : EmrCloneable<M38IngCode>
    {
        /// <summary>
        /// 成分コード
        /// 英数字7桁
        /// </summary>
        
        [Column("SEIBUN_CD", Order = 1)]
        [MaxLength(7)]
        public string SeibunCd { get; set; } = string.Empty;

        /// <summary>
        /// 成分
        /// 
        /// </summary>
        [Column("SEIBUN")]
        [MaxLength(200)]
        public string? Seibun { get; set; } = string.Empty;
    }
}
