using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m12_food_alrgy")]
    [Index(nameof(KikinCd), nameof(YjCd), nameof(FoodKbn), nameof(TenpuLevel), Name = "m12_food_alrgy_idx01")]
    public class M12FoodAlrgy : EmrCloneable<M12FoodAlrgy>
    {
        /// <summary>
        /// 基金コード
        /// 
        /// </summary>
        [Column("kikin_cd")]
        [MaxLength(9)]
        public string? KikinCd { get; set; } = string.Empty;

        /// <summary>
        /// YJコード
        /// 
        /// </summary>
        
        [Column("yj_cd", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// アレルギー区分
        /// 
        /// </summary>
        
        [Column("food_kbn", Order = 2)]
        [MaxLength(2)]
        public string FoodKbn { get; set; } = string.Empty;

        /// <summary>
        /// 添付文書レベル
        /// 
        /// </summary>
        
        [Column("tenpu_level", Order = 3)]
        [MaxLength(2)]
        public string TenpuLevel { get; set; } = string.Empty;

        /// <summary>
        /// 注意コメント
        /// 
        /// </summary>
        [Column("attention_cmt")]
        [MaxLength(500)]
        public string? AttentionCmt { get; set; } = string.Empty;

        /// <summary>
        /// 作用機序
        /// 
        /// </summary>
        [Column("working_mechanism")]
        [MaxLength(1000)]
        public string? WorkingMechanism { get; set; } = string.Empty;
    }
}