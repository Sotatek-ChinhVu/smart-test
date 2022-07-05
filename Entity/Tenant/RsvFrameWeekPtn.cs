using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RSV_FRAME_WEEK_PTN")]
    public class RsvFrameWeekPtn : EmrCloneable<RsvFrameWeekPtn>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", Order = 1)]
        public int Id { get; set; }
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 予約枠ID
        /// 
        /// </summary>
        [Key]
        [Column("RSV_FRAME_ID", Order = 3)]
        public int RsvFrameId { get; set; }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        [Column("START_DATE", Order = 4)]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        [Column("END_DATE")]
        public int EndDate { get; set; }

        /// <summary>
        /// 曜日
        /// 
        /// </summary>
        [Key]
        [Column("WEEK", Order = 5)]
        public int Week { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SEQ_NO", Order = 6)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 開始時間
        /// 
        /// </summary>
        [Column("START_TIME")]
        public int StartTime { get; set; }

        /// <summary>
        /// 終了時間
        /// 
        /// </summary>
        [Column("END_TIME")]
        public int EndTime { get; set; }

        /// <summary>
        /// 分単位
        /// 
        /// </summary>
        [Column("MINUTES")]
        [CustomAttribute.DefaultValue(0)]
        public int Minutes { get; set; }

        /// <summary>
        /// 人数枠
        /// 
        /// </summary>
        [Column("NUMBER")]
        [CustomAttribute.DefaultValue(0)]
        public int Number { get; set; }

        /// <summary>
        /// 受付種別
        /// 
        /// </summary>
        [Column("UKETUKE_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeSbt { get; set; }

        /// <summary>
        /// 休診区分
        /// 1: 休診日
        /// </summary>
        [Column("IS_HOLIDAY")]
        [CustomAttribute.DefaultValue(0)]
        public int IsHoliday { get; set; }

        /// <summary>
        /// 削除フラグ
        /// 1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
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
        public string CreateMachine { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
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
        public string UpdateMachine { get; set; }

    }
}
