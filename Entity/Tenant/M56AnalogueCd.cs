using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m56_analogue_cd")]
    public class M56AnalogueCd : EmrCloneable<M56AnalogueCd>
    {
        /// <summary>
        /// 類似成分コード
        /// 
        /// </summary>
        
        [Column("analogue_cd", Order = 1)]
        [MaxLength(9)]
        public string AnalogueCd { get; set; } = string.Empty;

        /// <summary>
        /// 類似成分名
        /// 
        /// </summary>
        [Column("analogue_name")]
        [MaxLength(200)]
        public string? AnalogueName { get; set; } = string.Empty;
    }
}
