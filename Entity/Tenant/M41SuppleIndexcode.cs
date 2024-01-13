using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m41_supple_indexcode")]
    public class M41SuppleIndexcode : EmrCloneable<M41SuppleIndexcode>
    {
        /// <summary>
        /// サプリメント成分コード
        /// 9Sで始まり、5桁の数字が続く
        /// </summary>
        
        [Column("seibun_cd", Order = 1)]
        [MaxLength(7)]
        public string SeibunCd { get; set; } = string.Empty;

        /// <summary>
        /// 索引語コード
        /// Iで始まり、6桁の数字が続く
        /// </summary>
        
        [Column("index_cd", Order = 2)]
        [MaxLength(7)]
        public string IndexCd { get; set; } = string.Empty;
    }
}

