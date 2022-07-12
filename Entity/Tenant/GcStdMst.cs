using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "GC_STD_MST")]
    public class GcStdMst : EmrCloneable<GcStdMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 基準値区分
        /// 0:体重 1:身長
        /// </summary>
        //[Key]
        [Column("STD_KBN", Order = 2)]
        public int StdKbn { get; set; }

        /// <summary>
        /// 性別
        /// 1:男 2:女
        /// </summary>
        //[Key]
        [Column("SEX", Order = 3)]
        public int Sex { get; set; }

        /// <summary>
        /// 位置
        /// 
        /// </summary>
        //[Key]
        [Column("POINT", Order = 4)]
        public double Point { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("SD_M25")]
        public double SdM25 { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("SD_M20")]
        public double SdM20 { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("SD_M10")]
        public double SdM10 { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("SD_AVG")]
        public double SdAvg { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("SD_P10")]
        public double SdP10 { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("SD_P20")]
        public double SdP20 { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("SD_P25")]
        public double SdP25 { get; set; }

        /// <summary>
        /// 3%
        /// 
        /// </summary>
        [Column("PER_03")]
        public double Per03 { get; set; }

        /// <summary>
        /// 10%
        /// 
        /// </summary>
        [Column("PER_10")]
        public double Per10 { get; set; }

        /// <summary>
        /// 25%
        /// 
        /// </summary>
        [Column("PER_25")]
        public double Per25 { get; set; }

        /// <summary>
        /// 50%
        /// 
        /// </summary>
        [Column("PER_50")]
        public double Per50 { get; set; }

        /// <summary>
        /// 75%
        /// 
        /// </summary>
        [Column("PER_75")]
        public double Per75 { get; set; }

        /// <summary>
        /// 90%
        /// 
        /// </summary>
        [Column("PER_90")]
        public double Per90 { get; set; }

        /// <summary>
        /// 97%
        /// 
        /// </summary>
        [Column("PER_97")]
        public double Per97 { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }  = string.Empty;

    }
}
