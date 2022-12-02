using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 時間帯設定
    /// </summary>
    [Table(name: "TIME_ZONE_CONF")]
    public class TimeZoneConf : EmrCloneable<TimeZoneConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column(name: "HP_ID", Order = 1)]
        //[Index("TIME_ZONE_CONF_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 曜日区分
        ///     1..7:日曜～土曜
        /// </summary>
        //[Key]
        [Column(name: "YOUBI_KBN", Order = 2)]
        //[Index("TIME_ZONE_CONF_IDX01", 2)]
        public int YoubiKbn { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        //[Key]
        [Column(name: "SEQ_NO", Order = 3)]
        //[Index("TIME_ZONE_CONF_IDX01", 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        [Column(name: "START_TIME")]
        [CustomAttribute.DefaultValue(0)]
        public int StartTime
        {
            get;
            set;
        }

        /// <summary>
        /// 終了時間
        /// </summary>
        [Column(name: "END_TIME")]
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
        [Column(name: "TIME_KBN")]
        [Required]
        public int TimeKbn { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column(name: "IS_DELETED")]
        //[Index("TIME_ZONE_CONF_IDX01", 4)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDelete { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
