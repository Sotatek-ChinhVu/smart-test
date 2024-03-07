using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m34_ar_discon")]
    public class M34ArDiscon : EmrCloneable<M34ArDiscon>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>

        [Column("yj_cd", Order = 1)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 2)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 副作用コード
        /// 
        /// </summary>
        [Column("fukusayo_cd")]
        public string? FukusayoCd { get; set; } = string.Empty;

    }
}
