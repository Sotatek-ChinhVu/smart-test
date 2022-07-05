using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M12_FOOD_ALRGY")]
    [Index(nameof(KikinCd), nameof(YjCd), nameof(FoodKbn), nameof(TenpuLevel), Name = "M12_FOOD_ALRGY_IDX01")]
    public class M12FoodAlrgy : EmrCloneable<M12FoodAlrgy>
    {
        /// <summary>
        /// 基金コード
        /// 
        /// </summary>
        [Column("KIKIN_CD")]
        [MaxLength(9)]
        public string KikinCd { get; set; }

        /// <summary>
        /// YJコード
        /// 
        /// </summary>
        [Key]
        [Column("YJ_CD", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; }

        /// <summary>
        /// アレルギー区分
        /// 
        /// </summary>
        [Key]
        [Column("FOOD_KBN", Order = 2)]
        [MaxLength(2)]
        public string FoodKbn { get; set; }

        /// <summary>
        /// 添付文書レベル
        /// 
        /// </summary>
        [Key]
        [Column("TENPU_LEVEL", Order = 3)]
        [MaxLength(2)]
        public string TenpuLevel { get; set; }

        /// <summary>
        /// 注意コメント
        /// 
        /// </summary>
        [Column("ATTENTION_CMT")]
        [MaxLength(500)]
        public string AttentionCmt { get; set; }

        /// <summary>
        /// 作用機序
        /// 
        /// </summary>
        [Column("WORKING_MECHANISM")]
        [MaxLength(1000)]
        public string WorkingMechanism { get; set; }

    }
}