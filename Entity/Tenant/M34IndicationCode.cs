using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_INDICATION_CODE")]
    public class M34IndicationCode : EmrCloneable<M34IndicationCode>
    {
        /// <summary>
        /// 効能効果コード
        /// 
        /// </summary>
        
        [Column("KONO_CD", Order = 1)]
        public string KonoCd { get; set; } = string.Empty;

        /// <summary>
        /// 効能効果（詳しい説明）
        /// 
        /// </summary>
        [Column("KONO_DETAIL_CMT")]
        [MaxLength(200)]
        public string? KonoDetailCmt { get; set; } = string.Empty;

        /// <summary>
        /// 効能効果（簡単な説明）
        /// 
        /// </summary>
        [Column("KONO_SIMPLE_CMT")]
        [MaxLength(200)]
        public string? KonoSimpleCmt { get; set; } = string.Empty;

    }
}
