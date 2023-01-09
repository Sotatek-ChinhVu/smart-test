using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RSV_INF")]
    public class RsvInf : EmrCloneable<RsvInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 予約枠ID
        /// 
        /// </summary>
        
        [Column("RSV_FRAME_ID", Order = 2)]
        public int RsvFrameId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        
        [Column("SIN_DATE", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SinDate { get; set; }

        /// <summary>
        /// 開始時間
        /// 
        /// </summary>
        
        [Column("START_TIME", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int StartTime { get; set; }

        /// <summary>
        /// 予約番号
        /// 
        /// </summary>
        
        [Column("RAIIN_NO", Order = 5)]
        [CustomAttribute.DefaultValue(0)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("PT_ID")]
        [CustomAttribute.DefaultValue(0)]
        public long PtId { get; set; }

        /// <summary>
        /// 予約種別コード
        /// 
        /// </summary>
        [Column("RSV_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int RsvSbt { get; set; }

        /// <summary>
        /// 担当医師コード
        /// 
        /// </summary>
        [Column("TANTO_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int TantoId { get; set; }

        /// <summary>
        /// 診療科コード
        /// 
        /// </summary>
        [Column("KA_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int KaId { get; set; }

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
        public string? CreateMachine { get; set; } = string.Empty;

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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
