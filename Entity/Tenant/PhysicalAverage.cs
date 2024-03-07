using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "physical_average")]
    public class PhysicalAverage : EmrCloneable<PhysicalAverage>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 実施年度
        /// 
        /// </summary>

        [Column("jissi_year", Order = 1)]
        public int JissiYear { get; set; }

        /// <summary>
        /// 年齢
        /// 
        /// </summary>

        [Column("age_year", Order = 2)]
        public int AgeYear { get; set; }

        /// <summary>
        /// 月齢
        /// 
        /// </summary>

        [Column("age_month", Order = 3)]
        public int AgeMonth { get; set; }

        /// <summary>
        /// 日齢
        /// 
        /// </summary>

        [Column("age_day", Order = 4)]
        public int AgeDay { get; set; }

        /// <summary>
        /// 男性平均身長
        /// cm
        /// </summary>
        [Column("male_height")]
        public double MaleHeight { get; set; }

        /// <summary>
        /// 男性平均体重
        /// kg
        /// </summary>
        [Column("male_weight")]
        public double MaleWeight { get; set; }

        /// <summary>
        /// 男性平均胸囲
        /// cm
        /// </summary>
        [Column("male_chest")]
        public double MaleChest { get; set; }

        /// <summary>
        /// 男性平均頭囲
        /// cm
        /// </summary>
        [Column("male_head")]
        public double MaleHead { get; set; }

        /// <summary>
        /// 女性平均身長
        /// cm
        /// </summary>
        [Column("female_height")]
        public double FemaleHeight { get; set; }

        /// <summary>
        /// 女性平均体重
        /// kg
        /// </summary>
        [Column("female_weight")]
        public double FemaleWeight { get; set; }

        /// <summary>
        /// 女性平均胸囲
        /// cm
        /// </summary>
        [Column("female_chest")]
        public double FemaleChest { get; set; }

        /// <summary>
        /// 女性平均頭囲
        /// cm
        /// </summary>
        [Column("female_head")]
        public double FemaleHead { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }
    }
}
