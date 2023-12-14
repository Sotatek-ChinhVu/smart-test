using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.Tenant;

/// <summary>
/// 電子処方箋重複投薬等チェック詳細
///     電子処方箋管理サービスから返ってきた重複投薬等チェック詳細情報
/// </summary>
[Table(name: "EPS_CHK_DETAIL")]
public class EpsChkDetail : EmrCloneable<EpsChkDetail>
{
    /// <summary>
    /// 医療機関識別ID
    ///     EPS_CHK.HP_ID
    /// </summary>
    [Column("HP_ID", Order = 1)]
    public int HpId { get; set; }

    /// <summary>
    /// 患者ID
    ///     EPS_CHK.PT_ID
    /// </summary>
    [Column("PT_ID", Order = 2)]
    public long PtId { get; set; }

    /// <summary>
    /// 来院番号
    ///     EPS_CHK.RAIIN_NO
    /// </summary>
    [Column("RAIIN_NO", Order = 3)]
    public long RaiinNo { get; set; }

    /// <summary>
    /// 連番
    ///     EPS_CHK.SEQ_NO
    /// </summary>
    [Column("SEQ_NO", Order = 4)]
    public long SeqNo { get; set; }

    /// <summary>
    /// メッセージID
    ///     チェック結果内の連番
    /// </summary>
    [Column("MESSAGE_ID", Order = 5)]
    [MaxLength(3)]
    public string MessageId { get; set; } = string.Empty;

    /// <summary>
    /// メッセージ分類
    /// </summary>
    [Column("MESSAGE_CATEGORY")]
    [MaxLength(50)]
    public string MessageCategory { get; set; } = string.Empty;

    /// <summary>
    /// 対象医薬品・成分名称
    /// </summary>
    [Column("PHARMACEUTICALS_INGREDIENT_NAME")]
    [MaxLength(80)]
    public string PharmaceuticalsIngredientName { get; set; } = string.Empty;

    /// <summary>
    /// メッセージ
    /// </summary>
    [Column("MESSAGE")]
    [MaxLength(400)]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿対象薬品コード種別
    /// </summary>
    [Column("TARGET_PHARMACEUTICAL_CODE_TYPE")]
    [MaxLength(1)]
    public string TargetPharmaceuticalCodeType { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿対象薬品コード
    /// </summary>
    [Column("TARGET_PHARMACEUTICAL_CODE")]
    [MaxLength(13)]
    public string TargetPharmaceuticalCode { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿対象薬品名称
    /// </summary>
    [Column("TARGET_PHARMACEUTICAL_NAME")]
    [MaxLength(80)]
    public string TargetPharmaceuticalName { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿調剤数量
    /// </summary>
    [Column("TARGET_DISPENSING_QUANTITY")]
    [MaxLength(3)]
    public string TargetDispensingQuantity { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿用法
    /// </summary>
    [Column("TARGET_USAGE")]
    [MaxLength(100)]
    public string TargetUsage { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿剤形
    /// </summary>
    [Column("TARGET_DOSAGE_FORM")]
    [MaxLength(10)]
    public string TargetDosageForm { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿調剤実施日
    /// </summary>
    [Column("PAST_DATE")]
    [MaxLength(8)]
    public string PastDate { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿対象薬品コード種別
    /// </summary>
    [Column("PAST_PHARMACEUTICAL_CODE_TYPE")]
    [MaxLength(1)]
    public string PastPharmaceuticalCodeType { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿対象薬品コード
    /// </summary>
    [Column("PAST_PHARMACEUTICAL_CODE")]
    [MaxLength(13)]
    public string PastPharmaceuticalCode { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿対象薬品名称
    /// </summary>
    [Column("PAST_PHARMACEUTICAL_NAME")]
    [MaxLength(80)]
    public string PastPharmaceuticalName { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿医療機関名称
    /// </summary>
    [Column("PAST_MEDICAL_INSTITUTION_NAME")]
    [MaxLength(120)]
    public string PastMedicalInstitutionName { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿保険薬局名称
    /// </summary>
    [Column("PAST_INSURANCE_PHARMACY_NAME")]
    [MaxLength(120)]
    public string PastInsurancePharmacyName { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿調剤数量
    /// </summary>
    [Column("PAST_DISPENSING_QUANTITY")]
    [MaxLength(3)]
    public string PastDispensingQuantity { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿用法
    /// </summary>
    [Column("PAST_USAGE")]
    [MaxLength(100)]
    public string PastUsage { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿剤形
    /// </summary>
    [Column("PAST_DOSAGE_FORM")]
    [MaxLength(10)]
    public string PastDosageForm { get; set; } = string.Empty;

    /// <summary>
    /// 投与理由コメント
    /// </summary>
    [Column("COMMENT")]
    [MaxLength(300)]
    public string Comment { get; set; } = string.Empty;
}
