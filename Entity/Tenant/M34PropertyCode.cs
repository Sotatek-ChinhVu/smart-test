using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_PROPERTY_CODE")]
    public class M34PropertyCode : EmrCloneable<M34PropertyCode>
    {
        /// <summary>
        /// 属性コード
        /// 
        /// </summary>
        [Key]
        [Column("PROPERTY_CD", Order = 1)]
        public int PropertyCd { get; set; }

        /// <summary>
        /// 属性
        /// 
        /// </summary>
        [Column("PROPERTY")]
        [MaxLength(100)]
        public string Property { get; set; }

    }
}
