using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 予約枠情報
    /// </summary>
    [Table("rsv_frame_inf")]
    public class RsvFrameInf : EmrCloneable<RsvFrameInf>
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 予約枠ID
        /// </summary>
        
        [Column("rsv_frame_id", Order = 3)]
        public int RsvFrameId { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        
        [Column("sin_date", Order = 4)]
        public int SinDate { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        
        [Column("start_time", Order = 5)]
        public int StartTime { get; set; }

        /// <summary>
        /// 終了時間
        /// </summary>
        [Column("end_time")]
        public int EndTime { get; set; }

        /// <summary>
        /// 枠番号
        /// </summary>
        [Column("frame_no")]
        public int FrameNo { get; set; }

        /// <summary>
        /// 休診区分
        ///     1:休診日
        /// </summary>
        [Column("is_holiday")]
        [CustomAttribute.DefaultValue(0)]
        public int IsHoliday { get; set; }

        /// <summary>
        /// 予約番号
        /// </summary>
        [Column("number")]
        [CustomAttribute.DefaultValue(0)]
        public long Number { get; set; }

        /// <summary>
        /// 枠種別
        ///     1: 追加枠
        /// </summary>
        [Column("frame_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int FrameSbt { get; set; }

        /// <summary>
        /// 受付種別
        /// </summary>
        [Column("uketuke_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeSbt { get; set; }

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
