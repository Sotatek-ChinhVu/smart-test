using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

[Table(name: "yousiki1_inf_detail")]
public class Yousiki1InfDetail : EmrCloneable<Yousiki1InfDetail>
{
    /// <summary>
    /// 医療機関識別ID
    /// 
    /// </summary>
    [Column("hp_id", Order = 1)]
    [CustomAttribute.DefaultValue(1)]
    public int HpId { get; set; }

    /// <summary>
    /// 患者番号
    /// 
    /// </summary>
    [Column("pt_id", Order = 2)]
    [CustomAttribute.DefaultValue(0)]
    public long PtId { get; set; }

    /// <summary>
    /// 診療年月
    /// 
    /// </summary>
    [Column("sin_ym", Order = 3)]
    [CustomAttribute.DefaultValue(0)]
    public int SinYm { get; set; }

    /// <summary>
    /// データの種類
    /// 0:共通、1:生活習慣、2:在宅、3:リハビリ
    /// </summary>
    [Column("data_type", Order = 4)]
    [CustomAttribute.DefaultValue(0)]
    public int DataType { get; set; }

    /// <summary>
    /// 連番
    /// 同一RP_NO,PT_ID,SIN_YM,DATA_TYPE,SEQ_NO内の連番
    /// </summary>
    [Column("seq_no", Order = 5)]
    [CustomAttribute.DefaultValue(1)]
    public int SeqNo { get; set; }

    /// <summary>
    /// コード番号
    /// 外来様式１で定められたコード
    /// </summary>
    [Column("code_no", Order = 6)]
    [MaxLength(10)]
    public string? CodeNo { get; set; } = string.Empty;

    /// <summary>
    /// 行番号
    /// グリッド等、複数レコード入力可能な場合のレコードごとの連番
    /// </summary>
    [Column("row_no", Order = 7)]
    [CustomAttribute.DefaultValue(0)]
    public int RowNo { get; set; }

    /// <summary>
    /// ペイロード
    /// 外来様式１で定められたコード
    /// </summary>
    [Column("payload", Order = 8)]
    [CustomAttribute.DefaultValue(1)]
    public int Payload { get; set; }

    /// <summary>
    /// 値
    /// 入力値
    /// </summary>
    [Column("value")]
    [MaxLength(160)]
    public string? Value { get; set; } = string.Empty;
}
