using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    [Table(name: "RECE_INF")]
    public class ReceInf : EmrCloneable<ReceInf>
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
        /// 請求年月
        /// 
        /// </summary>
        //[Key]
        [Column("SEIKYU_YM", Order = 2)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 3)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        //[Key]
        [Column("SIN_YM", Order = 4)]
        public int SinYm { get; set; }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        //[Key]
        [Column("HOKEN_ID", Order = 5)]
        public int HokenId { get; set; }

        /// <summary>
        /// 主保険保険ID2
        ///         社保生保+生保単独等で複数の保険IDを結合した場合に使用
        /// </summary>
        [Column("HOKEN_ID2")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenId2 { get; set; }

        /// <summary>
        /// 公費１保険ID
        /// 
        /// </summary>
        [Column("KOHI1_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Id { get; set; }

        /// <summary>
        /// 公費２保険ID
        /// 
        /// </summary>
        [Column("KOHI2_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Id { get; set; }

        /// <summary>
        /// 公費３保険ID
        /// 
        /// </summary>
        [Column("KOHI3_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Id { get; set; }

        /// <summary>
        /// 公費４保険ID
        /// 
        /// </summary>
        [Column("KOHI4_ID")]
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
        [Column("HOKEN_KBN")]
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
        [Column("HOKEN_SBT_CD")]
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
        [Column("RECE_SBT")]
        [MaxLength(4)]
        public string ReceSbt { get; set; } = string.Empty;

        /// <summary>
        /// 保険者番号
        /// 
        /// </summary>
        [Column("HOKENSYA_NO")]
        [MaxLength(8)]
        public string HokensyaNo { get; set; } = string.Empty;

        /// <summary>
        /// 法別番号
        /// 
        /// </summary>
        [Column("HOUBETU")]
        [MaxLength(3)]
        public string Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        [Column("KOHI1_HOUBETU")]
        [MaxLength(3)]
        public string Kohi1Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        [Column("KOHI2_HOUBETU")]
        [MaxLength(3)]
        public string Kohi2Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        [Column("KOHI3_HOUBETU")]
        [MaxLength(3)]
        public string Kohi3Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        [Column("KOHI4_HOUBETU")]
        [MaxLength(3)]
        public string Kohi4Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 本人家族区分
        /// 1:本人 2:家族
        /// </summary>
        [Column("HONKE_KBN")]
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
        [Column("KOGAKU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKbn { get; set; }

        /// <summary>
        /// 高額療養費適用区分
        /// 
        /// </summary>
        [Column("KOGAKU_TEKIYO_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuTekiyoKbn { get; set; }

        /// <summary>
        /// 限度額特例フラグ
        /// 
        /// </summary>
        [Column("IS_TOKUREI")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTokurei { get; set; }

        /// <summary>
        /// 多数回該当フラグ
        /// 
        /// </summary>
        [Column("IS_TASUKAI")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTasukai { get; set; }

        /// <summary>
        /// 高額療養費公１限度額
        /// 
        /// </summary>
        [Column("KOGAKU_KOHI1_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKohi1Limit { get; set; }

        /// <summary>
        /// 高額療養費公２限度額
        /// 
        /// </summary>
        [Column("KOGAKU_KOHI2_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKohi2Limit { get; set; }

        /// <summary>
        /// 高額療養費公３限度額
        /// 
        /// </summary>
        [Column("KOGAKU_KOHI3_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKohi3Limit { get; set; }

        /// <summary>
        /// 高額療養費公４限度額
        /// 
        /// </summary>
        [Column("KOGAKU_KOHI4_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKohi4Limit { get; set; }

        /// <summary>
        /// 高額療養費限度額(合算)
        /// 
        /// </summary>
        [Column("TOTAL_KOGAKU_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalKogakuLimit { get; set; }

        /// <summary>
        /// 国保減免区分
        ///     1:減額 2:免除 3:支払猶予 4:自立支援減免
        /// </summary>
        [Column("GENMEN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenKbn { get; set; }

        /// <summary>
        /// 保険負担率
        /// 
        /// </summary>
        [Column("HOKEN_RATE")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenRate { get; set; }

        /// <summary>
        /// 患者負担率
        /// 
        /// </summary>
        [Column("PT_RATE")]
        [CustomAttribute.DefaultValue(0)]
        public int PtRate { get; set; }

        /// <summary>
        /// 点数単価
        /// 
        /// </summary>
        [Column("EN_TEN")]
        [CustomAttribute.DefaultValue(0)]
        public int EnTen { get; set; }

        /// <summary>
        /// 公１負担限度額
        /// 
        /// </summary>
        [Column("KOHI1_LIMIT")]
        public int Kohi1Limit { get; set; }

        /// <summary>
        /// 公１他院負担額
        /// 
        /// </summary>
        [Column("KOHI1_OTHER_FUTAN")]
        public int Kohi1OtherFutan { get; set; }

        /// <summary>
        /// 公２負担限度額
        /// 
        /// </summary>
        [Column("KOHI2_LIMIT")]
        public int Kohi2Limit { get; set; }

        /// <summary>
        /// 公２他院負担額
        /// 
        /// </summary>
        [Column("KOHI2_OTHER_FUTAN")]
        public int Kohi2OtherFutan { get; set; }

        /// <summary>
        /// 公３負担限度額
        /// 
        /// </summary>
        [Column("KOHI3_LIMIT")]
        public int Kohi3Limit { get; set; }

        /// <summary>
        /// 公３他院負担額
        /// 
        /// </summary>
        [Column("KOHI3_OTHER_FUTAN")]
        public int Kohi3OtherFutan { get; set; }

        /// <summary>
        /// 公４負担限度額
        /// 
        /// </summary>
        [Column("KOHI4_LIMIT")]
        public int Kohi4Limit { get; set; }

        /// <summary>
        /// 公４他院負担額
        /// 
        /// </summary>
        [Column("KOHI4_OTHER_FUTAN")]
        public int Kohi4OtherFutan { get; set; }

        /// <summary>
        /// 診療点数
        /// 
        /// </summary>
        [Column("TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int Tensu { get; set; }

        /// <summary>
        /// 総医療費
        /// 
        /// </summary>
        [Column("TOTAL_IRYOHI")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalIryohi { get; set; }

        /// <summary>
        /// 保険負担額
        /// 
        /// </summary>
        [Column("HOKEN_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenFutan { get; set; }

        /// <summary>
        /// 高額負担額
        /// 
        /// </summary>
        [Column("KOGAKU_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuFutan { get; set; }

        /// <summary>
        /// 公１負担額
        /// 
        /// </summary>
        [Column("KOHI1_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Futan { get; set; }

        /// <summary>
        /// 公２負担額
        /// 
        /// </summary>
        [Column("KOHI2_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Futan { get; set; }

        /// <summary>
        /// 公３負担額
        /// 
        /// </summary>
        [Column("KOHI3_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Futan { get; set; }

        /// <summary>
        /// 公４負担額
        /// 
        /// </summary>
        [Column("KOHI4_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Futan { get; set; }

        /// <summary>
        /// 一部負担額
        /// 
        /// </summary>
        [Column("ICHIBU_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int IchibuFutan { get; set; }

        /// <summary>
        /// 減免額
        /// 
        /// </summary>
        [Column("GENMEN_GAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenGaku { get; set; }

        /// <summary>
        /// 保険負担額10円単位
        /// 
        /// </summary>
        [Column("HOKEN_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenFutan10en { get; set; }

        /// <summary>
        /// 高額負担額10円単位
        /// 
        /// </summary>
        [Column("KOGAKU_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuFutan10en { get; set; }

        /// <summary>
        /// 公１負担額10円単位
        /// 
        /// </summary>
        [Column("KOHI1_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Futan10en { get; set; }

        /// <summary>
        /// 公２負担額10円単位
        /// 
        /// </summary>
        [Column("KOHI2_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Futan10en { get; set; }

        /// <summary>
        /// 公３負担額10円単位
        /// 
        /// </summary>
        [Column("KOHI3_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Futan10en { get; set; }

        /// <summary>
        /// 公４負担額10円単位
        /// 
        /// </summary>
        [Column("KOHI4_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Futan10en { get; set; }

        /// <summary>
        /// 一部負担額10円単位
        /// 
        /// </summary>
        [Column("ICHIBU_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int IchibuFutan10en { get; set; }

        /// <summary>
        /// 減免額10円単位
        /// 
        /// </summary>
        [Column("GENMEN_GAKU_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenGaku10en { get; set; }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        [Column("PT_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int PtFutan { get; set; }

        /// <summary>
        /// 高額療養費超過区分
        /// 
        /// </summary>
        [Column("KOGAKU_OVER_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuOverKbn { get; set; }

        /// <summary>
        /// 保険分点数
        /// 
        /// </summary>
        [Column("HOKEN_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenTensu { get; set; }

        /// <summary>
        /// 保険分一部負担額
        /// 
        /// </summary>
        [Column("HOKEN_ICHIBU_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenIchibuFutan { get; set; }

        /// <summary>
        /// 保険分一部負担額10円単位
        /// 
        /// </summary>
        [Column("HOKEN_ICHIBU_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenIchibuFutan10en { get; set; }

        /// <summary>
        /// 公１分点数
        /// 
        /// </summary>
        [Column("KOHI1_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1Tensu { get; set; }

        /// <summary>
        /// 公１分一部負担相当額
        /// 
        /// </summary>
        [Column("KOHI1_ICHIBU_SOTOGAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1IchibuSotogaku { get; set; }

        /// <summary>
        /// 公１分一部負担相当額10円単位
        /// 
        /// </summary>
        [Column("KOHI1_ICHIBU_SOTOGAKU_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1IchibuSotogaku10en { get; set; }

        /// <summary>
        /// 公１分一部負担額
        /// 
        /// </summary>
        [Column("KOHI1_ICHIBU_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1IchibuFutan { get; set; }

        /// <summary>
        /// 公２分点数
        /// 
        /// </summary>
        [Column("KOHI2_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2Tensu { get; set; }

        /// <summary>
        /// 公２分一部負担相当額
        /// 
        /// </summary>
        [Column("KOHI2_ICHIBU_SOTOGAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2IchibuSotogaku { get; set; }

        /// <summary>
        /// 公２分一部負担相当額10円単位
        /// 
        /// </summary>
        [Column("KOHI2_ICHIBU_SOTOGAKU_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2IchibuSotogaku10en { get; set; }

        /// <summary>
        /// 公２分一部負担額
        /// 
        /// </summary>
        [Column("KOHI2_ICHIBU_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2IchibuFutan { get; set; }

        /// <summary>
        /// 公３分点数
        /// 
        /// </summary>
        [Column("KOHI3_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3Tensu { get; set; }

        /// <summary>
        /// 公３分一部負担相当額
        /// 
        /// </summary>
        [Column("KOHI3_ICHIBU_SOTOGAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3IchibuSotogaku { get; set; }

        /// <summary>
        /// 公３分一部負担相当額10円単位
        /// 
        /// </summary>
        [Column("KOHI3_ICHIBU_SOTOGAKU_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3IchibuSotogaku10en { get; set; }

        /// <summary>
        /// 公３分一部負担額
        /// 
        /// </summary>
        [Column("KOHI3_ICHIBU_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3IchibuFutan { get; set; }

        /// <summary>
        /// 公４分点数
        /// 
        /// </summary>
        [Column("KOHI4_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4Tensu { get; set; }

        /// <summary>
        /// 公４分一部負担相当額
        /// 
        /// </summary>
        [Column("KOHI4_ICHIBU_SOTOGAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4IchibuSotogaku { get; set; }

        /// <summary>
        /// 公４分一部負担相当額10円単位
        ///
        /// </summary>
        [Column("KOHI4_ICHIBU_SOTOGAKU_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4IchibuSotogaku10en { get; set; }

        /// <summary>
        /// 公４分一部負担額
        /// 
        /// </summary>
        [Column("KOHI4_ICHIBU_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4IchibuFutan { get; set; }

        /// <summary>
        /// 合算対象一部負担額
        ///
        /// </summary>
        [Column("TOTAL_ICHIBU_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalIchibuFutan { get; set; }

        /// <summary>
        /// 合算対象一部負担額10円単位
        ///
        /// </summary>
        [Column("TOTAL_ICHIBU_FUTAN_10EN")]
        [CustomAttribute.DefaultValue(0)]
        public int TotalIchibuFutan10en { get; set; }

        /// <summary>
        /// 保険レセ点数
        /// 
        /// </summary>
        [Column("HOKEN_RECE_TENSU")]
        public int? HokenReceTensu { get; set; }

        /// <summary>
        /// 保険レセ負担額
        /// 
        /// </summary>
        [Column("HOKEN_RECE_FUTAN")]
        public int? HokenReceFutan { get; set; }

        /// <summary>
        /// 公１レセ点数
        /// 
        /// </summary>
        [Column("KOHI1_RECE_TENSU")]
        public int? Kohi1ReceTensu { get; set; }

        /// <summary>
        /// 公１レセ負担額
        /// 
        /// </summary>
        [Column("KOHI1_RECE_FUTAN")]
        public int? Kohi1ReceFutan { get; set; }

        /// <summary>
        /// 公１レセ給付対象額
        /// 
        /// </summary>
        [Column("KOHI1_RECE_KYUFU")]
        public int? Kohi1ReceKyufu { get; set; }

        /// <summary>
        /// 公２レセ点数
        /// 
        /// </summary>
        [Column("KOHI2_RECE_TENSU")]
        public int? Kohi2ReceTensu { get; set; }

        /// <summary>
        /// 公２レセ負担額
        /// 
        /// </summary>
        [Column("KOHI2_RECE_FUTAN")]
        public int? Kohi2ReceFutan { get; set; }

        /// <summary>
        /// 公２レセ給付対象額
        /// 
        /// </summary>
        [Column("KOHI2_RECE_KYUFU")]
        public int? Kohi2ReceKyufu { get; set; }

        /// <summary>
        /// 公３レセ点数
        /// 
        /// </summary>
        [Column("KOHI3_RECE_TENSU")]
        public int? Kohi3ReceTensu { get; set; }

        /// <summary>
        /// 公３レセ負担額
        /// 
        /// </summary>
        [Column("KOHI3_RECE_FUTAN")]
        public int? Kohi3ReceFutan { get; set; }

        /// <summary>
        /// 公３レセ給付対象額
        /// 
        /// </summary>
        [Column("KOHI3_RECE_KYUFU")]
        public int? Kohi3ReceKyufu { get; set; }

        /// <summary>
        /// 公４レセ点数
        /// 
        /// </summary>
        [Column("KOHI4_RECE_TENSU")]
        public int? Kohi4ReceTensu { get; set; }

        /// <summary>
        /// 公４レセ負担額
        /// 
        /// </summary>
        [Column("KOHI4_RECE_FUTAN")]
        public int? Kohi4ReceFutan { get; set; }

        /// <summary>
        /// 公４レセ給付対象額
        /// 
        /// </summary>
        [Column("KOHI4_RECE_KYUFU")]
        public int? Kohi4ReceKyufu { get; set; }

        /// <summary>
        /// 保険実日数
        /// 
        /// </summary>
        [Column("HOKEN_NISSU")]
        public int? HokenNissu { get; set; }

        /// <summary>
        /// 公１実日数
        /// 
        /// </summary>
        [Column("KOHI1_NISSU")]
        public int? Kohi1Nissu { get; set; }

        /// <summary>
        /// 公２実日数
        /// 
        /// </summary>
        [Column("KOHI2_NISSU")]
        public int? Kohi2Nissu { get; set; }

        /// <summary>
        /// 公３実日数
        /// 
        /// </summary>
        [Column("KOHI3_NISSU")]
        public int? Kohi3Nissu { get; set; }

        /// <summary>
        /// 公４実日数
        /// 
        /// </summary>
        [Column("KOHI4_NISSU")]
        public int? Kohi4Nissu { get; set; }

        /// <summary>
        /// 公１レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        [Column("KOHI1_RECE_KISAI")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi1ReceKisai { get; set; }

        /// <summary>
        /// 公２レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        [Column("KOHI2_RECE_KISAI")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi2ReceKisai { get; set; }

        /// <summary>
        /// 公３レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        [Column("KOHI3_RECE_KISAI")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi3ReceKisai { get; set; }

        /// <summary>
        /// 公４レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        [Column("KOHI4_RECE_KISAI")]
        [CustomAttribute.DefaultValue(0)]
        public int Kohi4ReceKisai { get; set; }

        /// <summary>
        /// 公１制度略号
        /// 
        /// </summary>
        [Column("KOHI1_NAME_CD")]
        [MaxLength(5)]
        public string Kohi1NameCd { get; set; } = string.Empty;

        /// <summary>
        /// 公２制度略号
        /// 
        /// </summary>
        [Column("KOHI2_NAME_CD")]
        [MaxLength(5)]
        public string Kohi2NameCd { get; set; } = string.Empty;

        /// <summary>
        /// 公３制度略号
        /// 
        /// </summary>
        [Column("KOHI3_NAME_CD")]
        [MaxLength(5)]
        public string Kohi3NameCd { get; set; } = string.Empty;

        /// <summary>
        /// 公４制度略号
        /// 
        /// </summary>
        [Column("KOHI4_NAME_CD")]
        [MaxLength(5)]
        public string Kohi4NameCd { get; set; } = string.Empty;

        /// <summary>
        /// 請求区分
        /// 1:月遅れ 2:返戻 3:オンライン返戻
        /// </summary>
        [Column("SEIKYU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuKbn { get; set; }

        /// <summary>
        /// 特記事項
        /// 
        /// </summary>
        [Column("TOKKI")]
        [MaxLength(10)]
        public string Tokki { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項１
        /// 
        /// </summary>
        [Column("TOKKI1")]
        [MaxLength(10)]
        public string Tokki1 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項２
        /// 
        /// </summary>
        [Column("TOKKI2")]
        [MaxLength(10)]
        public string Tokki2 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項３
        /// 
        /// </summary>
        [Column("TOKKI3")]
        [MaxLength(10)]
        public string Tokki3 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項４
        /// 
        /// </summary>
        [Column("TOKKI4")]
        [MaxLength(10)]
        public string Tokki4 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項５
        /// 
        /// </summary>
        [Column("TOKKI5")]
        [MaxLength(10)]
        public string Tokki5 { get; set; } = string.Empty;

        /// <summary>
        /// 患者の状態
        /// 
        /// </summary>
        [Column("PT_STATUS")]
        [MaxLength(60)]
        public string PtStatus { get; set; } = string.Empty;

        /// <summary>
        /// 労災イ点負担額
        /// 
        /// </summary>
        [Column("ROUSAI_I_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiIFutan { get; set; }

        /// <summary>
        /// 労災ロ円負担額
        /// 
        /// </summary>
        [Column("ROUSAI_RO_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiRoFutan { get; set; }

        /// <summary>
        /// 自賠イ技術点数
        /// 
        /// </summary>
        [Column("JIBAI_I_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiITensu { get; set; }

        /// <summary>
        /// 自賠ロ薬剤点数
        /// 
        /// </summary>
        [Column("JIBAI_RO_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiRoTensu { get; set; }

        /// <summary>
        /// 自賠ハ円診察負担額
        /// 
        /// </summary>
        [Column("JIBAI_HA_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHaFutan { get; set; }

        /// <summary>
        /// 自賠ニ円他負担額
        /// 
        /// </summary>
        [Column("JIBAI_NI_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiNiFutan { get; set; }

        /// <summary>
        /// 自賠ホ診断書料
        /// 
        /// </summary>
        [Column("JIBAI_HO_SINDAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHoSindan { get; set; }

        /// <summary>
        /// 自賠ホ診断書料枚数
        /// 
        /// </summary>
        [Column("JIBAI_HO_SINDAN_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHoSindanCount { get; set; }

        /// <summary>
        /// 自賠ヘ明細書料
        /// 
        /// </summary>
        [Column("JIBAI_HE_MEISAI")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHeMeisai { get; set; }

        /// <summary>
        /// 自賠ヘ明細書料枚数
        /// 
        /// </summary>
        [Column("JIBAI_HE_MEISAI_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiHeMeisaiCount { get; set; }

        /// <summary>
        /// 自賠Ａ負担額
        /// 
        /// </summary>
        [Column("JIBAI_A_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiAFutan { get; set; }

        /// <summary>
        /// 自賠Ｂ負担額
        /// 
        /// </summary>
        [Column("JIBAI_B_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiBFutan { get; set; }

        /// <summary>
        /// 自賠Ｃ負担額
        /// 
        /// </summary>
        [Column("JIBAI_C_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiCFutan { get; set; }

        /// <summary>
        /// 自賠Ｄ負担額
        /// 
        /// </summary>
        [Column("JIBAI_D_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiDFutan { get; set; }

        /// <summary>
        /// 自賠健保点数
        /// 
        /// </summary>
        [Column("JIBAI_KENPO_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiKenpoTensu { get; set; }

        /// <summary>
        /// 自賠健保負担額
        /// 
        /// </summary>
        [Column("JIBAI_KENPO_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiKenpoFutan { get; set; }

        /// <summary>
        /// 新継再別
        /// 
        /// </summary>
        [Column("SINKEI")]
        [CustomAttribute.DefaultValue(0)]
        public int Sinkei { get; set; }

        /// <summary>
        /// 転帰事由
        /// 
        /// </summary>
        [Column("TENKI")]
        [CustomAttribute.DefaultValue(0)]
        public int Tenki { get; set; }

        /// <summary>
        /// 診療科ID
        /// 
        /// </summary>
        [Column("KA_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int KaId { get; set; }

        /// <summary>
        /// 担当医ID
        /// 
        /// </summary>
        [Column("TANTO_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int TantoId { get; set; }

        /// <summary>
        /// テスト患者区分
        ///     1:テスト患者区分
        /// </summary>
        [Column("IS_TESTER")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTester { get; set; }

        /// <summary>
        /// 在医総フラグ
        /// 1:1:在医総管又は在医総
        /// </summary>
        [Column("IS_ZAIISO")]
        [CustomAttribute.DefaultValue(0)]
        public int IsZaiiso { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        public string CreateMachine { get; set; } = string.Empty;

    }
}
