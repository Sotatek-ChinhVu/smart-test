using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 時間帯設定
    /// </summary>
    [Table(name: "time_zone_conf")]
    public class TimeZoneConf : EmrCloneable<TimeZoneConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column(name: "hp_id", Order = 1)]
        //[Index("time_zone_conf_idx01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 曜日区分
        ///     1..7:日曜～土曜
        /// </summary>
        
        [Column(name: "youbi_kbn", Order = 2)]
        //[Index("time_zone_conf_idx01", 2)]
        public int YoubiKbn { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column(name: "seq_no", Order = 3)]
        //[Index("time_zone_conf_idx01", 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        [Column(name: "start_time")]
        [CustomAttribute.DefaultValue(0)]
        public int StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// 終了時間
        /// </summary>
        [Column(name: "end_time")]
        [CustomAttribute.DefaultValue(2400)]
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
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column(name: "is_deleted")]
        //[Index("time_zone_conf_idx01", 4)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDelete { get; set; }

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
