using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rsv_frame_day_ptn")]
    public class RsvFrameDayPtn : EmrCloneable<RsvFrameDayPtn>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 予約枠ID
        /// 
        /// </summary>
        
        [Column("rsv_frame_id", Order = 2)]
        public int RsvFrameId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        
        [Column("sin_date", Order = 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("seq_no", Order = 4)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 開始時間
        /// 
        /// </summary>
        [Column("start_time")]
        public int StartTime { get; set; }

        /// <summary>
        /// 終了時間
        /// 
        /// </summary>
        [Column("end_time")]
        public int EndTime { get; set; }

        /// <summary>
        /// 分単位
        /// 
        /// </summary>
        [Column("minutes")]
        [CustomAttribute.DefaultValue(0)]
        public int Minutes { get; set; }

        /// <summary>
        /// 人数枠
        /// 
        /// </summary>
        [Column("number")]
        [CustomAttribute.DefaultValue(0)]
        public int Number { get; set; }

        /// <summary>
        /// 受付種別
        /// 
        /// </summary>
        [Column("uketuke_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeSbt { get; set; }

        /// <summary>
        /// 休診区分
        /// 1: 休診日
        /// </summary>
        [Column("is_holiday")]
        [CustomAttribute.DefaultValue(0)]
        public int IsHoliday { get; set; }

        /// <summary>
        /// 削除フラグ
        /// 1: 削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
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
        /// 更新者
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
