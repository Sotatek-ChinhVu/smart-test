using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m34_form_code")]
    public class M34FormCode : EmrCloneable<M34FormCode>
    {
        /// <summary>
        /// 剤形コード
        /// 
        /// </summary>
        
        [Column("form_cd", Order = 1)]
        [MaxLength(4)]
        public string FormCd { get; set; } = string.Empty;

        /// <summary>
        /// 剤形
        /// 
        /// </summary>
        [Column("form")]
        [MaxLength(80)]
        public string? Form { get; set; } = string.Empty;

    }
}