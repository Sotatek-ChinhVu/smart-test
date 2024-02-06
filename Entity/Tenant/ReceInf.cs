using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rece_inf")]
    public class ReceInf : EmrCloneable<ReceInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        
        [Column("seikyu_ym", Order = 2)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("sin_ym", Order = 4)]
        public int SinYm { get; set; }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        
        [Column("hoken_id", Order = 5)]
        public int HokenId { get; set; }

        /// <summary>
        /// 主保険保険ID2
        ///         社保生保+生保単独等で複数の保険IDを結合した場合に使用
        /// </summary>
        [Column("hoken_id2")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenId2 { get; set; }

        /// <summary>
        /// 公費１保険ID
        /// 
        /// </summary>
        [Column("kohi1_id")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Id { get; set; }

        /// <summary>
        /// 公費２保険ID
        /// 
        /// </summary>
        [Column("kohi2_id")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Id { get; set; }

        /// <summary>
        /// 公費３保険ID
        /// 
        /// </summary>
        [Column("kohi3_id")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Id { get; set; }

        /// <summary>
        /// 公費４保険ID
        /// 
        /// </summary>
        [Column("kohi4_id")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Id { get; set; }

        /// <summary>
        /// 保険区分
        ///     0:自費
        ///     1:社保          
        ///     2:国保          
        ///     11:労災(短期給付)          
        ///     12:労災(傷病年金)          
        ///     13:アフターケア          
        ///     14:自賠責          
        /// </summary>
        [Column("hoken_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 保険種別コード
        ///     0: 下記以外
        ///         左から          
        ///             1桁目 - 1:社保 2:国保 3:後期 4:退職 5:公費          
        ///             2桁目 - 組合せ数          
        ///             3桁目 - 1:単独 2:２併 .. 5:５併          
        ///         例) 社保単独     = 111    
        ///             社保２併(54)     = 122    
        ///             社保２併(マル長+54)     = 132    
        ///             国保単独     = 211    
        ///             国保２併(54)     = 222    
        ///             国保２併(マル長+54)     = 232    
        ///             公費単独(12)     = 511    
        ///             公費２併(21+12)     = 522    
        /// </summary>
        [Column("hoken_sbt_cd")]
        public int HokenSbtCd { get; set; }

        /// <summary>
        /// レセプト種別
        ///     11x2: 本人
        ///     11x4: 未就学者          
        ///     11x6: 家族          
        ///     11x8: 高齢一般・低所          
        ///     11x0: 高齢７割          
        ///     12x2: 公費          
        ///     13x8: 後期一般・低所          
        ///     13x0: 後期７割          
        ///     14x2: 退職本人          
        ///     14x4: 退職未就学者          
        ///     14x6: 退職家族          
        /// </summary>
        [Column("rece_sbt")]
        [MaxLength(4)]
        public string? ReceSbt { get; set; } = string.Empty;

        /// <summary>
        /// 保険者番号
        /// 
        /// </summary>
        [Column("hokensya_no")]
        [MaxLength(8)]
        public string? HokensyaNo { get; set; } = string.Empty;

        /// <summary>
        /// 法別番号
        /// 
        /// </summary>
        [Column("houbetu")]
        [MaxLength(3)]
        public string? Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        [Column("kohi1_houbetu")]
        [MaxLength(3)]
        public string? Kohi1Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        [Column("kohi2_houbetu")]
        [MaxLength(3)]
        public string? Kohi2Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        [Column("kohi3_houbetu")]
        [MaxLength(3)]
        public string? Kohi3Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        [Column("kohi4_houbetu")]
        [MaxLength(3)]
        public string? Kohi4Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 本人家族区分
        /// 1:本人 2:家族
        /// </summary>
        [Column("honke_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int HonkeKbn { get; set; }

        /// <summary>
        /// 高額療養費区分
        /// <70歳以上>
        ///     0:一般         
        ///     3:上位(～2018/07)         
        ///     4:低所Ⅱ         
        ///     5:低所Ⅰ         
        ///     6:特定収入(～2008/12)         
        ///     26:現役Ⅲ         
        ///     27:現役Ⅱ         
        ///     28:現役Ⅰ         
        /// <70歳未満>          
        ///     0:限度額認定証なし         
        ///     17:上位[A] (～2014/12)         
        ///     18:一般[B] (～2014/12)         
        ///     19:低所[C] (～2014/12)         
        ///     26:区ア／標準報酬月額83万円以上         
        ///     27:区イ／標準報酬月額53..79万円         
        ///     28:区ウ／標準報酬月額28..50万円         
        ///     29:区エ／標準報酬月額26万円以下         
        ///     30:区オ／低所得者         
        /// </summary>
        [Column("kogaku_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKbn { get; set; }

        /// <summary>
        /// 高額療養費適用区分
        /// 
        /// </summary>
        [Column("kogaku_tekiyo_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuTekiyoKbn { get; set; }

        /// <summary>
        /// 限度額特例フラグ
        /// 
        /// </summary>
        [Column("is_tokurei")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTokurei { get; set; }

        /// <summary>
        /// 多数回該当フラグ
        /// 
        /// </summary>
        [Column("is_tasukai")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTasukai { get; set; }

        /// <summary>
        /// 高額療養費公１限度額
        /// 
        /// </summary>
        [Column("kogaku_kohi1_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKohi1Limit { get; set; }

        /// <summary>
        /// 高額療養費公２限度額
        /// 
        /// </summary>
        [Column("kogaku_kohi2_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKohi2Limit { get; set; }

        /// <summary>
        /// 高額療養費公３限度額
        /// 
        /// </summary>
        [Column("kogaku_kohi3_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKohi3Limit { get; set; }

        /// <summary>
        /// 高額療養費公４限度額
        /// 
        /// </summary>
        [Column("kogaku_kohi4_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKohi4Limit { get; set; }

        /// <summary>
        /// 高額療養費限度額(合算)
        /// 
        /// </summary>
        [Column("total_kogaku_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalKogakuLimit { get; set; }

        /// <summary>
        /// 国保減免区分
        ///     1:減額 2:免除 3:支払猶予 4:自立支援減免
        /// </summary>
        [Column("genmen_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenKbn { get; set; }

        /// <summary>
        /// 保険負担率
        /// 
        /// </summary>
        [Column("hoken_rate")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenRate { get; set; }

        /// <summary>
        /// 患者負担率
        /// 
        /// </summary>
        [Column("pt_rate")]
        [CustomAttribute.DefaultValue(0)]
        public int PtRate { get; set; }

        /// <summary>
        /// 点数単価
        /// 
        /// </summary>
        [Column("en_ten")]
        [CustomAttribute.DefaultValue(0)]
        public double EnTen { get; set; }

        /// <summary>
        /// 公１負担限度額
        /// 
        /// </summary>
        [Column("kohi1_limit")]
        public int Kohi1Limit { get; set; }

        /// <summary>
        /// 公１他院負担額
        /// 
        /// </summary>
        [Column("kohi1_other_futan")]
        public int Kohi1OtherFutan { get; set; }

        /// <summary>
        /// 公２負担限度額
        /// 
        /// </summary>
        [Column("kohi2_limit")]
        public int Kohi2Limit { get; set; }

        /// <summary>
        /// 公２他院負担額
        /// 
        /// </summary>
        [Column("kohi2_other_futan")]
        public int Kohi2OtherFutan { get; set; }

        /// <summary>
        /// 公３負担限度額
        /// 
        /// </summary>
        [Column("kohi3_limit")]
        public int Kohi3Limit { get; set; }

        /// <summary>
        /// 公３他院負担額
        /// 
        /// </summary>
        [Column("kohi3_other_futan")]
        public int Kohi3OtherFutan { get; set; }

        /// <summary>
        /// 公４負担限度額
        /// 
        /// </summary>
        [Column("kohi4_limit")]
        public int Kohi4Limit { get; set; }

        /// <summary>
        /// 公４他院負担額
        /// 
        /// </summary>
        [Column("kohi4_other_futan")]
        public int Kohi4OtherFutan { get; set; }

        /// <summary>
        /// 診療点数
        /// 
        /// </summary>
        [Column("tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int Tensu { get; set; }

        /// <summary>
        /// 総医療費
        /// 
        /// </summary>
        [Column("total_iryohi")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalIryohi { get; set; }

        /// <summary>
        /// 保険負担額
        /// 
        /// </summary>
        [Column("hoken_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenFutan { get; set; }

        /// <summary>
        /// 高額負担額
        /// 
        /// </summary>
        [Column("kogaku_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuFutan { get; set; }

        /// <summary>
        /// 公１負担額
        /// 
        /// </summary>
        [Column("kohi1_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Futan { get; set; }

        /// <summary>
        /// 公２負担額
        /// 
        /// </summary>
        [Column("kohi2_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Futan { get; set; }

        /// <summary>
        /// 公３負担額
        /// 
        /// </summary>
        [Column("kohi3_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Futan { get; set; }

        /// <summary>
        /// 公４負担額
        /// 
        /// </summary>
        [Column("kohi4_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Futan { get; set; }

        /// <summary>
        /// 一部負担額
        /// 
        /// </summary>
        [Column("ichibu_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int IchibuFutan { get; set; }

        /// <summary>
        /// 減免額
        /// 
        /// </summary>
        [Column("genmen_gaku")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenGaku { get; set; }

        /// <summary>
        /// 保険負担額10円単位
        /// 
        /// </summary>
        [Column("hoken_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenFutan10en { get; set; }

        /// <summary>
        /// 高額負担額10円単位
        /// 
        /// </summary>
        [Column("kogaku_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuFutan10en { get; set; }

        /// <summary>
        /// 公１負担額10円単位
        /// 
        /// </summary>
        [Column("kohi1_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Futan10en { get; set; }

        /// <summary>
        /// 公２負担額10円単位
        /// 
        /// </summary>
        [Column("kohi2_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Futan10en { get; set; }

        /// <summary>
        /// 公３負担額10円単位
        /// 
        /// </summary>
        [Column("kohi3_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Futan10en { get; set; }

        /// <summary>
        /// 公４負担額10円単位
        /// 
        /// </summary>
        [Column("kohi4_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Futan10en { get; set; }

        /// <summary>
        /// 一部負担額10円単位
        /// 
        /// </summary>
        [Column("ichibu_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int IchibuFutan10en { get; set; }

        /// <summary>
        /// 減免額10円単位
        /// 
        /// </summary>
        [Column("genmen_gaku_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenGaku10en { get; set; }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        [Column("pt_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int PtFutan { get; set; }

        /// <summary>
        /// 高額療養費超過区分
        /// 
        /// </summary>
        [Column("kogaku_over_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuOverKbn { get; set; }

        /// <summary>
        /// 保険分点数
        /// 
        /// </summary>
        [Column("hoken_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenTensu { get; set; }

        /// <summary>
        /// 保険分一部負担額
        /// 
        /// </summary>
        [Column("hoken_ichibu_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenIchibuFutan { get; set; }

        /// <summary>
        /// 保険分一部負担額10円単位
        /// 
        /// </summary>
        [Column("hoken_ichibu_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenIchibuFutan10en { get; set; }

        /// <summary>
        /// 公１分点数
        /// 
        /// </summary>
        [Column("kohi1_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Tensu { get; set; }

        /// <summary>
        /// 公１分一部負担相当額
        /// 
        /// </summary>
        [Column("kohi1_ichibu_sotogaku")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1IchibuSotogaku { get; set; }

        /// <summary>
        /// 公１分一部負担相当額10円単位
        /// 
        /// </summary>
        [Column("kohi1_ichibu_sotogaku_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1IchibuSotogaku10en { get; set; }

        /// <summary>
        /// 公１分一部負担額
        /// 
        /// </summary>
        [Column("kohi1_ichibu_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1IchibuFutan { get; set; }

        /// <summary>
        /// 公２分点数
        /// 
        /// </summary>
        [Column("kohi2_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Tensu { get; set; }

        /// <summary>
        /// 公２分一部負担相当額
        /// 
        /// </summary>
        [Column("kohi2_ichibu_sotogaku")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2IchibuSotogaku { get; set; }

        /// <summary>
        /// 公２分一部負担相当額10円単位
        /// 
        /// </summary>
        [Column("kohi2_ichibu_sotogaku_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2IchibuSotogaku10en { get; set; }

        /// <summary>
        /// 公２分一部負担額
        /// 
        /// </summary>
        [Column("kohi2_ichibu_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2IchibuFutan { get; set; }

        /// <summary>
        /// 公３分点数
        /// 
        /// </summary>
        [Column("kohi3_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Tensu { get; set; }

        /// <summary>
        /// 公３分一部負担相当額
        /// 
        /// </summary>
        [Column("kohi3_ichibu_sotogaku")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3IchibuSotogaku { get; set; }

        /// <summary>
        /// 公３分一部負担相当額10円単位
        /// 
        /// </summary>
        [Column("kohi3_ichibu_sotogaku_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3IchibuSotogaku10en { get; set; }

        /// <summary>
        /// 公３分一部負担額
        /// 
        /// </summary>
        [Column("kohi3_ichibu_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3IchibuFutan { get; set; }

        /// <summary>
        /// 公４分点数
        /// 
        /// </summary>
        [Column("kohi4_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Tensu { get; set; }

        /// <summary>
        /// 公４分一部負担相当額
        /// 
        /// </summary>
        [Column("kohi4_ichibu_sotogaku")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4IchibuSotogaku { get; set; }

        /// <summary>
        /// 公４分一部負担相当額10円単位
        ///
        /// </summary>
        [Column("kohi4_ichibu_sotogaku_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4IchibuSotogaku10en { get; set; }

        /// <summary>
        /// 公４分一部負担額
        /// 
        /// </summary>
        [Column("kohi4_ichibu_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4IchibuFutan { get; set; }

        /// <summary>
        /// 合算対象一部負担額
        ///
        /// </summary>
        [Column("total_ichibu_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalIchibuFutan { get; set; }

        /// <summary>
        /// 合算対象一部負担額10円単位
        ///
        /// </summary>
        [Column("total_ichibu_futan_10en")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalIchibuFutan10en { get; set; }

        /// <summary>
        /// 保険レセ点数
        /// 
        /// </summary>
        [Column("hoken_rece_tensu")]
        public int? HokenReceTensu { get; set; }

        /// <summary>
        /// 保険レセ負担額
        /// 
        /// </summary>
        [Column("hoken_rece_futan")]
        public int? HokenReceFutan { get; set; }

        /// <summary>
        /// 公１レセ点数
        /// 
        /// </summary>
        [Column("kohi1_rece_tensu")]
        public int? Kohi1ReceTensu { get; set; }

        /// <summary>
        /// 公１レセ負担額
        /// 
        /// </summary>
        [Column("kohi1_rece_futan")]
        public int? Kohi1ReceFutan { get; set; }

        /// <summary>
        /// 公１レセ給付対象額
        /// 
        /// </summary>
        [Column("kohi1_rece_kyufu")]
        public int? Kohi1ReceKyufu { get; set; }

        /// <summary>
        /// 公２レセ点数
        /// 
        /// </summary>
        [Column("kohi2_rece_tensu")]
        public int? Kohi2ReceTensu { get; set; }

        /// <summary>
        /// 公２レセ負担額
        /// 
        /// </summary>
        [Column("kohi2_rece_futan")]
        public int? Kohi2ReceFutan { get; set; }

        /// <summary>
        /// 公２レセ給付対象額
        /// 
        /// </summary>
        [Column("kohi2_rece_kyufu")]
        public int? Kohi2ReceKyufu { get; set; }

        /// <summary>
        /// 公３レセ点数
        /// 
        /// </summary>
        [Column("kohi3_rece_tensu")]
        public int? Kohi3ReceTensu { get; set; }

        /// <summary>
        /// 公３レセ負担額
        /// 
        /// </summary>
        [Column("kohi3_rece_futan")]
        public int? Kohi3ReceFutan { get; set; }

        /// <summary>
        /// 公３レセ給付対象額
        /// 
        /// </summary>
        [Column("kohi3_rece_kyufu")]
        public int? Kohi3ReceKyufu { get; set; }

        /// <summary>
        /// 公４レセ点数
        /// 
        /// </summary>
        [Column("kohi4_rece_tensu")]
        public int? Kohi4ReceTensu { get; set; }

        /// <summary>
        /// 公４レセ負担額
        /// 
        /// </summary>
        [Column("kohi4_rece_futan")]
        public int? Kohi4ReceFutan { get; set; }

        /// <summary>
        /// 公４レセ給付対象額
        /// 
        /// </summary>
        [Column("kohi4_rece_kyufu")]
        public int? Kohi4ReceKyufu { get; set; }

        /// <summary>
        /// 保険実日数
        /// 
        /// </summary>
        [Column("hoken_nissu")]
        public int? HokenNissu { get; set; }

        /// <summary>
        /// 公１実日数
        /// 
        /// </summary>
        [Column("kohi1_nissu")]
        public int? Kohi1Nissu { get; set; }

        /// <summary>
        /// 公２実日数
        /// 
        /// </summary>
        [Column("kohi2_nissu")]
        public int? Kohi2Nissu { get; set; }

        /// <summary>
        /// 公３実日数
        /// 
        /// </summary>
        [Column("kohi3_nissu")]
        public int? Kohi3Nissu { get; set; }

        /// <summary>
        /// 公４実日数
        /// 
        /// </summary>
        [Column("kohi4_nissu")]
        public int? Kohi4Nissu { get; set; }

        /// <summary>
        /// 公１レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        [Column("kohi1_rece_kisai")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1ReceKisai { get; set; }

        /// <summary>
        /// 公２レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        [Column("kohi2_rece_kisai")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2ReceKisai { get; set; }

        /// <summary>
        /// 公３レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        [Column("kohi3_rece_kisai")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3ReceKisai { get; set; }

        /// <summary>
        /// 公４レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        [Column("kohi4_rece_kisai")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4ReceKisai { get; set; }

        /// <summary>
        /// 公１制度略号
        /// 
        /// </summary>
        [Column("kohi1_name_cd")]
        [MaxLength(5)]
        public string? Kohi1NameCd { get; set; } = string.Empty;

        /// <summary>
        /// 公２制度略号
        /// 
        /// </summary>
        [Column("kohi2_name_cd")]
        [MaxLength(5)]
        public string? Kohi2NameCd { get; set; } = string.Empty;

        /// <summary>
        /// 公３制度略号
        /// 
        /// </summary>
        [Column("kohi3_name_cd")]
        [MaxLength(5)]
        public string? Kohi3NameCd { get; set; } = string.Empty;

        /// <summary>
        /// 公４制度略号
        /// 
        /// </summary>
        [Column("kohi4_name_cd")]
        [MaxLength(5)]
        public string? Kohi4NameCd { get; set; } = string.Empty;

        /// <summary>
        /// 請求区分
        /// 1:月遅れ 2:返戻 3:オンライン返戻
        /// </summary>
        [Column("seikyu_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuKbn { get; set; }

        /// <summary>
        /// 特記事項
        /// 
        /// </summary>
        [Column("tokki")]
        [MaxLength(10)]
        public string? Tokki { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項１
        /// 
        /// </summary>
        [Column("tokki1")]
        [MaxLength(10)]
        public string? Tokki1 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項２
        /// 
        /// </summary>
        [Column("tokki2")]
        [MaxLength(10)]
        public string? Tokki2 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項３
        /// 
        /// </summary>
        [Column("tokki3")]
        [MaxLength(10)]
        public string? Tokki3 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項４
        /// 
        /// </summary>
        [Column("tokki4")]
        [MaxLength(10)]
        public string? Tokki4 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項５
        /// 
        /// </summary>
        [Column("tokki5")]
        [MaxLength(10)]
        public string? Tokki5 { get; set; } = string.Empty;

        /// <summary>
        /// 患者の状態
        /// 
        /// </summary>
        [Column("pt_status")]
        [MaxLength(60)]
        public string? PtStatus { get; set; } = string.Empty;

        /// <summary>
        /// 労災イ点数
        /// 
        /// </summary>
        [Column("rousai_i_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiITensu { get; set; }

        /// <summary>
        /// 労災イ点負担額
        /// 
        /// </summary>
        [Column("rousai_i_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiIFutan { get; set; }

        /// <summary>
        /// 労災ロ円負担額
        /// 
        /// </summary>
        [Column("rousai_ro_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiRoFutan { get; set; }

        /// <summary>
        /// 自賠イ技術点数
        /// 
        /// </summary>
        [Column("jibai_i_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiITensu { get; set; }

        /// <summary>
        /// 自賠ロ薬剤点数
        /// 
        /// </summary>
        [Column("jibai_ro_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiRoTensu { get; set; }

        /// <summary>
        /// 自賠ハ円診察負担額
        /// 
        /// </summary>
        [Column("jibai_ha_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHaFutan { get; set; }

        /// <summary>
        /// 自賠ニ円他負担額
        /// 
        /// </summary>
        [Column("jibai_ni_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiNiFutan { get; set; }

        /// <summary>
        /// 自賠ホ診断書料
        /// 
        /// </summary>
        [Column("jibai_ho_sindan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHoSindan { get; set; }

        /// <summary>
        /// 自賠ホ診断書料枚数
        /// 
        /// </summary>
        [Column("jibai_ho_sindan_count")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHoSindanCount { get; set; }

        /// <summary>
        /// 自賠ヘ明細書料
        /// 
        /// </summary>
        [Column("jibai_he_meisai")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHeMeisai { get; set; }

        /// <summary>
        /// 自賠ヘ明細書料枚数
        /// 
        /// </summary>
        [Column("jibai_he_meisai_count")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHeMeisaiCount { get; set; }

        /// <summary>
        /// 自賠Ａ負担額
        /// 
        /// </summary>
        [Column("jibai_a_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiAFutan { get; set; }

        /// <summary>
        /// 自賠Ｂ負担額
        /// 
        /// </summary>
        [Column("jibai_b_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiBFutan { get; set; }

        /// <summary>
        /// 自賠Ｃ負担額
        /// 
        /// </summary>
        [Column("jibai_c_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiCFutan { get; set; }

        /// <summary>
        /// 自賠Ｄ負担額
        /// 
        /// </summary>
        [Column("jibai_d_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiDFutan { get; set; }

        /// <summary>
        /// 自賠健保点数
        /// 
        /// </summary>
        [Column("jibai_kenpo_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiKenpoTensu { get; set; }

        /// <summary>
        /// 自賠健保負担額
        /// 
        /// </summary>
        [Column("jibai_kenpo_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiKenpoFutan { get; set; }

        /// <summary>
        /// 新継再別
        /// 
        /// </summary>
        [Column("sinkei")]
        [CustomAttribute.DefaultValue(0)]
        public int Sinkei { get; set; }

        /// <summary>
        /// 転帰事由
        /// 
        /// </summary>
        [Column("tenki")]
        [CustomAttribute.DefaultValue(0)]
        public int Tenki { get; set; }

        /// <summary>
        /// 診療科ID
        /// 
        /// </summary>
        [Column("ka_id")]
        [CustomAttribute.DefaultValue(0)]
        public int KaId { get; set; }

        /// <summary>
        /// 担当医ID
        /// 
        /// </summary>
        [Column("tanto_id")]
        [CustomAttribute.DefaultValue(0)]
        public int TantoId { get; set; }

        /// <summary>
        /// テスト患者区分
        ///     1:テスト患者区分
        /// </summary>
        [Column("is_tester")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTester { get; set; }

        /// <summary>
        /// 在医総フラグ
        /// 1:1:在医総管又は在医総
        /// </summary>
        [Column("is_zaiiso")]
        [CustomAttribute.DefaultValue(0)]
        public int IsZaiiso { get; set; }

        /// <summary>
        /// マル長フラグ
        ///     0:なし
        ///     1:あり(上限未満)
        ///     2:あり(上限超)
        /// </summary>
        [Column("choki_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int ChokiKbn { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
    }
}
