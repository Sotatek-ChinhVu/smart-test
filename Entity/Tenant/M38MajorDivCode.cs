using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M38_MAJOR_DIV_CODE")]
    public class M38MajorDivCode : EmrCloneable<M38MajorDivCode>
    {
        /// <summary>
        /// 大分類コード
        /// 数字1桁
        /// </summary>
        
        [Column("MAJOR_DIV_CD", Order = 1)]
        public string MajorDivCd { get; set; } = string.Empty;

        /// <summary>
        /// 大分類名
        /// 
        /// </summary>
        [Column("MAJOR_DIV_NAME")]
        [MaxLength(100)]
        public string? MajorDivName { get; set; } = string.Empty;
    }
}