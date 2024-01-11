using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m34_precautions")]
    public class M34Precaution : EmrCloneable<M34Precaution>
    {
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
        /// 注意事項コード
        /// 
        /// </summary>
        [Column("precaution_cd")]
        public string? PrecautionCd { get; set; } = string.Empty;

    }
}
