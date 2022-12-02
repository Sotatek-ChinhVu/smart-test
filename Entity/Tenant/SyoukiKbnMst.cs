using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SYOUKI_KBN_MST")]
    public class SyoukiKbnMst : EmrCloneable<SyoukiKbnMst>
    {
        /// <summary>
        /// 症状詳記区分
        /// 
        /// </summary>
        [Key]
        [Column("SYOUKI_KBN", Order = 1)]
        public int SyoukiKbn { get; set; }

        /// <summary>
        /// 適用開始年月
        /// 
        /// </summary>
        //[Key]
        [Column("START_YM", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int StartYm { get; set; }

        /// <summary>
        /// 適用終了年月
        /// 
        /// </summary>
        [Column("END_YM")]
        [CustomAttribute.DefaultValue(999999)]
        public int EndYm { get; set; }

        /// <summary>
        /// 症状詳記区分名称
        /// 
        /// </summary>
        [Column("NAME")]
        [MaxLength(200)]
        public string? Name { get; set; } = string.Empty;
    }
}
