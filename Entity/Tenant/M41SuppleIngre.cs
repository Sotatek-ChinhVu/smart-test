using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m41_supple_ingre")]
    public class M41SuppleIngre : EmrCloneable<M41SuppleIngre>
    {
        /// <summary>
        /// サプリメント成分コード
        /// 9Sで始まり、5桁の数字が続く
        /// </summary>
        
        [Column("seibun_cd", Order = 1)]
        [MaxLength(7)]
        public string SeibunCd { get; set; } = string.Empty;

        /// <summary>
        /// サプリメント成分代表名
        /// 
        /// </summary>
        [Column("seibun")]
        [MaxLength(200)]
        public string? Seibun { get; set; } = string.Empty;
    }
}
