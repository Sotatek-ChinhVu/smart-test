using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M01_KINKI")]
    public class M01Kinki : EmrCloneable<M01Kinki>
    {
        /// <summary>
        /// Aコード
        /// YJコードの上位7,8,9,12桁
        /// </summary>
        [Key]
        [Column("A_CD", Order = 1)]
        [MaxLength(12)]
        public string ACd { get; set; } = string.Empty;

        /// <summary>
        /// Bコード
        /// YJコードの上位4,7,8,9,12桁 or H or Zで始まり、4桁の数字が続く
        /// </summary>
        //[Key]
        [Column("B_CD", Order = 2)]
        [MaxLength(12)]
        public string BCd { get; set; } = string.Empty;

        /// <summary>
        /// コメントコード
        /// 
        /// </summary>
        //[Key]
        [Column("CMT_CD", Order = 3)]
        [MaxLength(6)]
        public string CmtCd { get; set; } = string.Empty;

        /// <summary>
        /// 作用機序コード
        /// 
        /// </summary>
        //[Key]
        [Column("SAYOKIJYO_CD", Order = 4)]
        [MaxLength(6)]
        public string SayokijyoCd { get; set; } = string.Empty;

        /// <summary>
        /// 強度コード
        /// 
        /// </summary>
        [Column("KYODO_CD")]
        [MaxLength(3)]
        public string KyodoCd { get; set; } = string.Empty;

        /// <summary>
        /// 強度
        /// 
        /// </summary>
        [Column("KYODO")]
        [MaxLength(2)]
        public string Kyodo { get; set; } = string.Empty;

        /// <summary>
        /// データ区分
        /// 
        /// </summary>
        [Column("DATA_KBN")]
        [MaxLength(1)]
        public string DataKbn { get; set; } = string.Empty;

    }
}
