using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m12_food_alrgy_kbn")]
    public class M12FoodAlrgyKbn : EmrCloneable<M12FoodAlrgyKbn>
    {
        /// <summary>
        /// アレルギー区分
        /// 
        /// </summary>
        
        [Column("food_kbn", Order = 1)]
        [MaxLength(2)]
        public string FoodKbn { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// 
        /// </summary>
        [Column("food_name")]
        [Required]
        [MaxLength(60)]
        public string? FoodName { get; set; } = string.Empty;

    }
}
