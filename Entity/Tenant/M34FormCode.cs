using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_FORM_CODE")]
    public class M34FormCode : EmrCloneable<M34FormCode>
    {
        /// <summary>
        /// 剤形コード
        /// 
        /// </summary>
        [Key]
        [Column("FORM_CD", Order = 1)]
        [MaxLength(4)]
        public string FormCd { get; set; } = string.Empty;

        /// <summary>
        /// 剤形
        /// 
        /// </summary>
        [Column("FORM")]
        [MaxLength(80)]
        public string Form { get; set; } = string.Empty;

    }
}