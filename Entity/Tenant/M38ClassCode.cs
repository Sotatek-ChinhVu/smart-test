using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M38_CLASS_CODE")]
    public class M38ClassCode : EmrCloneable<M38ClassCode>
    {
        /// <summary>
        /// 分類コード
        /// 数字2桁
        /// </summary>
        
        [Column("CLASS_CD", Order = 1)]
        public string ClassCd { get; set; } = string.Empty;

        /// <summary>
        /// 分類名
        /// 
        /// </summary>
        [Column("CLASS_NAME")]
        [MaxLength(100)]
        public string? ClassName { get; set; } = string.Empty;

        /// <summary>
        /// 大分類コード
        /// 数字1桁
        /// </summary>
        [Column("MAJOR_DIV_CD")]
        public string? MajorDivCd { get; set; } = string.Empty;
    }
}