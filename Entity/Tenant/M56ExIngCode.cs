using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m56_ex_ing_code")]
    public class M56ExIngCode : EmrCloneable<M56ExIngCode>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 成分コード
        /// 
        /// </summary>

        [Column("seibun_cd", Order = 1)]
        [MaxLength(9)]
        public string SeibunCd { get; set; } = string.Empty;

        /// <summary>
        /// 成分索引コード
        /// 
        /// </summary>
        
        [Column("seibun_index_cd", Order = 2)]
        [MaxLength(3)]
        public string SeibunIndexCd { get; set; } = string.Empty;

        /// <summary>
        /// 成分名
        /// 
        /// </summary>
        [Column("seibun_name")]
        [MaxLength(200)]
        public string? SeibunName { get; set; } = string.Empty;

        /// <summary>
        /// 用法コード
        /// 
        /// </summary>
        [Column("yoho_cd")]
        [MaxLength(6)]
        public string? YohoCd { get; set; } = string.Empty;

    }
}
