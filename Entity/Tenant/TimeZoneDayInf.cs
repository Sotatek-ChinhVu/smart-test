using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 時間帯日情報
    /// </summary>
    [Table(name: "time_zone_day_inf")]
    public class TimeZoneDayInf : EmrCloneable<TimeZoneDayInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column(name: "hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        
        [Column(name: "id", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        
        [Column(name: "sin_date", Order = 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        [Column(name: "start_time")]
        public int StartTime { get; set; }

        /// <summary>
        /// 終了時間
        /// </summary>
        [Column(name: "end_time")]
        [Required]
        public int EndTime { get; set; }

        /// <summary>
        /// 時間区分
        ///     0:時間内 ※時間内のレコードは不要
        ///     1:時間外
        ///     2:休日 ※HOLIDAY_MSTで設定するため未使用
        ///     3:深夜
        ///     4:夜間早朝
        /// </summary>
        [Column(name: "time_kbn")]
        [Required]
        public int TimeKbn { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
