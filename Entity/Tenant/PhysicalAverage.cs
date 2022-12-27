using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "PHYSICAL_AVERAGE")]
    public class PhysicalAverage : EmrCloneable<PhysicalAverage>
    {
        /// <summary>
        /// 実施年度
        /// 
        /// </summary>
        
        [Column("JISSI_YEAR", Order = 1)]
        public int JissiYear { get; set; }

        /// <summary>
        /// 年齢
        /// 
        /// </summary>
        
        [Column("AGE_YEAR", Order = 2)]
        public int AgeYear { get; set; }

        /// <summary>
        /// 月齢
        /// 
        /// </summary>
        
        [Column("AGE_MONTH", Order = 3)]
        public int AgeMonth { get; set; }

        /// <summary>
        /// 日齢
        /// 
        /// </summary>
        
        [Column("AGE_DAY", Order = 4)]
        public int AgeDay { get; set; }

        /// <summary>
        /// 男性平均身長
        /// cm
        /// </summary>
        [Column("MALE_HEIGHT")]
        public double MaleHeight { get; set; }

        /// <summary>
        /// 男性平均体重
        /// kg
        /// </summary>
        [Column("MALE_WEIGHT")]
        public double MaleWeight { get; set; }

        /// <summary>
        /// 男性平均胸囲
        /// cm
        /// </summary>
        [Column("MALE_CHEST")]
        public double MaleChest { get; set; }

        /// <summary>
        /// 男性平均頭囲
        /// cm
        /// </summary>
        [Column("MALE_HEAD")]
        public double MaleHead { get; set; }

        /// <summary>
        /// 女性平均身長
        /// cm
        /// </summary>
        [Column("FEMALE_HEIGHT")]
        public double FemaleHeight { get; set; }

        /// <summary>
        /// 女性平均体重
        /// kg
        /// </summary>
        [Column("FEMALE_WEIGHT")]
        public double FemaleWeight { get; set; }

        /// <summary>
        /// 女性平均胸囲
        /// cm
        /// </summary>
        [Column("FEMALE_CHEST")]
        public double FemaleChest { get; set; }

        /// <summary>
        /// 女性平均頭囲
        /// cm
        /// </summary>
        [Column("FEMALE_HEAD")]
        public double FemaleHead { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}
