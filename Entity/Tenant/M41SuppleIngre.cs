using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M41_SUPPLE_INGRE")]
    public class M41SuppleIngre : EmrCloneable<M41SuppleIngre>
    {
        /// <summary>
        /// サプリメント成分コード
        /// 9Sで始まり、5桁の数字が続く
        /// </summary>
        
        [Column("SEIBUN_CD", Order = 1)]
        [MaxLength(7)]
        public string SeibunCd { get; set; } = string.Empty;

        /// <summary>
        /// サプリメント成分代表名
        /// 
        /// </summary>
        [Column("SEIBUN")]
        [MaxLength(200)]
        public string? Seibun { get; set; } = string.Empty;
    }
}
