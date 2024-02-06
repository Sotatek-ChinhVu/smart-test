using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

/// <summary>
/// 電子処方箋処方内容控え
///     電子処方箋管理サービスに登録した電子処方箋の処方内容控え(PDF)
///     作成日時から保存期間(SYSTEM_CONF.GRP_CD=100040, GRP_EDA_NO=6)を過ぎたレコードは物理削除する
/// </summary>
[Table(name: "eps_reference")]
public class EpsReference : EmrCloneable<EpsReference>
{
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    [Column("hp_id", Order = 1)]
    public int HpId { get; set; }

    /// <summary>
    /// 患者ID
    /// </summary>
    [Column("pt_id", Order = 2)]
    public long PtId { get; set; }

    /// <summary>
    /// 来院番号
    /// </summary>
    [Column("raiin_no")]
    public long RaiinNo { get; set; }

    /// <summary>
    /// 診療日
    /// </summary>
    [Column("sin_date")]
    public int SinDate { get; set; }

    /// <summary>
    /// 処方箋ID
    ///     EPS_PRESCRIPTION.PRESCRIPTION_ID
    /// </summary>
    [Column("prescription_id", Order = 3)]
    [MaxLength(36)]
    public string PrescriptionId { get; set; } = string.Empty;

    /// <summary>
    /// 処方内容控え
    ///     base64エンコードされた処方内容控え(PDF)
    /// </summary>
    [Column("prescription_reference_information")]
    public string PrescriptionReferenceInformation { get; set; } = string.Empty;

    /// <summary>
    /// 作成日時
    /// </summary>
    [Column("create_date")]
    [CustomAttribute.DefaultValueSql("current_timestamp")]
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 作成ID
    /// </summary>
    [Column("create_id")]
    public int CreateId { get; set; }

    /// <summary>
    /// 作成端末
    /// </summary>
    [Column("create_machine")]
    [MaxLength(60)]
    public string CreateMachine { get; set; } = string.Empty;
}
