using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 来院情報
	/// </summary>
	[Table("RAIIN_INF")]
    [Index(nameof(HpId), nameof(PtId), nameof(SinDate), nameof(Status), nameof(IsDeleted), Name = "RAIIN_INF_IDX01")]
    [Index(nameof(HpId), nameof(PtId), nameof(SinDate), nameof(Status), nameof(SyosaisinKbn), nameof(IsDeleted), Name = "RAIIN_INF_IDX02")]
    public class RaiinInf : EmrCloneable<RaiinInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        //[Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

		/// <summary>
		/// 診療日
		///		yyyymmdd	
		/// </summary>
		[Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        [Key]
        [Column("RAIIN_NO", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long RaiinNo { get; set; }

        /// <summary>
        /// 親来院番号
        /// </summary>
        [Column("OYA_RAIIN_NO")]
        public long OyaRaiinNo { get; set; }

        /// <summary>
        /// 状態
        ///		0:予約
        ///		1:受付
        ///		3:一時保存
        ///		5:計算
        ///		7:精算待ち
        ///		9:精算済
        /// </summary>
        [Column("STATUS")]
        [CustomAttribute.DefaultValue(0)]
        public int Status { get; set; }

		/// <summary>
		/// 予約フラグ
		///		1:予約の来院		
		/// </summary>
		[Column("IS_YOYAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int IsYoyaku { get; set; }

		/// <summary>
		/// 予約時間
		///		HH24MISS
		/// </summary>
		[Column("YOYAKU_TIME")]
		[MaxLength(6)]
		[CustomAttribute.DefaultValue("0")]
		public string? YoyakuTime { get; set; } = string.Empty;

        /// <summary>
        /// 予約者ID
        /// </summary>
        [Column("YOYAKU_ID")]
		[CustomAttribute.DefaultValue(0)]
		public int YoyakuId { get; set; }
       
        /// <summary>
        /// 受付種別
        /// </summary>
        [Column("UKETUKE_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeSbt { get; set; }

        /// <summary>
        /// 受付時間
        ///		HH24MISS
        /// </summary>
        [Column("UKETUKE_TIME")]
        [MaxLength(6)]
        [CustomAttribute.DefaultValue("0")]
        public string? UketukeTime { get; set; } = string.Empty;

        /// <summary>
        /// 受付者ID
        /// </summary>
        [Column("UKETUKE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeId { get; set; }

        /// <summary>
        /// 受付番号
        /// </summary>
        [Column("UKETUKE_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeNo { get; set; }

        /// <summary>
        /// 診察開始時間
        ///		HH24MISS
        /// </summary>
        [Column("SIN_START_TIME")]
        [MaxLength(6)]
        [CustomAttribute.DefaultValue("0")]
        public string SinStartTime { get; set; } = string.Empty;

        /// <summary>
        /// 診察終了時間
        ///		HH24MISS　※状態が計算以上になった時間								
        /// </summary>
        [Column("SIN_END_TIME")]
        [MaxLength(6)]
        [CustomAttribute.DefaultValue("0")]
        public string? SinEndTime { get; set; } = string.Empty;

        /// <summary>
        /// 精算時間
        ///		HH24MISS
        /// </summary>
        [Column("KAIKEI_TIME")]
        [MaxLength(6)]
        [CustomAttribute.DefaultValue("0")]
        public string? KaikeiTime { get; set; } = string.Empty;

        /// <summary>
        /// 精算者ID
        /// </summary>
        [Column("KAIKEI_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int KaikeiId { get; set; }

        /// <summary>
        /// 診療科ID
        /// </summary>
        [Column("KA_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int KaId { get; set; }

        /// <summary>
        /// 担当医ID
        /// </summary>
        [Column("TANTO_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int TantoId { get; set; }

        /// <summary>
        /// 保険組合せID
        ///		患者別に保険情報を識別するための固有の番号
        /// </summary>
        [Column("HOKEN_PID")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 算定区分
        ///     2: 自費算定
        /// </summary>
        [Column("SANTEI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiKbn { get; set; }

        /// <summary>
        /// 初再診区分
        ///		受付時設定、ODR_INF更新後はトリガーで設定							
        /// </summary>
        [Column("SYOSAISIN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SyosaisinKbn { get; set; }

		/// <summary>
		/// 時間枠区分
		///		受付時設定、ODR_INF更新後はトリガーで設定							
		/// </summary>
		[Column("JIKAN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int JikanKbn { get; set; }

        /// <summary>
        /// CONFIRMATION_RESULT
        /// </summary>
        [Column("CONFIRMATION_RESULT")]
        [MaxLength(120)]
        public string? ConfirmationResult { get; set; } = string.Empty;

        /// <summary>
        /// CONFIRMATION_STATE
        /// </summary>
        [Column("CONFIRMATION_STATE")]
        [CustomAttribute.DefaultValue(0)]
        public int ConfirmationState { get; set; }

        /// <summary>
        /// 削除区分
        ///		1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
		public string? UpdateMachine { get; set; }  = string.Empty;
    }
}