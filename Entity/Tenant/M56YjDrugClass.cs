using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m56_yj_drug_class")]
    public class M56YjDrugClass : EmrCloneable<M56YjDrugClass>
    {
        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>
        
        [Column("yj_cd", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 系統コード
        /// 
        /// </summary>
        
        [Column("class_cd", Order = 2)]
        [MaxLength(8)]
        public string ClassCd { get; set; } = string.Empty;
    }
}
