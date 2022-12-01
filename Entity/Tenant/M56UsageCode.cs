using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M56_USAGE_CODE")]
    public class M56UsageCode : EmrCloneable<M56UsageCode>
    {
        /// <summary>
        /// 用法コード
        /// 
        /// </summary>
        [Key]
        [Column("YOHO_CD", Order = 1)]
        public string YohoCd { get; set; } = string.Empty;

        /// <summary>
        /// 用法
        /// 
        /// </summary>
        [Column("YOHO")]
        [MaxLength(200)]
        public string? Yoho { get; set; } = string.Empty;
    }
}
