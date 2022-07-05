using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M28_DRUG_MST")]
    public class M28DrugMst : EmrCloneable<M28DrugMst>
    {
        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>
        [Key]
        [Column("YJ_CD", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; }

        /// <summary>
        /// 厚生労働省コード
        /// 
        /// </summary>
        [Column("KOSEISYO_CD")]
        [MaxLength(12)]
        public string KoseisyoCd { get; set; }

        /// <summary>
        /// 基金コード
        /// 
        /// </summary>
        [Column("KIKIN_CD")]
        [MaxLength(9)]
        public string KikinCd { get; set; }

        /// <summary>
        /// 製品名
        /// 
        /// </summary>
        [Column("DRUG_NAME")]
        [MaxLength(200)]
        public string DrugName { get; set; }

        /// <summary>
        /// 医薬品名称読み１
        /// 
        /// </summary>
        [Column("DRUG_KANA1")]
        [MaxLength(100)]
        public string DrugKana1 { get; set; }

        /// <summary>
        /// 医薬品名称読み２
        /// 
        /// </summary>
        [Column("DRUG_KANA2")]
        [MaxLength(100)]
        public string DrugKana2 { get; set; }

        /// <summary>
        /// 一般名
        /// 
        /// </summary>
        [Column("IPN_NAME")]
        [MaxLength(400)]
        public string IpnName { get; set; }

        /// <summary>
        /// 一般名読み
        /// 
        /// </summary>
        [Column("IPN_KANA")]
        [MaxLength(100)]
        public string IpnKana { get; set; }

        /// <summary>
        /// 薬価単位数量
        /// 
        /// </summary>
        [Column("YAKKA_VAL")]
        public int YakkaVal { get; set; }

        /// <summary>
        /// 薬価単位
        /// 
        /// </summary>
        [Column("YAKKA_UNIT")]
        [MaxLength(20)]
        public string YakkaUnit { get; set; }

        /// <summary>
        /// 成分量・力価
        /// Y
        /// </summary>
        [Column("SEIBUN_RIKIKA")]
        public double SeibunRikika { get; set; }

        /// <summary>
        /// 成分量・力価単位
        /// 
        /// </summary>
        [Column("SEIBUN_RIKIKA_UNIT")]
        [MaxLength(30)]
        public string SeibunRikikaUnit { get; set; }

        /// <summary>
        /// 容量・重量
        /// Y
        /// </summary>
        [Column("YORYO_JYURYO")]
        public double YoryoJyuryo { get; set; }

        /// <summary>
        /// 容量・重量単位
        /// 
        /// </summary>
        [Column("YORYO_JYURYO_UNIT")]
        [MaxLength(20)]
        public string YoryoJyuryoUnit { get; set; }

        /// <summary>
        /// 成分量・力価/用量・重量比
        /// Y
        /// </summary>
        [Column("SEIRIKI_YORYO_RATE")]
        public double SeirikiYoryoRate { get; set; }

        /// <summary>
        /// 成分量・力価/用量・重量比単位
        /// 
        /// </summary>
        [Column("SEIRIKI_YORYO_UNIT")]
        [MaxLength(40)]
        public string SeirikiYoryoUnit { get; set; }

        /// <summary>
        /// 製造販売承認メーカーコード
        /// 
        /// </summary>
        [Column("MAKER_CD")]
        [MaxLength(4)]
        public string MakerCd { get; set; }

        /// <summary>
        /// 製造販売承認メーカー
        /// 
        /// </summary>
        [Column("MAKER_NAME")]
        [MaxLength(40)]
        public string MakerName { get; set; }

        /// <summary>
        /// 薬剤区分コード
        /// 
        /// </summary>
        [Column("DRUG_KBN_CD")]
        [MaxLength(1)]
        public string DrugKbnCd { get; set; }

        /// <summary>
        /// 薬剤区分
        /// 
        /// </summary>
        [Column("DRUG_KBN")]
        [MaxLength(10)]
        public string DrugKbn { get; set; }

        /// <summary>
        /// 剤形区分コード
        /// 1:内用薬 4:注射薬 6:外用薬
        /// </summary>
        [Column("FORM_KBN_CD")]
        [MaxLength(3)]
        public string FormKbnCd { get; set; }

        /// <summary>
        /// 剤形区分
        /// 
        /// </summary>
        [Column("FORM_KBN")]
        [MaxLength(100)]
        public string FormKbn { get; set; }

        /// <summary>
        /// 毒薬フラグ
        /// 1:毒薬 Null:毒薬でない
        /// </summary>
        [Column("DOKUYAKU_FLG")]
        [MaxLength(1)]
        public string DokuyakuFlg { get; set; }

        /// <summary>
        /// 劇薬フラグ
        /// 1:劇薬 Null:劇薬でない
        /// </summary>
        [Column("GEKIYAKU_FLG")]
        [MaxLength(1)]
        public string GekiyakuFlg { get; set; }

        /// <summary>
        /// 麻薬フラグ
        /// 1:麻薬 Null:麻薬でない
        /// </summary>
        [Column("MAYAKU_FLG")]
        [MaxLength(1)]
        public string MayakuFlg { get; set; }

        /// <summary>
        /// 向精神薬フラグ
        /// 1:向精神薬 Null:向精神薬でない
        /// </summary>
        [Column("KOSEISINYAKU_FLG")]
        [MaxLength(1)]
        public string KoseisinyakuFlg { get; set; }

        /// <summary>
        /// 覚醒剤フラグ
        /// 1:覚醒剤 Null:覚醒剤でない
        /// </summary>
        [Column("KAKUSEIZAI_FLG")]
        [MaxLength(1)]
        public string KakuseizaiFlg { get; set; }

        /// <summary>
        /// 覚醒剤原料フラグ
        /// 1:覚醒剤原料 Null:覚醒剤原料でない
        /// </summary>
        [Column("KAKUSEIZAI_GENRYO_FLG")]
        [MaxLength(1)]
        public string KakuseizaiGenryoFlg { get; set; }

        /// <summary>
        /// 生物由来製品フラグ
        /// 1:生物由来製品 Null:生物由来製品でない
        /// </summary>
        [Column("SEIBUTU_FLG")]
        [MaxLength(1)]
        public string SeibutuFlg { get; set; }

        /// <summary>
        /// 特定生物由来製品フラグ
        /// 1:特定生物由来製品区分 Null:特定生物由来製品区分でない
        /// </summary>
        [Column("SP_SEIBUTU_FLG")]
        [MaxLength(1)]
        public string SpSeibutuFlg { get; set; }

        /// <summary>
        /// 後発品フラグ
        /// 1:後発品である Null:後発品でない
        /// </summary>
        [Column("KOHATU_FLG")]
        [MaxLength(1)]
        public string KohatuFlg { get; set; }

        /// <summary>
        /// 薬価
        /// Y
        /// </summary>
        [Column("YAKKA")]
        public double Yakka { get; set; }

        /// <summary>
        /// 規格単位
        /// 
        /// </summary>
        [Column("KIKAKU_UNIT")]
        [MaxLength(100)]
        public string KikakuUnit { get; set; }

        /// <summary>
        /// 薬価換算式
        /// 
        /// </summary>
        [Column("YAKKA_RATE_FORMURA")]
        [MaxLength(30)]
        public string YakkaRateFormura { get; set; }

        /// <summary>
        /// 薬価換算可能単位
        /// 
        /// </summary>
        [Column("YAKKA_RATE_UNIT")]
        [MaxLength(40)]
        public string YakkaRateUnit { get; set; }

        /// <summary>
        /// 薬価基準収載日付
        /// 
        /// </summary>
        [Column("YAKKA_SYUSAI_DATE")]
        [MaxLength(8)]
        public string YakkaSyusaiDate { get; set; }

        /// <summary>
        /// 経過措置期限日付
        /// 
        /// </summary>
        [Column("KEIKASOTI_DATE")]
        [MaxLength(8)]
        public string KeikasotiDate { get; set; }

        /// <summary>
        /// 代表医薬品コード
        /// 医薬品を成分と投与経路別にグループ化するコード
        /// </summary>
        [Column("MAIN_DRUG_CD")]
        [MaxLength(8)]
        public string MainDrugCd { get; set; }

        /// <summary>
        /// 代表医薬品名
        /// 
        /// </summary>
        [Column("MAIN_DRUG_NAME")]
        [MaxLength(400)]
        public string MainDrugName { get; set; }

        /// <summary>
        /// 代表医薬品名読み
        /// 
        /// </summary>
        [Column("MAIN_DRUG_KANA")]
        [MaxLength(400)]
        public string MainDrugKana { get; set; }

        /// <summary>
        /// キー成分
        /// 
        /// </summary>
        [Column("KEY_SEIBUN")]
        [MaxLength(200)]
        public string KeySeibun { get; set; }

        /// <summary>
        /// 配合剤フラグ
        /// 1:配合剤 Null:1:配合剤でない
        /// </summary>
        [Column("HAIGO_FLG")]
        [MaxLength(1)]
        public string HaigoFlg { get; set; }

        /// <summary>
        /// 代表医薬品名決定フラグ
        /// 1:代表医薬品名候補となる銘柄が2銘柄以上
        ///                    Null:代表医薬品名銘柄が1銘柄のみ          
        /// </summary>
        [Column("MAIN_DRUG_NAME_FLG")]
        [MaxLength(1)]
        public string MainDrugNameFlg { get; set; }

    }
}
