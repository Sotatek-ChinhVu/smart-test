using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 予約枠情報
    /// </summary>
    [Table("RSV_FRAME_INF")]
    public class RsvFrameInf : EmrCloneable<RsvFrameInf>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        //[Key]
        [Column("HP_ID", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 予約枠ID
        /// </summary>
        //[Key]
        [Column("RSV_FRAME_ID", Order = 3)]
        public int RsvFrameId { get; set; }

        /// <summary>
        /// 診療日
        /// </summary>
        //[Key]
        [Column("SIN_DATE", Order = 4)]
        public int SinDate { get; set; }

        /// <summary>
        /// 開始時間
        /// </summary>
        //[Key]
        [Column("START_TIME", Order = 5)]
        public int StartTime { get; set; }

        /// <summary>
        /// 終了時間
        /// </summary>
        [Column("END_TIME")]
        public int EndTime { get; set; }

        /// <summary>
        /// 枠番号
        /// </summary>
        [Column("FRAME_NO")]
        public int FrameNo { get; set; }

        /// <summary>
        /// 休診区分
        ///     1:休診日
        /// </summary>
        [Column("IS_HOLIDAY")]
        [CustomAttribute.DefaultValue(0)]
        public int IsHoliday { get; set; }

        /// <summary>
        /// 予約番号
        /// </summary>
        [Column("NUMBER")]
        [CustomAttribute.DefaultValue(0)]
        public long Number { get; set; }

        /// <summary>
        /// 枠種別
        ///     1: 追加枠
        /// </summary>
        [Column("FRAME_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int FrameSbt { get; set; }

        /// <summary>
        /// 受付種別
        /// </summary>
        [Column("UKETUKE_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeSbt { get; set; }

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
