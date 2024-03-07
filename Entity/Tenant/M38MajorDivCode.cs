using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m38_major_div_code")]
    public class M38MajorDivCode : EmrCloneable<M38MajorDivCode>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 大分類コード
        /// 数字1桁
        /// </summary>

        [Column("major_div_cd", Order = 1)]
        public string MajorDivCd { get; set; } = string.Empty;

        /// <summary>
        /// 大分類名
        /// 
        /// </summary>
        [Column("major_div_name")]
        [MaxLength(100)]
        public string? MajorDivName { get; set; } = string.Empty;
    }
}