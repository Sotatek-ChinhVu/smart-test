using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "yakka_syusai_mst")]
    [Index(nameof(StartDate), nameof(EndDate), Name = "yakka_syusai_mst_idx01")]
    public class YakkaSyusaiMst : EmrCloneable<YakkaSyusaiMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 薬価基準コード
        /// 
        /// </summary>

        [Column("yakka_cd", Order = 2)]
        [MaxLength(12)]
        public string YakkaCd { get; set; } = string.Empty;

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>

        [Column("item_cd", Order = 3)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 開始日
        /// 
        /// </summary>

        [Column("start_date", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        [Column("end_date")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 成分名
        /// 
        /// </summary>
        [Column("seibun")]
        [MaxLength(255)]
        public string? Seibun { get; set; } = string.Empty;

        /// <summary>
        /// 品目名
        /// 
        /// </summary>
        [Column("hinmoku")]
        [MaxLength(255)]
        public string? Hinmoku { get; set; } = string.Empty;

        /// <summary>
        /// 区分
        /// "1: 後発品のない先発品
        /// 2: 後発品のある先発品
        /// ★:後発品のある先発品のうち、加算対象外の薬剤
        /// 3: 後発品
        /// ☆:後発品のうち、加算対象外の薬剤
        /// NULL:その他"
        /// </summary>
        [Column("kbn")]
        [MaxLength(2)]
        public string? Kbn { get; set; } = string.Empty;

        /// <summary>
        /// 収載日
        /// 
        /// </summary>
        [Column("syusai_date")]
        [CustomAttribute.DefaultValue(0)]
        public int SyusaiDate { get; set; }

        /// <summary>
        /// 経過情報
        /// 
        /// </summary>
        [Column("keika")]
        [MaxLength(255)]
        public string? Keika { get; set; } = string.Empty;

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        [Column("biko")]
        [MaxLength(255)]
        public string? Biko { get; set; } = string.Empty;

        /// <summary>
        /// 準先発
        /// 1: 準先発で後発品のある薬剤（加算対象）
        /// </summary>
        [Column("jun_senpatu")]
        [CustomAttribute.DefaultValue(0)]
        public int JunSenpatu { get; set; }

        /// <summary>
        /// 単位
        /// 
        /// </summary>
        [Column("unit_name")]
        [MaxLength(100)]
        public string? UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 薬価
        /// 
        /// </summary>
        [Column("yakka")]
        [CustomAttribute.DefaultValue(0)]
        public double Yakka { get; set; }

        /// <summary>
        /// 対象外フラグ
        /// 1:後発医薬品の規格単位数量の割合を算出する際に除外する医薬品
        /// </summary>
        [Column("is_notarget")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNotarget { get; set; }

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
}
