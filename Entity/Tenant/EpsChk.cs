using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

/// <summary>
/// 電子処方箋重複投薬等チェック情報
///     電子処方箋管理サービスに送った重複投薬等チェック情報
/// </summary>
[Table(name: "EPS_CHK")]
public class EpsChk : EmrCloneable<EpsChk>
{
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    [Column("HP_ID", Order = 1)]
    public int HpId { get; set; }

    /// <summary>
    /// 患者ID
    /// </summary>
    [Column("PT_ID", Order = 2)]
    public long PtId { get; set; }

    /// <summary>
    /// 来院番号
    /// </summary>
    [Column("RAIIN_NO", Order = 3)]
    public long RaiinNo { get; set; }

    /// <summary>
    /// 連番
    /// </summary>
    [Column("SEQ_NO", Order = 4)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SeqNo { get; set; }

    /// <summary>
    /// 診療日
    /// </summary>
    [Column("SIN_DATE")]
    public int SinDate { get; set; }

    /// <summary>
    /// チェック結果
    ///     0:重複等あり(医師許可なし)
    ///     1:重複等あり(医師許可)
    ///     2:重複等なし
    /// </summary>
    [Column("CHECK_RESULT")]
    public int CheckResult { get; set; }

    /// <summary>
    /// 同一処方箋発行医療機関チェックフラグ
    ///     1:自院分をチェック対象にしない
    ///     2:自院分をチェック対象にする
    /// </summary>
    [Column("SAME_MEDICAL_INSTITUTION_ALERT_FLG")]
    public int SameMedicalInstitutionAlertFlg { get; set; }

    /// <summary>
    /// オンライン資格確認同意区分
    ///     0:オンライン資格確認端末による同意なし
    ///     1:オンライン資格確認端末による同意あり
    /// </summary>
    [Column("ONLINE_CONSENT")]
    public int OnlineConsent { get; set; }

    /// <summary>
    /// 口頭同意区分
    ///     0:口頭等による同意なし
    ///     1:口頭等による同意あり
    /// </summary>
    [Column("ORAL_BROWSING_CONSENT")]
    public int OralBrowsingConsent { get; set; }

    /// <summary>
    /// 処方情報
    ///     処方箋情報CSVの101,111,201レコード
    /// </summary>
    [Column("DRUG_INFO")]
    public string DrugInfo { get; set; } = string.Empty;

    /// <summary>
    /// 削除フラグ
    ///     0:有効…最新のチェック結果
    ///     1:無効…過去のチェック結果
    ///     2:未確定…カルテ保存していない処方に対するチェック結果
    ///     ※処方箋情報の登録には有効なチェック結果が必要
    /// </summary>
    [Column("IS_DELETED")]
    public int IsDeleted { get; set; }

    /// <summary>
    /// 作成日時
    /// </summary>
    [Column("CREATE_DATE")]
    [CustomAttribute.DefaultValueSql("current_timestamp")]
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 作成ID
    /// </summary>
    [Column("CREATE_ID")]
    public int CreateId { get; set; }

    /// <summary>
    /// 作成端末
    /// </summary>
    [Column("CREATE_MACHINE")]
    [MaxLength(60)]
    public string CreateMachine { get; set; } = string.Empty;

    /// <summary>
    /// 更新日時
    /// </summary>
    [Column("UPDATE_DATE")]
    [CustomAttribute.DefaultValueSql("current_timestamp")]
    public DateTime UpdateDate { get; set; }

    /// <summary>
    /// 更新ID
    /// </summary>
    [Column("UPDATE_ID")]
    public int UpdateId { get; set; }

    /// <summary>
    /// 更新端末
    /// </summary>
    [Column("UPDATE_MACHINE")]
    [MaxLength(60)]
    public string UpdateMachine { get; set; } = string.Empty;
}
