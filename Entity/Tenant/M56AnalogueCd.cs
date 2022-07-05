using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M56_ANALOGUE_CD")]
    public class M56AnalogueCd : EmrCloneable<M56AnalogueCd>
    {
        /// <summary>
        /// 類似成分コード
        /// 
        /// </summary>
        [Key]
        [Column("ANALOGUE_CD", Order = 1)]
        [MaxLength(9)]
        public string AnalogueCd { get; set; }

        /// <summary>
        /// 類似成分名
        /// 
        /// </summary>
        [Column("ANALOGUE_NAME")]
        [MaxLength(200)]
        public string AnalogueName { get; set; }

    }
}
