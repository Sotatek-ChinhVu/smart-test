using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m01_kinki")]
    public class M01Kinki : EmrCloneable<M01Kinki>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// Aコード
        /// YJコードの上位7,8,9,12桁
        /// </summary>

        [Column("a_cd", Order = 1)]
        [MaxLength(12)]
        public string ACd { get; set; } = string.Empty;

        /// <summary>
        /// Bコード
        /// YJコードの上位4,7,8,9,12桁 or H or Zで始まり、4桁の数字が続く
        /// </summary>
        
        [Column("b_cd", Order = 2)]
        [MaxLength(12)]
        public string BCd { get; set; } = string.Empty;

        /// <summary>
        /// コメントコード
        /// 
        /// </summary>
        
        [Column("cmt_cd", Order = 3)]
        [MaxLength(6)]
        public string CmtCd { get; set; } = string.Empty;

        /// <summary>
        /// 作用機序コード
        /// 
        /// </summary>
        
        [Column("sayokijyo_cd", Order = 4)]
        [MaxLength(6)]
        public string SayokijyoCd { get; set; } = string.Empty;

        /// <summary>
        /// 強度コード
        /// 
        /// </summary>
        [Column("kyodo_cd")]
        [MaxLength(3)]
        public string? KyodoCd { get; set; } = string.Empty;

        /// <summary>
        /// 強度
        /// 
        /// </summary>
        [Column("kyodo")]
        [MaxLength(2)]
        public string? Kyodo { get; set; } = string.Empty;

        /// <summary>
        /// データ区分
        /// 
        /// </summary>
        [Column("data_kbn")]
        [MaxLength(1)]
        public string? DataKbn { get; set; } = string.Empty;

    }
}
