using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M56_ALRGY_DERIVATIVES")]
    public class M56AlrgyDerivatives : EmrCloneable<M56AlrgyDerivatives>
    {
        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>
        [Key]
        [Column("YJ_CD", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// アレルギー関連系統コード
        /// 
        /// </summary>
        //[Key]
        [Column("DRVALRGY_CD", Order = 2)]
        [MaxLength(8)]
        public string DrvalrgyCd { get; set; } = string.Empty;

        /// <summary>
        /// 成分コード
        /// 
        /// </summary>
        //[Key]
        [Column("SEIBUN_CD", Order = 3)]
        [MaxLength(9)]
        public string SeibunCd { get; set; } = string.Empty;

    }
}
