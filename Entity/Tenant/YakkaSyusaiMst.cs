using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "YAKKA_SYUSAI_MST")]
    public class YakkaSyusaiMst : EmrCloneable<YakkaSyusaiMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 薬価基準コード
        /// 
        /// </summary>
        [Key]
        [Column("YAKKA_CD", Order = 2)]
        [MaxLength(12)]
        public string YakkaCd { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Key]
        [Column("ITEM_CD", Order = 3)]
        [MaxLength(10)]
        public string ItemCd { get; set; }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        [Key]
        [Column("START_DATE", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        [Column("END_DATE")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 成分名
        /// 
        /// </summary>
        [Column("SEIBUN")]
        [MaxLength(255)]
        public string Seibun { get; set; }

        /// <summary>
        /// 品目名
        /// 
        /// </summary>
        [Column("HINMOKU")]
        [MaxLength(255)]
        public string Hinmoku { get; set; }

        /// <summary>
        /// 区分
        /// "1: 後発品のない先発品
        /// 2: 後発品のある先発品
        /// ★:後発品のある先発品のうち、加算対象外の薬剤
        /// 3: 後発品
        /// ☆:後発品のうち、加算対象外の薬剤
        /// NULL:その他"
        /// </summary>
        [Column("KBN")]
        [MaxLength(2)]
        public string Kbn { get; set; }

        /// <summary>
        /// 収載日
        /// 
        /// </summary>
        [Column("SYUSAI_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int SyusaiDate { get; set; }

        /// <summary>
        /// 経過情報
        /// 
        /// </summary>
        [Column("KEIKA")]
        [MaxLength(255)]
        public string Keika { get; set; }

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        [Column("BIKO")]
        [MaxLength(255)]
        public string Biko { get; set; }

        /// <summary>
        /// 準先発
        /// 1: 準先発で後発品のある薬剤（加算対象）
        /// </summary>
        [Column("JUN_SENPATU")]
        [CustomAttribute.DefaultValue(0)]
        public int JunSenpatu { get; set; }

        /// <summary>
        /// 単位
        /// 
        /// </summary>
        [Column("UNIT_NAME")]
        [MaxLength(100)]
        public string UnitName { get; set; }

        /// <summary>
        /// 薬価
        /// 
        /// </summary>
        [Column("YAKKA")]
        [CustomAttribute.DefaultValue(0)]
        public double Yakka { get; set; }

        /// <summary>
        /// 対象外フラグ
        /// 1:後発医薬品の規格単位数量の割合を算出する際に除外する医薬品
        /// </summary>
        [Column("IS_NOTARGET")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNotarget { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }

    }
}
