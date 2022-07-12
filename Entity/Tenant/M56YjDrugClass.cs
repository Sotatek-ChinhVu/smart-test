using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M56_YJ_DRUG_CLASS")]
    public class M56YjDrugClass : EmrCloneable<M56YjDrugClass>
    {
        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>
        [Key]
        [Column("YJ_CD", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 系統コード
        /// 
        /// </summary>
        //[Key]
        [Column("CLASS_CD", Order = 2)]
        [MaxLength(8)]
        public string ClassCd { get; set; } = string.Empty;

    }
}
