using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m28_drug_mst")]
    public class M28DrugMst : EmrCloneable<M28DrugMst>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 医薬品コード
        /// 
        /// </summary>

        [Column("yj_cd", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 厚生労働省コード
        /// 
        /// </summary>
        [Column("koseisyo_cd")]
        [MaxLength(12)]
        public string? KoseisyoCd { get; set; } = string.Empty;

        /// <summary>
        /// 基金コード
        /// 
        /// </summary>
        [Column("kikin_cd")]
        [MaxLength(9)]
        public string? KikinCd { get; set; } = string.Empty;

        /// <summary>
        /// 製品名
        /// 
        /// </summary>
        [Column("drug_name")]
        [MaxLength(200)]
        public string? DrugName { get; set; } = string.Empty;

        /// <summary>
        /// 医薬品名称読み１
        /// 
        /// </summary>
        [Column("drug_kana1")]
        [MaxLength(100)]
        public string? DrugKana1 { get; set; } = string.Empty;

        /// <summary>
        /// 医薬品名称読み２
        /// 
        /// </summary>
        [Column("drug_kana2")]
        [MaxLength(100)]
        public string? DrugKana2 { get; set; } = string.Empty;

        /// <summary>
        /// 一般名
        /// 
        /// </summary>
        [Column("ipn_name")]
        [MaxLength(400)]
        public string? IpnName { get; set; } = string.Empty;

        /// <summary>
        /// 一般名読み
        /// 
        /// </summary>
        [Column("ipn_kana")]
        [MaxLength(100)]
        public string? IpnKana { get; set; } = string.Empty;

        /// <summary>
        /// 薬価単位数量
        /// 
        /// </summary>
        [Column("yakka_val")]
        public int YakkaVal { get; set; }

        /// <summary>
        /// 薬価単位
        /// 
        /// </summary>
        [Column("yakka_unit")]
        [MaxLength(20)]
        public string? YakkaUnit { get; set; } = string.Empty;

        /// <summary>
        /// 成分量・力価
        /// Y
        /// </summary>
        [Column("seibun_rikika")]
        public double SeibunRikika { get; set; }

        /// <summary>
        /// 成分量・力価単位
        /// 
        /// </summary>
        [Column("seibun_rikika_unit")]
        [MaxLength(30)]
        public string? SeibunRikikaUnit { get; set; } = string.Empty;

        /// <summary>
        /// 容量・重量
        /// Y
        /// </summary>
        [Column("yoryo_jyuryo")]
        public double YoryoJyuryo { get; set; }

        /// <summary>
        /// 容量・重量単位
        /// 
        /// </summary>
        [Column("yoryo_jyuryo_unit")]
        [MaxLength(20)]
        public string? YoryoJyuryoUnit { get; set; } = string.Empty;

        /// <summary>
        /// 成分量・力価/用量・重量比
        /// Y
        /// </summary>
        [Column("seiriki_yoryo_rate")]
        public double SeirikiYoryoRate { get; set; }

        /// <summary>
        /// 成分量・力価/用量・重量比単位
        /// 
        /// </summary>
        [Column("seiriki_yoryo_unit")]
        [MaxLength(40)]
        public string? SeirikiYoryoUnit { get; set; } = string.Empty;

        /// <summary>
        /// 製造販売承認メーカーコード
        /// 
        /// </summary>
        [Column("maker_cd")]
        [MaxLength(4)]
        public string? MakerCd { get; set; } = string.Empty;

        /// <summary>
        /// 製造販売承認メーカー
        /// 
        /// </summary>
        [Column("maker_name")]
        [MaxLength(40)]
        public string? MakerName { get; set; } = string.Empty;

        /// <summary>
        /// 薬剤区分コード
        /// 
        /// </summary>
        [Column("drug_kbn_cd")]
        [MaxLength(1)]
        public string? DrugKbnCd { get; set; } = string.Empty;

        /// <summary>
        /// 薬剤区分
        /// 
        /// </summary>
        [Column("drug_kbn")]
        [MaxLength(10)]
        public string? DrugKbn { get; set; } = string.Empty;

        /// <summary>
        /// 剤形区分コード
        /// 1:内用薬 4:注射薬 6:外用薬
        /// </summary>
        [Column("form_kbn_cd")]
        [MaxLength(3)]
        public string? FormKbnCd { get; set; } = string.Empty;

        /// <summary>
        /// 剤形区分
        /// 
        /// </summary>
        [Column("form_kbn")]
        [MaxLength(100)]
        public string? FormKbn { get; set; } = string.Empty;

        /// <summary>
        /// 毒薬フラグ
        /// 1:毒薬 Null:毒薬でない
        /// </summary>
        [Column("dokuyaku_flg")]
        [MaxLength(1)]
        public string? DokuyakuFlg { get; set; } = string.Empty;

        /// <summary>
        /// 劇薬フラグ
        /// 1:劇薬 Null:劇薬でない
        /// </summary>
        [Column("gekiyaku_flg")]
        [MaxLength(1)]
        public string? GekiyakuFlg { get; set; } = string.Empty;

        /// <summary>
        /// 麻薬フラグ
        /// 1:麻薬 Null:麻薬でない
        /// </summary>
        [Column("mayaku_flg")]
        [MaxLength(1)]
        public string? MayakuFlg { get; set; } = string.Empty;

        /// <summary>
        /// 向精神薬フラグ
        /// 1:向精神薬 Null:向精神薬でない
        /// </summary>
        [Column("koseisinyaku_flg")]
        [MaxLength(1)]
        public string? KoseisinyakuFlg { get; set; } = string.Empty;

        /// <summary>
        /// 覚醒剤フラグ
        /// 1:覚醒剤 Null:覚醒剤でない
        /// </summary>
        [Column("kakuseizai_flg")]
        [MaxLength(1)]
        public string? KakuseizaiFlg { get; set; } = string.Empty;

        /// <summary>
        /// 覚醒剤原料フラグ
        /// 1:覚醒剤原料 Null:覚醒剤原料でない
        /// </summary>
        [Column("kakuseizai_genryo_flg")]
        [MaxLength(1)]
        public string? KakuseizaiGenryoFlg { get; set; } = string.Empty;

        /// <summary>
        /// 生物由来製品フラグ
        /// 1:生物由来製品 Null:生物由来製品でない
        /// </summary>
        [Column("seibutu_flg")]
        [MaxLength(1)]
        public string? SeibutuFlg { get; set; } = string.Empty;

        /// <summary>
        /// 特定生物由来製品フラグ
        /// 1:特定生物由来製品区分 Null:特定生物由来製品区分でない
        /// </summary>
        [Column("sp_seibutu_flg")]
        [MaxLength(1)]
        public string? SpSeibutuFlg { get; set; } = string.Empty;

        /// <summary>
        /// 後発品フラグ
        /// 1:後発品である Null:後発品でない
        /// </summary>
        [Column("kohatu_flg")]
        [MaxLength(1)]
        public string? KohatuFlg { get; set; } = string.Empty;

        /// <summary>
        /// 薬価
        /// Y
        /// </summary>
        [Column("yakka")]
        public double Yakka { get; set; }

        /// <summary>
        /// 規格単位
        /// 
        /// </summary>
        [Column("kikaku_unit")]
        [MaxLength(100)]
        public string? KikakuUnit { get; set; } = string.Empty;

        /// <summary>
        /// 薬価換算式
        /// 
        /// </summary>
        [Column("yakka_rate_formura")]
        [MaxLength(30)]
        public string? YakkaRateFormura { get; set; } = string.Empty;

        /// <summary>
        /// 薬価換算可能単位
        /// 
        /// </summary>
        [Column("yakka_rate_unit")]
        [MaxLength(40)]
        public string? YakkaRateUnit { get; set; } = string.Empty;

        /// <summary>
        /// 薬価基準収載日付
        /// 
        /// </summary>
        [Column("yakka_syusai_date")]
        [MaxLength(8)]
        public string? YakkaSyusaiDate { get; set; } = string.Empty;

        /// <summary>
        /// 経過措置期限日付
        /// 
        /// </summary>
        [Column("keikasoti_date")]
        [MaxLength(8)]
        public string? KeikasotiDate { get; set; } = string.Empty;

        /// <summary>
        /// 代表医薬品コード
        /// 医薬品を成分と投与経路別にグループ化するコード
        /// </summary>
        [Column("main_drug_cd")]
        [MaxLength(8)]
        public string? MainDrugCd { get; set; } = string.Empty;

        /// <summary>
        /// 代表医薬品名
        /// 
        /// </summary>
        [Column("main_drug_name")]
        [MaxLength(400)]
        public string? MainDrugName { get; set; } = string.Empty;

        /// <summary>
        /// 代表医薬品名読み
        /// 
        /// </summary>
        [Column("main_drug_kana")]
        [MaxLength(400)]
        public string? MainDrugKana { get; set; } = string.Empty;

        /// <summary>
        /// キー成分
        /// 
        /// </summary>
        [Column("key_seibun")]
        [MaxLength(200)]
        public string? KeySeibun { get; set; } = string.Empty;

        /// <summary>
        /// 配合剤フラグ
        /// 1:配合剤 Null:1:配合剤でない
        /// </summary>
        [Column("haigo_flg")]
        [MaxLength(1)]
        public string? HaigoFlg { get; set; } = string.Empty;

        /// <summary>
        /// 代表医薬品名決定フラグ
        /// 1:代表医薬品名候補となる銘柄が2銘柄以上
        ///                    Null:代表医薬品名銘柄が1銘柄のみ          
        /// </summary>
        [Column("main_drug_name_flg")]
        [MaxLength(1)]
        public string? MainDrugNameFlg { get; set; } = string.Empty;
    }
}
