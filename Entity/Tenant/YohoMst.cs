using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entity.Tenant;

/// <summary>
/// 用法マスタ
///     電子処方箋管理サービスで管理する用法マスタ
/// </summary>
[Table(name: "yoho_mst")]
public class YohoMst : EmrCloneable<YohoMst>
{
    /// <summary>
    /// 医療機関識別ID
    /// </summary>
    [Column("hp_id", Order = 1)]
    public int HpId { get; set; }

    /// <summary>
    /// 用法コード
    /// </summary>
    [Column("yoho_cd", Order = 2)]
    [MaxLength(16)]
    public string YohoCd { get; set; } = string.Empty;

    /// <summary>
    /// 基本用法区分コード
    ///     1：内服、
    ///     2：外用、
    ///     3：注射、
    ///     4：注入、
    ///     X：ー（指定なし）
    /// </summary>
    [Column("yoho_kbn_cd")]
    [MaxLength(1)]
    [CustomAttribute.DefaultValue("x")]
    public string YohoKbnCd { get; set; } = string.Empty;

    /// <summary>
    /// 基本用法区分
    /// </summary>
    [Column("yoho_kbn")]
    [MaxLength(2)]
    public string YohoKbn { get; set; } = string.Empty;

    /// <summary>
    /// 用法詳細区分コード
    ///     基本用法区分が1：内服の場合
    ///     0：経口、1：舌下、2：バッカル、3：口腔内塗布
    ///     基本用法区分が2：外用の場合
    ///     A：貼付、B：塗布、C：湿布、D：撒布、E：噴霧、F：消毒、
    ///     G：点耳、H：点眼、J：点鼻、K：うがい、L：吸入、M：トローチ、
    ///     N：膀胱洗浄、P：鼻腔内洗浄、Q：浣腸、R：肛門挿入、S：肛門注入、
    ///     T：腟内挿入、U：膀胱注入
    ///     基本用法区分が3：注射の場合
    ///     0：静脈注射、1：中心静脈注射、2：皮下注射、3：筋肉内注射、4：皮内注射、
    ///     5：動脈注射、
    ///     A：硬膜外注射、B：脳脊髄腔内注射、C：脊髄内注射、D：関節腔内注射、E：腱鞘内注射、
    ///     F：腱鞘周囲注射、G：硝子体内注射、H：結膜下注射、J：テノン氏のう内注射、
    ///     K：耳茸内注射、L：咽頭注射、M：胸腔内注射、N：角膜内注射、Q：球後注射、
    ///     R：腹腔内注射、Z：局所・病巣内注射
    ///     基本用法区分が4：注入の場合
    ///     0：腹膜透析、1：気管内注入、2：涙のう内注入、3：鼓室内注入、4：滑液嚢穿刺後の注入、
    ///     5：腹腔内注入、Z：病巣内注入
    ///     基本用法区分がX：ー（指定なし）の場合
    ///     X：ー（指定なし）
    /// </summary>
    [Column("yoho_detail_kbn_cd")]
    [MaxLength(1)]
    [CustomAttribute.DefaultValue("x")]
    public string YohoDetailKbnCd { get; set; } = string.Empty;

    /// <summary>
    /// 用法詳細区分
    /// </summary>
    [Column("yoho_detail_kbn")]
    [MaxLength(15)]
    public string YohoDetailKbn { get; set; } = string.Empty;

    /// <summary>
    /// タイミング指定区分コード
    ///     1：食事ベース型　　　　　　（１日回数明示、投与時を食事タイミングで指定）
    ///     2：回数指定（等間隔）型　　（１日回数明示、等間隔投与を指定）
    ///     3：時刻指定型　　　　　　　（１日回数明示、投与時刻を指定）
    ///     投与時刻は用法補足レコードに用法補足区分５（用法の続き）として用法補足情報欄に記録する（処方箋情報・調剤結果情報）
    ///     4：回数指定（イベント）型　（１日回数明示、投与時を育児等の生活イベントで指定）
    ///     5：頓用指示型　　　　　　　（投与時を身体条件やイベントで指定）
    ///     6：生活リズム型　　　　　　（１日回数を明示、投与時を生活リズムで指定）
    ///     7：回数のみ指定型　　　　　（１日回数のみ指定）
    ///     8：時間間隔指定型　　　　　（時間間隔のみ指定）
    /// </summary>
    [Column("timing_kbn_cd")]
    public int TimingKbnCd { get; set; }

    /// <summary>
    /// タイミング指定区分
    /// </summary>
    [Column("timing_kbn")]
    [MaxLength(60)]
    public string TimingKbn { get; set; } = string.Empty;

    /// <summary>
    /// 用法名称
    /// </summary>
    [Column("yoho_name")]
    [MaxLength(50)]
    public string YohoName { get; set; } = string.Empty;

    /// <summary>
    /// 標準用法整理番号
    ///     「標準用法用語集（第2版）」（平成28年1月：日本薬剤師会、日本病院薬剤師会編）に収載されている4桁の整理番号
    ///     該当する標準用法整理番号が無い場合、記録を省略する
    /// </summary>
    [Column("reference_no")]
    public int ReferenceNo { get; set; }

    /// <summary>
    /// 使用開始日
    /// </summary>
    [Column("start_date", Order = 3)]
    public int StartDate { get; set; }

    /// <summary>
    /// 使用終了日
    /// </summary>
    [Column("end_date")]
    public int EndDate { get; set; }

    /// <summary>
    /// 用法コード区分
    ///     0：標準コード
    ///     （標準コードについては、当該コード及び用法名称にて回数及びタイミング等を指定できる。用法レコードの用法名称欄に記録されたものを用法として用いる。）
    ///     1：拡張コード
    ///     （拡張コードについては、当該コードだけでは回数及びタイミンング等を指定できないため、必ず用法補足レコードを用いて用法を記録する。用法補足レコードの用法補足情報欄に記録されたものを用法として用いる。）
    ///     2：汎用コード
    ///     （基本用法区分及び用法詳細区分を指定しない場合に汎用コードを用いて用法を記録する。用法レコードの用法名称欄に記録されたものを用法として用い、投与方法・経路を指示する場合は用法補足レコードに記録する。）
    /// </summary>
    [Column("yoho_cd_kbn")]
    [CustomAttribute.DefaultValue(0)]
    public int YohoCdKbn { get; set; }

    /// <summary>
    /// 頓用の条件指定
    ///     投与のタイミングを用法補足レコードを用いて頓用の条件を記録する必要があるか否かを表す
    ///     0：不要、
    ///     1：必要
    /// </summary>
    [Column("tonyo_joken")]
    [CustomAttribute.DefaultValue(0)]
    public int TonyoJoken { get; set; }

    /// <summary>
    /// 投与タイミング
    ///     用法補足レコードを用いて投与のタイミングを記録する必要があるか否かを表す
    ///     0：不要、
    ///     1：必要
    ///     ※食事ベース型の拡張コードの場合、「朝食後」「夕食前」など食事等のタイミングを用法補足情報欄に記録する。
    ///     生活リズム型の拡張コードの場合、「朝昼夕」「起床時」など生活リズム上の出来事や行為を用法補足情報欄に記録する。
    /// </summary>
    [Column("toyo_timing")]
    [CustomAttribute.DefaultValue(0)]
    public int ToyoTiming { get; set; }

    /// <summary>
    /// 投与時刻
    ///     用法補足レコードを用いて投与時刻を記録する必要があるか否かを表す
    ///     0：不要、
    ///     1：必要
    /// </summary>
    [Column("toyo_time")]
    [CustomAttribute.DefaultValue(0)]
    public int ToyoTime { get; set; }

    /// <summary>
    /// 投与間隔
    ///     用法補足レコードを用いて投与間隔を記録する必要があるか否かを表す
    ///     0：不要、
    ///     1：必要
    /// </summary>
    [Column("toyo_interval")]
    [CustomAttribute.DefaultValue(0)]
    public int ToyoInterval { get; set; }

    /// <summary>
    /// 部位
    ///     用法補足レコードを用いて部位を記録する必要か否かを表す。
    ///     0：不要、1：任意、2：左・右・両、3：必須
    ///     注）調剤結果情報の場合、部位を用法補足レコードの用法補足情報欄に記載する
    /// </summary>
    [Column("bui")]
    [CustomAttribute.DefaultValue(0)]
    public int Bui { get; set; }

    /// <summary>
    /// 用法カナ名称
    ///     用法名称のカナ（全角）
    /// </summary>
    [Column("yoho_kana_name")]
    [MaxLength(120)]
    public string YohoKanaName { get; set; } = string.Empty;

    /// <summary>
    /// 用法コード（調剤レセプト）
    ///     オンライン又は光ディスク等による請求に係る記録条件仕様（調剤用）の「別表12　用法コード」に収載されているコード
    /// </summary>
    [Column("chozai_yoho_cd")]
    public int ChozaiYohoCd { get; set; }

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
