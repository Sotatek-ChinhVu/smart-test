using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M56_EX_ING_CODE")]
    public class M56ExIngCode : EmrCloneable<M56ExIngCode>
    {
        /// <summary>
        /// 成分コード
        /// 
        /// </summary>
        [Key]
        [Column("SEIBUN_CD", Order = 1)]
        [MaxLength(9)]
        public string SeibunCd { get; set; } = string.Empty;

        /// <summary>
        /// 成分索引コード
        /// 
        /// </summary>
        //[Key]
        [Column("SEIBUN_INDEX_CD", Order = 2)]
        [MaxLength(3)]
        public string SeibunIndexCd { get; set; } = string.Empty;

        /// <summary>
        /// 成分名
        /// 
        /// </summary>
        [Column("SEIBUN_NAME")]
        [MaxLength(200)]
        public string SeibunName { get; set; } = string.Empty;

        /// <summary>
        /// 用法コード
        /// 
        /// </summary>
        [Column("YOHO_CD")]
        [MaxLength(6)]
        public string YohoCd { get; set; } = string.Empty;

    }
}
