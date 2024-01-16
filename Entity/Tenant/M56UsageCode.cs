using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m56_usage_code")]
    public class M56UsageCode : EmrCloneable<M56UsageCode>
    {
        /// <summary>
        /// 用法コード
        /// 
        /// </summary>
        
        [Column("yoho_cd", Order = 1)]
        public string YohoCd { get; set; } = string.Empty;

        /// <summary>
        /// 用法
        /// 
        /// </summary>
        [Column("yoho")]
        [MaxLength(200)]
        public string? Yoho { get; set; } = string.Empty;
    }
}
