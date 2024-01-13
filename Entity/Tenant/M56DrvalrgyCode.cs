using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m56_drvalrgy_code")]
    public class M56DrvalrgyCode : EmrCloneable<M56DrvalrgyCode>
    {
        /// <summary>
        /// アレルギー関連系統コード
        /// 
        /// </summary>
        
        [Column("drvalrgy_cd", Order = 1)]
        [MaxLength(8)]
        public string DrvalrgyCd { get; set; } = string.Empty;

        /// <summary>
        /// アレルギー関連系統名
        /// 
        /// </summary>
        [Column("drvalrgy_name")]
        [MaxLength(200)]
        public string? DrvalrgyName { get; set; } = string.Empty;

        /// <summary>
        /// アレルギー関連系統グループ
        /// 
        /// </summary>
        [Column("drvalrgy_grp")]
        [MaxLength(4)]
        public string? DrvalrgyGrp { get; set; } = string.Empty;

        /// <summary>
        /// 優先順位
        /// 
        /// </summary>
        [Column("rank_no")]
        [CustomAttribute.DefaultValue(0)]
        public int RankNo { get; set; }
    }
}
