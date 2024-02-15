using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "syouki_kbn_mst")]
    public class SyoukiKbnMst : EmrCloneable<SyoukiKbnMst>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 症状詳記区分
        /// 
        /// </summary>

        [Column("syouki_kbn", Order = 1)]
        public int SyoukiKbn { get; set; }

        /// <summary>
        /// 適用開始年月
        /// 
        /// </summary>
        
        [Column("start_ym", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int StartYm { get; set; }

        /// <summary>
        /// 適用終了年月
        /// 
        /// </summary>
        [Column("end_ym")]
        [CustomAttribute.DefaultValue(999999)]
        public int EndYm { get; set; }

        /// <summary>
        /// 症状詳記区分名称
        /// 
        /// </summary>
        [Column("name")]
        [MaxLength(200)]
        public string? Name { get; set; } = string.Empty;
    }
}
