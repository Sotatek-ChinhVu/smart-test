using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m34_indication_code")]
    public class M34IndicationCode : EmrCloneable<M34IndicationCode>
    {
        /// <summary>
        /// 効能効果コード
        /// 
        /// </summary>
        
        [Column("kono_cd", Order = 1)]
        public string KonoCd { get; set; } = string.Empty;

        /// <summary>
        /// 効能効果（詳しい説明）
        /// 
        /// </summary>
        [Column("kono_detail_cmt")]
        [MaxLength(200)]
        public string? KonoDetailCmt { get; set; } = string.Empty;

        /// <summary>
        /// 効能効果（簡単な説明）
        /// 
        /// </summary>
        [Column("kono_simple_cmt")]
        [MaxLength(200)]
        public string? KonoSimpleCmt { get; set; } = string.Empty;

    }
}
