using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
	/// 来院情報
	/// </summary>
	[Table("z_raiin_inf")]
    public class ZRaiinInf : EmrCloneable<ZRaiinInf>
    {

        [Column("op_id", Order = 1)]
        public long OpId { get; set; }

        [Column("op_type")]
        [MaxLength(10)]
        public string? OpType { get; set; } = string.Empty;

        [Column("op_time")]
        public DateTime OpTime { get; set; }

        [Column("op_addr")]
        [MaxLength(100)]
        public string? OpAddr { get; set; } = string.Empty;

        [Column("op_hostname")]
        [MaxLength(100)]
        public string? OpHostName { get; set; } = string.Empty;
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>
        [Column("raiin_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        ///		yyyymmdd	
        /// </summary>
        [Column("sin_date")]
        public int SinDate { get; set; }

        /// <summary>
        /// 親来院番号
        /// </summary>
        [Column("oya_raiin_no")]
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
        [Column("status")]
        [CustomAttribute.DefaultValue(0)]
        public int Status { get; set; }

        /// <summary>
        /// 予約フラグ
        ///		1:予約の来院		
        /// </summary>
        [Column("is_yoyaku")]
        [CustomAttribute.DefaultValue(0)]
        public int IsYoyaku { get; set; }

        /// <summary>
        /// 予約時間
        ///		HH24MISS
        /// </summary>
        [Column("yoyaku_time")]
        [MaxLength(6)]
        [CustomAttribute.DefaultValue("0")]
        public string? YoyakuTime { get; set; } = string.Empty;

        /// <summary>
        /// 予約者ID
        /// </summary>
        [Column("yoyaku_id")]
        [CustomAttribute.DefaultValue(0)]
        public int YoyakuId { get; set; }

        /// <summary>
        /// 受付種別
        /// </summary>
        [Column("uketuke_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeSbt { get; set; }

        /// <summary>
        /// 受付時間
        ///		HH24MISS
        /// </summary>
        [Column("uketuke_time")]
        [MaxLength(6)]
        [CustomAttribute.DefaultValue("0")]
        public string? UketukeTime { get; set; } = string.Empty;

        /// <summary>
        /// 受付者ID
        /// </summary>
        [Column("uketuke_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeId { get; set; }

        /// <summary>
        /// 受付番号
        /// </summary>
        [Column("uketuke_no")]
        [CustomAttribute.DefaultValue(0)]
        public int UketukeNo { get; set; }

        /// <summary>
        /// 診察開始時間
        ///		HH24MISS
        /// </summary>
        [Column("sin_start_time")]
        [MaxLength(6)]
        [CustomAttribute.DefaultValue("0")]
        public string? SinStartTime { get; set; } = string.Empty;

        /// <summary>
        /// 診察終了時間
        ///		HH24MISS　※状態が計算以上になった時間								
        /// </summary>
        [Column("sin_end_time")]
        [MaxLength(6)]
        [CustomAttribute.DefaultValue("0")]
        public string? SinEndTime { get; set; } = string.Empty;

        /// <summary>
        /// 精算時間
        ///		HH24MISS
        /// </summary>
        [Column("kaikei_time")]
        [MaxLength(6)]
        [CustomAttribute.DefaultValue("0")]
        public string? KaikeiTime { get; set; } = string.Empty;

        /// <summary>
        /// 精算者ID
        /// </summary>
        [Column("kaikei_id")]
        [CustomAttribute.DefaultValue(0)]
        public int KaikeiId { get; set; }

        /// <summary>
        /// 診療科ID
        /// </summary>
        [Column("ka_id")]
        [CustomAttribute.DefaultValue(0)]
        public int KaId { get; set; }

        /// <summary>
        /// 担当医ID
        /// </summary>
        [Column("tanto_id")]
        [CustomAttribute.DefaultValue(0)]
        public int TantoId { get; set; }

        /// <summary>
        /// 保険組合せID
        ///		患者別に保険情報を識別するための固有の番号
        /// </summary>
        [Column("hoken_pid")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 初再診区分
        ///		受付時設定、ODR_INF更新後はトリガーで設定							
        /// </summary>
        [Column("syosaisin_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SyosaisinKbn { get; set; }

        /// <summary>
        /// 時間枠区分
        ///		受付時設定、ODR_INF更新後はトリガーで設定							
        /// </summary>
        [Column("jikan_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int JikanKbn { get; set; }

        /// <summary>
        /// CONFIRMATION_RESULT
        /// </summary>
        [Column("confirmation_result")]
        [MaxLength(120)]
        public string? ConfirmationResult { get; set; } = string.Empty;

        /// <summary>
        /// CONFIRMATION_STATE
        /// </summary>
        [Column("confirmation_state")]
        [CustomAttribute.DefaultValue(0)]
        public int ConfirmationState { get; set; }

        /// <summary>
        /// 削除区分
        ///		1: 削除
        /// </summary>
        [Column("is_deleted")]
        //[Index("raiin_inf_idx01", 5)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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

        /// <summary>
        /// 算定区分
        ///     2: 自費算定
        /// </summary>
        [Column("santei_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiKbn { get; set; }

        /// <summary>
        /// 資格確認タイプ
        ///		1: マイナンバーカードで確認
        ///     2: 保険証で確認
        /// </summary>
        [Column("confirmation_type")]
        [CustomAttribute.DefaultValue(0)]
        public int ConfirmationType { get; set; }

        [Column("info_cons_flg")]
        [MaxLength(10)]
        public string? InfoConsFlg { get; set; } = string.Empty;

        /// <summary>
        /// PRESCRIPTION_ISSUE_TYPE
        /// </summary>
        [Column("prescription_issue_type")]
        [CustomAttribute.DefaultValue(0)]
        public int PrescriptionIssueType { get; set; }
    }
}
