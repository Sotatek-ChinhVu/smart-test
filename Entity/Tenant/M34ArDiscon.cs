using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_AR_DISCON")]
    public class M34ArDiscon : EmrCloneable<M34ArDiscon>
    {
        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>
        
        [Column("YJ_CD", Order = 1)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("SEQ_NO", Order = 2)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 副作用コード
        /// 
        /// </summary>
        [Column("FUKUSAYO_CD")]
        public string? FukusayoCd { get; set; } = string.Empty;

    }
}
