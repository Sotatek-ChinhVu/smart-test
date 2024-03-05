using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m56_alrgy_derivatives")]
    public class M56AlrgyDerivatives : EmrCloneable<M56AlrgyDerivatives>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>

        [Column("yj_cd", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// アレルギー関連系統コード
        /// 
        /// </summary>
        
        [Column("drvalrgy_cd", Order = 2)]
        [MaxLength(8)]
        public string DrvalrgyCd { get; set; } = string.Empty;

        /// <summary>
        /// 成分コード
        /// 
        /// </summary>
        
        [Column("seibun_cd", Order = 3)]
        [MaxLength(9)]
        public string SeibunCd { get; set; } = string.Empty;
    }
}
