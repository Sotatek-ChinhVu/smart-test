using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

/// <summary>
/// 電子処方箋処方内容控え
///     電子処方箋管理サービスに登録した電子処方箋の処方内容控え(PDF)
///     作成日時から保存期間(SYSTEM_CONF.GRP_CD=100040, GRP_EDA_NO=6)を過ぎたレコードは物理削除する
/// </summary>
[Table(name: "EPS_REFERENCE")]
public class EpsReference : EmrCloneable<EpsReference>
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
    [Column("RAIIN_NO")]
    public long RaiinNo { get; set; }

    /// <summary>
    /// 診療日
    /// </summary>
    [Column("SIN_DATE")]
    public int SinDate { get; set; }

    /// <summary>
    /// 処方箋ID
    ///     EPS_PRESCRIPTION.PRESCRIPTION_ID
    /// </summary>
    [Column("PRESCRIPTION_ID", Order = 3)]
    [MaxLength(36)]
    public string PrescriptionId { get; set; } = string.Empty;

    /// <summary>
    /// 処方内容控え
    ///     base64エンコードされた処方内容控え(PDF)
    /// </summary>
    [Column("PRESCRIPTION_REFERENCE_INFORMATION")]
    public string PrescriptionReferenceInformation { get; set; } = string.Empty;

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
}
