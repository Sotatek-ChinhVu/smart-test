using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m38_class_code")]
    public class M38ClassCode : EmrCloneable<M38ClassCode>
    {
        /// <summary>
        /// 分類コード
        /// 数字2桁
        /// </summary>
        
        [Column("class_cd", Order = 1)]
        public string ClassCd { get; set; } = string.Empty;

        /// <summary>
        /// 分類名
        /// 
        /// </summary>
        [Column("class_name")]
        [MaxLength(100)]
        public string? ClassName { get; set; } = string.Empty;

        /// <summary>
        /// 大分類コード
        /// 数字1桁
        /// </summary>
        [Column("major_div_cd")]
        public string? MajorDivCd { get; set; } = string.Empty;
    }
}