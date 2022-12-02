using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M12_FOOD_ALRGY_KBN")]
    public class M12FoodAlrgyKbn : EmrCloneable<M12FoodAlrgyKbn>
    {
        /// <summary>
        /// アレルギー区分
        /// 
        /// </summary>
        [Key]
        [Column("FOOD_KBN", Order = 1)]
        [MaxLength(2)]
        public string FoodKbn { get; set; } = string.Empty;

        /// <summary>
        /// 名称
        /// 
        /// </summary>
        [Column("FOOD_NAME")]
        [Required]
        [MaxLength(60)]
        public string? FoodName { get; set; } = string.Empty;

    }
}
