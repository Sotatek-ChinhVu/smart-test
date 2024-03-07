using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "gc_std_mst")]
    public class GcStdMst : EmrCloneable<GcStdMst>
    {
        /// <summary>
        /// 基準値区分
        /// 0:体重 1:身長
        /// </summary>

        [Column("std_kbn")]
        public int StdKbn { get; set; }

        /// <summary>
        /// 性別
        /// 1:男 2:女
        /// </summary>

        [Column("sex")]
        public int Sex { get; set; }

        /// <summary>
        /// 位置
        /// 
        /// </summary>
        [Column("point")]
        public double Point { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("sd_m25")]
        public double SdM25 { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("sd_m20")]
        public double SdM20 { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("sd_m10")]
        public double SdM10 { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("sd_avg")]
        public double SdAvg { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("sd_p10")]
        public double SdP10 { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("sd_p20")]
        public double SdP20 { get; set; }

        /// <summary>
        /// SD
        /// N
        /// </summary>
        [Column("sd_p25")]
        public double SdP25 { get; set; }

        /// <summary>
        /// 3%
        /// 
        /// </summary>
        [Column("per_03")]
        public double Per03 { get; set; }

        /// <summary>
        /// 10%
        /// 
        /// </summary>
        [Column("per_10")]
        public double Per10 { get; set; }

        /// <summary>
        /// 25%
        /// 
        /// </summary>
        [Column("per_25")]
        public double Per25 { get; set; }

        /// <summary>
        /// 50%
        /// 
        /// </summary>
        [Column("per_50")]
        public double Per50 { get; set; }

        /// <summary>
        /// 75%
        /// 
        /// </summary>
        [Column("per_75")]
        public double Per75 { get; set; }

        /// <summary>
        /// 90%
        /// 
        /// </summary>
        [Column("per_90")]
        public double Per90 { get; set; }

        /// <summary>
        /// 97%
        /// 
        /// </summary>
        [Column("per_97")]
        public double Per97 { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
