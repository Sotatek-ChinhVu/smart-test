using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.Tenant;

/// <summary>
/// 電子処方箋重複投薬等チェック詳細
///     電子処方箋管理サービスから返ってきた重複投薬等チェック詳細情報
/// </summary>
[Table(name: "eps_chk_detail")]
public class EpsChkDetail : EmrCloneable<EpsChkDetail>
{
    /// <summary>
    /// 医療機関識別ID
    ///     EPS_CHK.HP_ID
    /// </summary>
    [Column("hp_id", Order = 1)]
    public int HpId { get; set; }

    /// <summary>
    /// 患者ID
    ///     EPS_CHK.PT_ID
    /// </summary>
    [Column("pt_id", Order = 2)]
    public long PtId { get; set; }

    /// <summary>
    /// 来院番号
    ///     EPS_CHK.RAIIN_NO
    /// </summary>
    [Column("raiin_no", Order = 3)]
    public long RaiinNo { get; set; }

    /// <summary>
    /// 連番
    ///     EPS_CHK.SEQ_NO
    /// </summary>
    [Column("seq_no", Order = 4)]
    public long SeqNo { get; set; }

    /// <summary>
    /// メッセージID
    ///     チェック結果内の連番
    /// </summary>
    [Column("message_id", Order = 5)]
    [MaxLength(3)]
    public string MessageId { get; set; } = string.Empty;

    /// <summary>
    /// メッセージ分類
    /// </summary>
    [Column("message_category")]
    [MaxLength(50)]
    public string MessageCategory { get; set; } = string.Empty;

    /// <summary>
    /// 対象医薬品・成分名称
    /// </summary>
    [Column("pharmaceuticals_ingredient_name")]
    [MaxLength(80)]
    public string PharmaceuticalsIngredientName { get; set; } = string.Empty;

    /// <summary>
    /// メッセージ
    /// </summary>
    [Column("message")]
    [MaxLength(400)]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿対象薬品コード種別
    /// </summary>
    [Column("target_pharmaceutical_code_type")]
    [MaxLength(1)]
    public string TargetPharmaceuticalCodeType { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿対象薬品コード
    /// </summary>
    [Column("target_pharmaceutical_code")]
    [MaxLength(13)]
    public string TargetPharmaceuticalCode { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿対象薬品名称
    /// </summary>
    [Column("target_pharmaceutical_name")]
    [MaxLength(80)]
    public string TargetPharmaceuticalName { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿調剤数量
    /// </summary>
    [Column("target_dispensing_quantity")]
    [MaxLength(3)]
    public string TargetDispensingQuantity { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿用法
    /// </summary>
    [Column("target_usage")]
    [MaxLength(100)]
    public string TargetUsage { get; set; } = string.Empty;

    /// <summary>
    /// 今回＿剤形
    /// </summary>
    [Column("target_dosage_form")]
    [MaxLength(10)]
    public string TargetDosageForm { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿調剤実施日
    /// </summary>
    [Column("past_date")]
    [MaxLength(8)]
    public string PastDate { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿対象薬品コード種別
    /// </summary>
    [Column("past_pharmaceutical_code_type")]
    [MaxLength(1)]
    public string PastPharmaceuticalCodeType { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿対象薬品コード
    /// </summary>
    [Column("past_pharmaceutical_code")]
    [MaxLength(13)]
    public string PastPharmaceuticalCode { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿対象薬品名称
    /// </summary>
    [Column("past_pharmaceutical_name")]
    [MaxLength(80)]
    public string PastPharmaceuticalName { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿医療機関名称
    /// </summary>
    [Column("past_medical_institution_name")]
    [MaxLength(120)]
    public string PastMedicalInstitutionName { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿保険薬局名称
    /// </summary>
    [Column("past_insurance_pharmacy_name")]
    [MaxLength(120)]
    public string PastInsurancePharmacyName { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿調剤数量
    /// </summary>
    [Column("past_dispensing_quantity")]
    [MaxLength(3)]
    public string PastDispensingQuantity { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿用法
    /// </summary>
    [Column("past_usage")]
    [MaxLength(100)]
    public string PastUsage { get; set; } = string.Empty;

    /// <summary>
    /// 過去＿剤形
    /// </summary>
    [Column("past_dosage_form")]
    [MaxLength(10)]
    public string PastDosageForm { get; set; } = string.Empty;

    /// <summary>
    /// 投与理由コメント
    /// </summary>
    [Column("comment")]
    [MaxLength(300)]
    public string Comment { get; set; } = string.Empty;
}
