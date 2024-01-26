using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant;

[Table(name: "yousiki1_inf")]
public class Yousiki1Inf : EmrCloneable<Yousiki1Inf>
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
    /// 同一HP_ID,PT_ID,SIN_YM,DATA_TYPE内の連番
    /// </summary>
    [Column("seq_no", Order = 5)]
    [CustomAttribute.DefaultValue(1)]
    public int SeqNo { get; set; }

    /// <summary>
    /// 削除フラグ
    /// 1:削除
    /// </summary>
    [Column("is_deleted")]
    [CustomAttribute.DefaultValue(0)]
    public int IsDeleted { get; set; }

    /// <summary>
    /// 保存状態
    /// 0:未入力、1:一時保存、2:保存
    /// </summary>
    [Column("status")]
    [CustomAttribute.DefaultValue(0)]
    public int Status { get; set; }

    /// <summary>
    /// 作成日時
    /// 
    /// </summary>
    [Column("create_date")]
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// 作成者
    /// 
    /// </summary>
    [Column("create_id")]
    [CustomAttribute.DefaultValue(0)]
    public int CreateId { get; set; }

    /// <summary>
    /// 作成端末
    /// 
    /// </summary>
    [Column("create_machine")]
    [MaxLength(60)]
    public string? CreateMachine { get; set; } = string.Empty;

    /// <summary>
    /// 更新日時
    /// 
    /// </summary>
    [Column("update_date")]
    public DateTime UpdateDate { get; set; }

    /// <summary>
    /// 更新者
    /// 
    /// </summary>
    [Column("update_id")]
    [CustomAttribute.DefaultValue(0)]
    public int UpdateId { get; set; }

    /// <summary>
    /// 更新端末
    /// 
    /// </summary>
    [Column("update_machine")]
    [MaxLength(60)]
    public string? UpdateMachine { get; set; } = string.Empty;

}
