using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m34_property_code")]
    public class M34PropertyCode : EmrCloneable<M34PropertyCode>
    {
        /// <summary>
        /// 属性コード
        /// 
        /// </summary>
        
        [Column("property_cd", Order = 1)]
        public int PropertyCd { get; set; }

        /// <summary>
        /// 属性
        /// 
        /// </summary>
        [Column("property")]
        [MaxLength(100)]
        public string? Property { get; set; } = string.Empty;

    }
}
