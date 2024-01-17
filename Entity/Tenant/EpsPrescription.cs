using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Entity.Tenant;

/// <summary>
/// 電子処方箋登録情報
///     電子処方箋管理サービスに登録した処方箋情報
///     ここに記録する処方箋は電子処方箋または紙の処方箋(引換番号あり)
/// </summary>
[Table(name: "eps_prescription")]
[Index(nameof(HpId), nameof(PrescriptionId), Name = "eps_prescription_idx01")]
public class EpsPrescription : EmrCloneable<EpsPrescription>
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
    [Column("raiin_no", Order = 3)]
    public long RaiinNo { get; set; }

    /// <summary>
    /// 連番
    /// </summary>
    [Column("seq_no", Order = 4)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SeqNo { get; set; }

    /// <summary>
    /// リフィル回数
    ///     1:リフィルではない通常の処方箋（総使用回数1回）
    ///     2:リフィル処方箋（総使用回数2回）
    ///     3:リフィル処方箋（総使用回数3回）
    /// </summary>
    [Column("refile_count")]
    [CustomAttribute.DefaultValue(1)]
    public int RefileCount { get; set; }

    /// <summary>
    /// 診療日
    /// </summary>
    [Column("sin_date")]
    public int SinDate { get; set; }

    /// <summary>
    /// 保険者番号
    ///     PT_HOKEN_INF.HOKENSYA_NO
    ///     保険が公費単独の場合は空
    /// </summary>
    [Column("hokensya_no")]
    [MaxLength(8)]
    public string HokensyaNo { get; set; } = string.Empty;

    /// <summary>
    /// 被保険者記号
    ///     PT_HOKEN_INF.KIGO
    ///     保険が公費単独の場合は空
    /// </summary>
    [Column("kigo")]
    [MaxLength(80)]
    public string Kigo { get; set; } = string.Empty;

    /// <summary>
    /// 被保険者番号
    ///     PT_HOKEN_INF.BANGO
    ///     保険が公費単独の場合は空
    /// </summary>
    [Column("bango")]
    [MaxLength(80)]
    public string Bango { get; set; } = string.Empty;

    /// <summary>
    /// 被保険者枝番
    ///     PT_HOKEN_INF.EDA_NO
    ///     保険が公費単独の場合は空
    /// </summary>
    [Column("eda_no")]
    [MaxLength(2)]
    public string EdaNo { get; set; } = string.Empty;

    /// <summary>
    /// 公費負担者番号
    ///     保険が公費単独以外の場合は空
    /// </summary>
    [Column("kohi_futansya_no")]
    [MaxLength(8)]
    public string KohiFutansyaNo { get; set; } = string.Empty;

    /// <summary>
    /// 公費受給者番号
    ///     保険が公費単独以外の場合は空
    /// </summary>
    [Column("kohi_jyukyusya_no")]
    [MaxLength(7)]
    public string KohiJyukyusyaNo { get; set; } = string.Empty;

    /// <summary>
    /// 処方箋ID
    /// </summary>
    [Column("prescription_id")]
    [MaxLength(36)]
    public string PrescriptionId { get; set; } = string.Empty;

    /// <summary>
    /// 引換番号
    /// </summary>
    [Column("access_code")]
    [MaxLength(16)]
    public string AccessCode { get; set; }=string.Empty;

    /// <summary>
    /// 発行形態
    ///     1: 電子
    ///     2: 紙
    /// </summary>
    [Column("issue_type")]
    [CustomAttribute.DefaultValue(2)]
    public int IssueType { get; set; }

    /// <summary>
    /// 処方箋情報CSV
    ///     base64エンコードされた処方箋情報CSV
    /// </summary>
    [Column("prescription_document")]
    public string PrescriptionDocument { get; set; } = string.Empty;

    /// <summary>
    /// 状態
    ///     0:登録
    ///     1:取消待ち
    ///     2:取消中
    ///     3:取消済み
    /// </summary>
    [Column("status")]
    [CustomAttribute.DefaultValue(0)]
    public int Status { get; set; }

    /// <summary>
    /// 取消理由
    ///     1:自動取消
    ///     2:変更
    ///     3:エラー
    ///     4:登録中断
    ///     5:手動取消
    ///     6:紙の処方箋（引換番号なし）発行
    /// </summary>
    [Column("deleted_reason")]
    public int DeletedReason { get; set; }

    /// <summary>
    /// 取消日時
    ///     STATUS>0に更新した日時
    /// </summary>
    [Column("deleted_date")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// 診療科
    ///     RAIIN_INF.KA_ID
    /// </summary>
    [Column("ka_id")]
    public int KaId { get; set; }

    /// <summary>
    /// 担当医
    ///     RAIIN_INF.TANTO_ID
    /// </summary>
    [Column("tanto_id")]
    public int TantoId { get; set; }

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

    /// <summary>
    /// 更新日時
    /// </summary>
    [Column("update_date")]
    [CustomAttribute.DefaultValueSql("current_timestamp")]
    public DateTime UpdateDate { get; set; }

    /// <summary>
    /// 更新ID
    /// </summary>
    [Column("update_id")]
    public int UpdateId { get; set; }

    /// <summary>
    /// 更新端末
    /// </summary>
    [Column("update_machine")]
    [MaxLength(60)]
    public string UpdateMachine { get; set; } = string.Empty;
}
