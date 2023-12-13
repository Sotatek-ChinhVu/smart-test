using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.Tenant;

/// <summary>
/// 用法補足情報
///     用法に紐づく用法補足情報
///     用法補足には、TEN_MSTに登録された補助用法(TEN_MST.YOHO_KBN=2)またはフリーコメントを登録する
/// </summary>
[Table(name: "YOHO_HOSOKU")]
public class YohoHosoku : EmrCloneable<YohoHosoku>
{
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    [Column("HP_ID", Order = 1)]
    public int HpId { get; set; }

    /// <summary>
    /// 用法項目コード
    ///     用法のTEN_MST.ITEM_CD
    /// </summary>
    [Column("ITEM_CD", Order = 2)]
    [MaxLength(10)]
    public string ItemCd { get; set; } = string.Empty;

    /// <summary>
    /// 開始日
    ///     用法のTEN_MST.START_DATE
    /// </summary>
    [Column("START_DATE", Order = 3)]
    public int StartDate { get; set; }

    /// <summary>
    /// 連番
    /// </summary>
    [Column("SEQ_NO", Order = 4)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SeqNo { get; set; }

    /// <summary>
    /// 並び順
    /// </summary>
    [Column("SORT_NO")]
    [CustomAttribute.DefaultValue(1)]
    public int SortNo { get; set; }

    /// <summary>
    /// 用法補足区分
    ///     0:未指定
    ///     1:漸減
    ///     2:一包化
    ///     3:隔日
    ///     4:粉砕
    ///     5:用法の続き
    ///     6:部位
    ///     7:１回使用量
    /// </summary>
    [Column("YOHO_HOSOKU_KBN")]
    [CustomAttribute.DefaultValue(0)]
    public int YohoHosokuKbn { get; set; }

    /// <summary>
    /// 用法補足記録
    ///     0:未指定
    ///     1:頓用の条件指定
    ///     2:投与タイミング
    ///     3:投与時刻
    ///     4:投与間隔
    ///     50:部位（左・右・両）
    ///     51:部位（その他）
    /// </summary>
    [Column("YOHO_HOSOKU_REC")]
    [CustomAttribute.DefaultValue(0)]
    public int YohoHosokuRec { get; set; }

    /// <summary>
    /// 補助用法項目コード
    ///     用法補足が補助用法の場合は補助用法のTEN_MST.ITEM_CD、フリーコメントの場合はnull
    /// </summary>
    [Column("HOSOKU_ITEM_CD")]
    [MaxLength(10)]
    public string HosokuItemCd { get; set; } = string.Empty;

    /// <summary>
    /// 補足内容
    ///     用法補足が補助用法の場合はTEN_MST.NAME、フリーコメントの場合はフリーコメント
    /// </summary>
    [Column("HOSOKU")]
    [MaxLength(240)]
    public string Hosoku { get; set; } = string.Empty;

    /// <summary>
    /// 削除フラグ
    ///     1: 削除
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
