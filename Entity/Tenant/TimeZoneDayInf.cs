using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 時間帯日情報
    /// </summary>
    [Table(name: "TIME_ZONE_DAY_INF")]
    public class TimeZoneDayInf : EmrCloneable<TimeZoneDayInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column(name: "HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        //[Key]
        [Column(name: "ID", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        //[Key]
        [Column(name: "SIN_DATE", Order = 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        [Column(name: "START_TIME")]
        public int StartTime { get; set; }

        /// <summary>
        /// 終了時間
        /// </summary>
        [Column(name: "END_TIME")]
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
        [Column(name: "TIME_KBN")]
        [Required]
        public int TimeKbn { get; set; }

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
        public string CreateMachine { get; set; }

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
        public string UpdateMachine { get; set; }
    }
}
