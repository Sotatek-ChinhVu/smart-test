using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M41_SUPPLE_INDEXCODE")]
    public class M41SuppleIndexcode : EmrCloneable<M41SuppleIndexcode>
    {
        /// <summary>
        /// サプリメント成分コード
        /// 9Sで始まり、5桁の数字が続く
        /// </summary>
        [Key]
        [Column("SEIBUN_CD", Order = 1)]
        [MaxLength(7)]
        public string SeibunCd { get; set; }

        /// <summary>
        /// 索引語コード
        /// Iで始まり、6桁の数字が続く
        /// </summary>
        //[Key]
        [Column("INDEX_CD", Order = 2)]
        [MaxLength(7)]
        public string IndexCd { get; set; }
    }
}

