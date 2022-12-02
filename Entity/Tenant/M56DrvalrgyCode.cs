using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M56_DRVALRGY_CODE")]
    public class M56DrvalrgyCode : EmrCloneable<M56DrvalrgyCode>
    {
        /// <summary>
        /// アレルギー関連系統コード
        /// 
        /// </summary>
        
        [Column("DRVALRGY_CD", Order = 1)]
        [MaxLength(8)]
        public string DrvalrgyCd { get; set; } = string.Empty;

        /// <summary>
        /// アレルギー関連系統名
        /// 
        /// </summary>
        [Column("DRVALRGY_NAME")]
        [MaxLength(200)]
        public string? DrvalrgyName { get; set; } = string.Empty;

        /// <summary>
        /// アレルギー関連系統グループ
        /// 
        /// </summary>
        [Column("DRVALRGY_GRP")]
        [MaxLength(4)]
        public string? DrvalrgyGrp { get; set; } = string.Empty;

        /// <summary>
        /// 優先順位
        /// 
        /// </summary>
        [Column("RANK_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int RankNo { get; set; }
    }
}
