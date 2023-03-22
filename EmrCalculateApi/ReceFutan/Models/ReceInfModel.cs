using Entity.Tenant;
using Helper.Common;
using Helper.Constants;

namespace EmrCalculateApi.ReceFutan.Models
{
    public class ReceInfModel
    {
        public ReceInf ReceInf { get; }

        public ReceInfModel(ReceInf receInf)
        {
            ReceInf = receInf;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return ReceInf.HpId; }
            set
            {
                if (ReceInf.HpId == value) return;
                ReceInf.HpId = value;
            }
        }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        public int SeikyuYm
        {
            get { return ReceInf.SeikyuYm; }
            set
            {
                if (ReceInf.SeikyuYm == value) return;
                ReceInf.SeikyuYm = value;
            }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return ReceInf.PtId; }
            set
            {
                if (ReceInf.PtId == value) return;
                ReceInf.PtId = value;
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return ReceInf.SinYm; }
            set
            {
                if (ReceInf.SinYm == value) return;
                ReceInf.SinYm = value;
            }
        }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return ReceInf.HokenId; }
            set
            {
                if (ReceInf.HokenId == value) return;
                ReceInf.HokenId = value;
            }
        }

        /// <summary>
        /// 主保険保険ID2
        /// 
        /// </summary>
        public int HokenId2
        {
            get { return ReceInf.HokenId2; }
            set
            {
                if (ReceInf.HokenId2 == value) return;
                ReceInf.HokenId2 = value;
            }
        }

        /// <summary>
        /// 公費１保険ID
        /// 
        /// </summary>
        public int Kohi1Id
        {
            get { return ReceInf.Kohi1Id; }
            set
            {
                if (ReceInf.Kohi1Id == value) return;
                ReceInf.Kohi1Id = value;
            }
        }

        /// <summary>
        /// 公費２保険ID
        /// 
        /// </summary>
        public int Kohi2Id
        {
            get { return ReceInf.Kohi2Id; }
            set
            {
                if (ReceInf.Kohi2Id == value) return;
                ReceInf.Kohi2Id = value;
            }
        }

        /// <summary>
        /// 公費３保険ID
        /// 
        /// </summary>
        public int Kohi3Id
        {
            get { return ReceInf.Kohi3Id; }
            set
            {
                if (ReceInf.Kohi3Id == value) return;
                ReceInf.Kohi3Id = value;
            }
        }

        /// <summary>
        /// 公費４保険ID
        /// 
        /// </summary>
        public int Kohi4Id
        {
            get { return ReceInf.Kohi4Id; }
            set
            {
                if (ReceInf.Kohi4Id == value) return;
                ReceInf.Kohi4Id = value;
            }
        }

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
        public int HokenKbn
        {
            get { return ReceInf.HokenKbn; }
            set
            {
                if (ReceInf.HokenKbn == value) return;
                ReceInf.HokenKbn = value;
            }
        }

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
        public int HokenSbtCd
        {
            get { return ReceInf.HokenSbtCd; }
            set
            {
                if (ReceInf.HokenSbtCd == value) return;
                ReceInf.HokenSbtCd = value;
            }
        }

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
        public string ReceSbt
        {
            get { return ReceInf.ReceSbt; }
            set
            {
                if (ReceInf.ReceSbt == value) return;
                ReceInf.ReceSbt = value;
            }
        }

        /// <summary>
        /// 保険者番号
        /// 
        /// </summary>
        public string HokensyaNo
        {
            get { return ReceInf.HokensyaNo; }
            set
            {
                if (ReceInf.HokensyaNo == value) return;
                ReceInf.HokensyaNo = value;
            }
        }

        /// <summary>
        /// 法別番号
        /// 
        /// </summary>
        public string Houbetu
        {
            get { return ReceInf.Houbetu; }
            set
            {
                if (ReceInf.Houbetu == value) return;
                ReceInf.Houbetu = value;
            }
        }

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        public string Kohi1Houbetu
        {
            get { return ReceInf.Kohi1Houbetu ?? string.Empty; }
            set
            {
                if (ReceInf.Kohi1Houbetu == value) return;
                ReceInf.Kohi1Houbetu = value;
            }
        }

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        public string Kohi2Houbetu
        {
            get { return ReceInf.Kohi2Houbetu ?? string.Empty; }
            set
            {
                if (ReceInf.Kohi2Houbetu == value) return;
                ReceInf.Kohi2Houbetu = value;
            }
        }

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        public string Kohi3Houbetu
        {
            get { return ReceInf.Kohi3Houbetu ?? string.Empty; }
            set
            {
                if (ReceInf.Kohi3Houbetu == value) return;
                ReceInf.Kohi3Houbetu = value;
            }
        }

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        public string Kohi4Houbetu
        {
            get { return ReceInf.Kohi4Houbetu ?? string.Empty; }
            set
            {
                if (ReceInf.Kohi4Houbetu == value) return;
                ReceInf.Kohi4Houbetu = value;
            }
        }

        /// <summary>
        /// 本人家族区分
        /// 1:本人 2:家族
        /// </summary>
        public int HonkeKbn
        {
            get { return ReceInf.HonkeKbn; }
            set
            {
                if (ReceInf.HonkeKbn == value) return;
                ReceInf.HonkeKbn = value;
            }
        }

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
        public int KogakuKbn
        {
            get { return ReceInf.KogakuKbn; }
            set
            {
                if (ReceInf.KogakuKbn == value) return;
                ReceInf.KogakuKbn = value;
            }
        }

        /// <summary>
        /// 高額療養費適用区分
        /// 
        /// </summary>
        public int KogakuTekiyoKbn
        {
            get { return ReceInf.KogakuTekiyoKbn; }
            set
            {
                if (ReceInf.KogakuTekiyoKbn == value) return;
                ReceInf.KogakuTekiyoKbn = value;
            }
        }

        /// <summary>
        /// 限度額特例フラグ
        /// 
        /// </summary>
        public int IsTokurei
        {
            get { return ReceInf.IsTokurei; }
            set
            {
                if (ReceInf.IsTokurei == value) return;
                ReceInf.IsTokurei = value;
            }
        }

        /// <summary>
        /// 多数回該当フラグ
        /// 
        /// </summary>
        public int IsTasukai
        {
            get { return ReceInf.IsTasukai; }
            set
            {
                if (ReceInf.IsTasukai == value) return;
                ReceInf.IsTasukai = value;
            }
        }

        /// <summary>
        /// 高額療養費合算対象外
        ///     1:対象外
        /// </summary>
        //public int IsNotKogakuTotal;

        /// <summary>
        /// マル長適用フラグ
        /// 1:適用
        /// </summary>
        public int IsChoki;

        /// <summary>
        /// 高額療養費公１限度額
        /// 
        /// </summary>
        public int KogakuKohi1Limit
        {
            get { return ReceInf.KogakuKohi1Limit; }
            set
            {
                if (ReceInf.KogakuKohi1Limit == value) return;
                ReceInf.KogakuKohi1Limit = value;
            }
        }

        /// <summary>
        /// 高額療養費公２限度額
        /// 
        /// </summary>
        public int KogakuKohi2Limit
        {
            get { return ReceInf.KogakuKohi2Limit; }
            set
            {
                if (ReceInf.KogakuKohi2Limit == value) return;
                ReceInf.KogakuKohi2Limit = value;
            }
        }

        /// <summary>
        /// 高額療養費公３限度額
        /// 
        /// </summary>
        public int KogakuKohi3Limit
        {
            get { return ReceInf.KogakuKohi3Limit; }
            set
            {
                if (ReceInf.KogakuKohi3Limit == value) return;
                ReceInf.KogakuKohi3Limit = value;
            }
        }

        /// <summary>
        /// 高額療養費公４限度額
        /// 
        /// </summary>
        public int KogakuKohi4Limit
        {
            get { return ReceInf.KogakuKohi4Limit; }
            set
            {
                if (ReceInf.KogakuKohi4Limit == value) return;
                ReceInf.KogakuKohi4Limit = value;
            }
        }

        /// <summary>
        /// 高額療養費限度額(合算)
        /// 
        /// </summary>
        public int TotalKogakuLimit
        {
            get { return ReceInf.TotalKogakuLimit; }
            set
            {
                if (ReceInf.TotalKogakuLimit == value) return;
                ReceInf.TotalKogakuLimit = value;
            }
        }

        /// <summary>
        /// 国保減免区分
        ///     1:減額 2:免除 3:支払猶予 4:自立支援減免
        /// </summary>
        public int GenmenKbn
        {
            get { return ReceInf.GenmenKbn; }
            set
            {
                if (ReceInf.GenmenKbn == value) return;
                ReceInf.GenmenKbn = value;
            }
        }

        /// <summary>
        /// 保険負担率
        /// 
        /// </summary>
        public int HokenRate
        {
            get { return ReceInf.HokenRate; }
            set
            {
                if (ReceInf.HokenRate == value) return;
                ReceInf.HokenRate = value;
            }
        }

        /// <summary>
        /// 患者負担率
        /// 
        /// </summary>
        public int PtRate
        {
            get { return ReceInf.PtRate; }
            set
            {
                if (ReceInf.PtRate == value) return;
                ReceInf.PtRate = value;
            }
        }

        /// <summary>
        /// 点数単価
        /// 
        /// </summary>
        public int EnTen
        {
            get { return ReceInf.EnTen; }
            set
            {
                if (ReceInf.EnTen == value) return;
                ReceInf.EnTen = value;
            }
        }

        /// <summary>
        /// 公１負担限度額
        /// 
        /// </summary>
        public int Kohi1Limit
        {
            get { return ReceInf.Kohi1Limit; }
            set
            {
                if (ReceInf.Kohi1Limit == value) return;
                ReceInf.Kohi1Limit = value;
            }
        }

        /// <summary>
        /// 公１他院負担額
        /// 
        /// </summary>
        public int Kohi1OtherFutan
        {
            get { return ReceInf.Kohi1OtherFutan; }
            set
            {
                if (ReceInf.Kohi1OtherFutan == value) return;
                ReceInf.Kohi1OtherFutan = value;
            }
        }

        /// <summary>
        /// 公２負担限度額
        /// 
        /// </summary>
        public int Kohi2Limit
        {
            get { return ReceInf.Kohi2Limit; }
            set
            {
                if (ReceInf.Kohi2Limit == value) return;
                ReceInf.Kohi2Limit = value;
            }
        }

        /// <summary>
        /// 公２他院負担額
        /// 
        /// </summary>
        public int Kohi2OtherFutan
        {
            get { return ReceInf.Kohi2OtherFutan; }
            set
            {
                if (ReceInf.Kohi2OtherFutan == value) return;
                ReceInf.Kohi2OtherFutan = value;
            }
        }

        /// <summary>
        /// 公３負担限度額
        /// 
        /// </summary>
        public int Kohi3Limit
        {
            get { return ReceInf.Kohi3Limit; }
            set
            {
                if (ReceInf.Kohi3Limit == value) return;
                ReceInf.Kohi3Limit = value;
            }
        }

        /// <summary>
        /// 公３他院負担額
        /// 
        /// </summary>
        public int Kohi3OtherFutan
        {
            get { return ReceInf.Kohi3OtherFutan; }
            set
            {
                if (ReceInf.Kohi3OtherFutan == value) return;
                ReceInf.Kohi3OtherFutan = value;
            }
        }

        /// <summary>
        /// 公４負担限度額
        /// 
        /// </summary>
        public int Kohi4Limit
        {
            get { return ReceInf.Kohi4Limit; }
            set
            {
                if (ReceInf.Kohi4Limit == value) return;
                ReceInf.Kohi4Limit = value;
            }
        }

        /// <summary>
        /// 公４他院負担額
        /// 
        /// </summary>
        public int Kohi4OtherFutan
        {
            get { return ReceInf.Kohi4OtherFutan; }
            set
            {
                if (ReceInf.Kohi4OtherFutan == value) return;
                ReceInf.Kohi4OtherFutan = value;
            }
        }

        /// <summary>
        /// 診療点数
        /// 
        /// </summary>
        public int Tensu
        {
            get { return ReceInf.Tensu; }
            set
            {
                if (ReceInf.Tensu == value) return;
                ReceInf.Tensu = value;
            }
        }

        /// <summary>
        /// 総医療費
        /// 
        /// </summary>
        public int TotalIryohi
        {
            get { return ReceInf.TotalIryohi; }
            set
            {
                if (ReceInf.TotalIryohi == value) return;
                ReceInf.TotalIryohi = value;
            }
        }

        /// <summary>
        /// 保険負担額
        /// 
        /// </summary>
        public int HokenFutan
        {
            get { return ReceInf.HokenFutan; }
            set
            {
                if (ReceInf.HokenFutan == value) return;
                ReceInf.HokenFutan = value;
            }
        }

        /// <summary>
        /// 高額負担額
        /// 
        /// </summary>
        public int KogakuFutan
        {
            get { return ReceInf.KogakuFutan; }
            set
            {
                if (ReceInf.KogakuFutan == value) return;
                ReceInf.KogakuFutan = value;
            }
        }

        /// <summary>
        /// 公１負担額
        /// 
        /// </summary>
        public int Kohi1Futan
        {
            get { return ReceInf.Kohi1Futan; }
            set
            {
                if (ReceInf.Kohi1Futan == value) return;
                ReceInf.Kohi1Futan = value;
            }
        }

        /// <summary>
        /// 公２負担額
        /// 
        /// </summary>
        public int Kohi2Futan
        {
            get { return ReceInf.Kohi2Futan; }
            set
            {
                if (ReceInf.Kohi2Futan == value) return;
                ReceInf.Kohi2Futan = value;
            }
        }

        /// <summary>
        /// 公３負担額
        /// 
        /// </summary>
        public int Kohi3Futan
        {
            get { return ReceInf.Kohi3Futan; }
            set
            {
                if (ReceInf.Kohi3Futan == value) return;
                ReceInf.Kohi3Futan = value;
            }
        }

        /// <summary>
        /// 公４負担額
        /// 
        /// </summary>
        public int Kohi4Futan
        {
            get { return ReceInf.Kohi4Futan; }
            set
            {
                if (ReceInf.Kohi4Futan == value) return;
                ReceInf.Kohi4Futan = value;
            }
        }

        /// <summary>
        /// 一部負担額
        /// 
        /// </summary>
        public int IchibuFutan
        {
            get { return ReceInf.IchibuFutan; }
            set
            {
                if (ReceInf.IchibuFutan == value) return;
                ReceInf.IchibuFutan = value;
            }
        }

        /// <summary>
        /// 減免額
        /// 
        /// </summary>
        public int GenmenGaku
        {
            get { return ReceInf.GenmenGaku; }
            set
            {
                if (ReceInf.GenmenGaku == value) return;
                ReceInf.GenmenGaku = value;
            }
        }

        /// <summary>
        /// 保険負担額10円単位
        /// 
        /// </summary>
        public int HokenFutan10en
        {
            get { return ReceInf.HokenFutan10en; }
            set
            {
                if (ReceInf.HokenFutan10en == value) return;
                ReceInf.HokenFutan10en = value;
            }
        }

        /// <summary>
        /// 高額負担額10円単位
        /// 
        /// </summary>
        public int KogakuFutan10en
        {
            get { return ReceInf.KogakuFutan10en; }
            set
            {
                if (ReceInf.KogakuFutan10en == value) return;
                ReceInf.KogakuFutan10en = value;
            }
        }

        /// <summary>
        /// 公１負担額10円単位
        /// 
        /// </summary>
        public int Kohi1Futan10en
        {
            get { return ReceInf.Kohi1Futan10en; }
            set
            {
                if (ReceInf.Kohi1Futan10en == value) return;
                ReceInf.Kohi1Futan10en = value;
            }
        }

        /// <summary>
        /// 公２負担額10円単位
        /// 
        /// </summary>
        public int Kohi2Futan10en
        {
            get { return ReceInf.Kohi2Futan10en; }
            set
            {
                if (ReceInf.Kohi2Futan10en == value) return;
                ReceInf.Kohi2Futan10en = value;
            }
        }

        /// <summary>
        /// 公３負担額10円単位
        /// 
        /// </summary>
        public int Kohi3Futan10en
        {
            get { return ReceInf.Kohi3Futan10en; }
            set
            {
                if (ReceInf.Kohi3Futan10en == value) return;
                ReceInf.Kohi3Futan10en = value;
            }
        }

        /// <summary>
        /// 公４負担額10円単位
        /// 
        /// </summary>
        public int Kohi4Futan10en
        {
            get { return ReceInf.Kohi4Futan10en; }
            set
            {
                if (ReceInf.Kohi4Futan10en == value) return;
                ReceInf.Kohi4Futan10en = value;
            }
        }

        /// <summary>
        /// 一部負担額10円単位
        /// 
        /// </summary>
        public int IchibuFutan10en
        {
            get { return ReceInf.IchibuFutan10en; }
            set
            {
                if (ReceInf.IchibuFutan10en == value) return;
                ReceInf.IchibuFutan10en = value;
            }
        }

        /// <summary>
        /// 減免額10円単位
        /// 
        /// </summary>
        public int GenmenGaku10en
        {
            get { return ReceInf.GenmenGaku10en; }
            set
            {
                if (ReceInf.GenmenGaku10en == value) return;
                ReceInf.GenmenGaku10en = value;
            }
        }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        public int PtFutan
        {
            get { return ReceInf.PtFutan; }
            set
            {
                if (ReceInf.PtFutan == value) return;
                ReceInf.PtFutan = value;
            }
        }

        /// <summary>
        /// 高額療養費超過区分
        /// 
        /// </summary>
        public int KogakuOverKbn
        {
            get { return ReceInf.KogakuOverKbn; }
            set
            {
                if (ReceInf.KogakuOverKbn == value) return;
                ReceInf.KogakuOverKbn = value;
            }
        }

        /// <summary>
        /// 保険分点数
        /// 
        /// </summary>
        public int HokenTensu
        {
            get { return ReceInf.HokenTensu; }
            set
            {
                if (ReceInf.HokenTensu == value) return;
                ReceInf.HokenTensu = value;
            }
        }

        /// <summary>
        /// 保険分一部負担額
        /// 
        /// </summary>
        public int HokenIchibuFutan
        {
            get { return ReceInf.HokenIchibuFutan; }
            set
            {
                if (ReceInf.HokenIchibuFutan == value) return;
                ReceInf.HokenIchibuFutan = value;
            }
        }

        /// <summary>
        /// 保険分一部負担額10円単位
        /// 
        /// </summary>
        public int HokenIchibuFutan10en
        {
            get { return ReceInf.HokenIchibuFutan10en; }
            set
            {
                if (ReceInf.HokenIchibuFutan10en == value) return;
                ReceInf.HokenIchibuFutan10en = value;
            }
        }

        /// <summary>
        /// 公１分点数
        /// 
        /// </summary>
        public int Kohi1Tensu
        {
            get { return ReceInf.Kohi1Tensu; }
            set
            {
                if (ReceInf.Kohi1Tensu == value) return;
                ReceInf.Kohi1Tensu = value;
            }
        }

        /// <summary>
        /// 公１分一部負担相当額
        /// 
        /// </summary>
        public int Kohi1IchibuSotogaku
        {
            get { return ReceInf.Kohi1IchibuSotogaku; }
            set
            {
                if (ReceInf.Kohi1IchibuSotogaku == value) return;
                ReceInf.Kohi1IchibuSotogaku = value;
            }
        }

        /// <summary>
        /// 公１分一部負担相当額10円単位
        /// 
        /// </summary>
        public int Kohi1IchibuSotogaku10en
        {
            get { return ReceInf.Kohi1IchibuSotogaku10en; }
            set
            {
                if (ReceInf.Kohi1IchibuSotogaku10en == value) return;
                ReceInf.Kohi1IchibuSotogaku10en = value;
            }
        }

        /// <summary>
        /// 公１分一部負担額
        /// 
        /// </summary>
        public int Kohi1IchibuFutan
        {
            get { return ReceInf.Kohi1IchibuFutan; }
            set
            {
                if (ReceInf.Kohi1IchibuFutan == value) return;
                ReceInf.Kohi1IchibuFutan = value;
            }
        }

        /// <summary>
        /// 公２分点数
        /// 
        /// </summary>
        public int Kohi2Tensu
        {
            get { return ReceInf.Kohi2Tensu; }
            set
            {
                if (ReceInf.Kohi2Tensu == value) return;
                ReceInf.Kohi2Tensu = value;
            }
        }

        /// <summary>
        /// 公２分一部負担相当額
        /// 
        /// </summary>
        public int Kohi2IchibuSotogaku
        {
            get { return ReceInf.Kohi2IchibuSotogaku; }
            set
            {
                if (ReceInf.Kohi2IchibuSotogaku == value) return;
                ReceInf.Kohi2IchibuSotogaku = value;
            }
        }

        /// <summary>
        /// 公２分一部負担相当額10円単位
        /// 
        /// </summary>
        public int Kohi2IchibuSotogaku10en
        {
            get { return ReceInf.Kohi2IchibuSotogaku10en; }
            set
            {
                if (ReceInf.Kohi2IchibuSotogaku10en == value) return;
                ReceInf.Kohi2IchibuSotogaku10en = value;
            }
        }

        /// <summary>
        /// 公２分一部負担額
        /// 
        /// </summary>
        public int Kohi2IchibuFutan
        {
            get { return ReceInf.Kohi2IchibuFutan; }
            set
            {
                if (ReceInf.Kohi2IchibuFutan == value) return;
                ReceInf.Kohi2IchibuFutan = value;
            }
        }

        /// <summary>
        /// 公３分点数
        /// 
        /// </summary>
        public int Kohi3Tensu
        {
            get { return ReceInf.Kohi3Tensu; }
            set
            {
                if (ReceInf.Kohi3Tensu == value) return;
                ReceInf.Kohi3Tensu = value;
            }
        }

        /// <summary>
        /// 公３分一部負担相当額
        /// 
        /// </summary>
        public int Kohi3IchibuSotogaku
        {
            get { return ReceInf.Kohi3IchibuSotogaku; }
            set
            {
                if (ReceInf.Kohi3IchibuSotogaku == value) return;
                ReceInf.Kohi3IchibuSotogaku = value;
            }
        }

        /// <summary>
        /// 公３分一部負担相当額10円単位
        /// 
        /// </summary>
        public int Kohi3IchibuSotogaku10en
        {
            get { return ReceInf.Kohi3IchibuSotogaku10en; }
            set
            {
                if (ReceInf.Kohi3IchibuSotogaku10en == value) return;
                ReceInf.Kohi3IchibuSotogaku10en = value;
            }
        }

        /// <summary>
        /// 公３分一部負担額
        /// 
        /// </summary>
        public int Kohi3IchibuFutan
        {
            get { return ReceInf.Kohi3IchibuFutan; }
            set
            {
                if (ReceInf.Kohi3IchibuFutan == value) return;
                ReceInf.Kohi3IchibuFutan = value;
            }
        }

        /// <summary>
        /// 公４分点数
        /// 
        /// </summary>
        public int Kohi4Tensu
        {
            get { return ReceInf.Kohi4Tensu; }
            set
            {
                if (ReceInf.Kohi4Tensu == value) return;
                ReceInf.Kohi4Tensu = value;
            }
        }

        /// <summary>
        /// 公４分一部負担相当額
        /// 
        /// </summary>
        public int Kohi4IchibuSotogaku
        {
            get { return ReceInf.Kohi4IchibuSotogaku; }
            set
            {
                if (ReceInf.Kohi4IchibuSotogaku == value) return;
                ReceInf.Kohi4IchibuSotogaku = value;
            }
        }

        /// <summary>
        /// 公４分一部負担相当額10円単位
        ///
        /// </summary>
        public int Kohi4IchibuSotogaku10en
        {
            get { return ReceInf.Kohi4IchibuSotogaku10en; }
            set
            {
                if (ReceInf.Kohi4IchibuSotogaku10en == value) return;
                ReceInf.Kohi4IchibuSotogaku10en = value;
            }
        }

        /// <summary>
        /// 公４分一部負担額
        /// 
        /// </summary>
        public int Kohi4IchibuFutan
        {
            get { return ReceInf.Kohi4IchibuFutan; }
            set
            {
                if (ReceInf.Kohi4IchibuFutan == value) return;
                ReceInf.Kohi4IchibuFutan = value;
            }
        }

        /// <summary>
        /// 合算対象一部負担額
        ///
        /// </summary>
        public int TotalIchibuFutan
        {
            get { return ReceInf.TotalIchibuFutan; }
            set
            {
                if (ReceInf.TotalIchibuFutan == value) return;
                ReceInf.TotalIchibuFutan = value;
            }
        }

        /// <summary>
        /// 合算対象一部負担額10円単位
        ///
        /// </summary>
        public int TotalIchibuFutan10en
        {
            get { return ReceInf.TotalIchibuFutan10en; }
            set
            {
                if (ReceInf.TotalIchibuFutan10en == value) return;
                ReceInf.TotalIchibuFutan10en = value;
            }
        }

        /// <summary>
        /// 保険レセ点数
        /// 
        /// </summary>
        public int? HokenReceTensu
        {
            get { return ReceInf.HokenReceTensu; }
            set
            {
                if (ReceInf.HokenReceTensu == value) return;
                ReceInf.HokenReceTensu = value;
            }
        }

        /// <summary>
        /// 保険レセ負担額
        /// 
        /// </summary>
        public int? HokenReceFutan
        {
            get { return ReceInf.HokenReceFutan; }
            set
            {
                if (ReceInf.HokenReceFutan == value) return;
                ReceInf.HokenReceFutan = value;
            }
        }

        /// <summary>
        /// 公１レセ点数
        /// 
        /// </summary>
        public int? Kohi1ReceTensu
        {
            get { return ReceInf.Kohi1ReceTensu; }
            set
            {
                if (ReceInf.Kohi1ReceTensu == value) return;
                ReceInf.Kohi1ReceTensu = value;
            }
        }

        /// <summary>
        /// 公１レセ負担額
        /// 
        /// </summary>
        public int? Kohi1ReceFutan
        {
            get { return ReceInf.Kohi1ReceFutan; }
            set
            {
                if (ReceInf.Kohi1ReceFutan == value) return;
                ReceInf.Kohi1ReceFutan = value;
            }
        }

        /// <summary>
        /// 公１レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi1ReceKyufu
        {
            get { return ReceInf.Kohi1ReceKyufu; }
            set
            {
                if (ReceInf.Kohi1ReceKyufu == value) return;
                ReceInf.Kohi1ReceKyufu = value;
            }
        }

        /// <summary>
        /// 公１レセ給付対象額10円単位
        /// 
        /// </summary>
        public int? Kohi1ReceKyufu10en;

        /// <summary>
        /// 公２レセ点数
        /// 
        /// </summary>
        public int? Kohi2ReceTensu
        {
            get { return ReceInf.Kohi2ReceTensu; }
            set
            {
                if (ReceInf.Kohi2ReceTensu == value) return;
                ReceInf.Kohi2ReceTensu = value;
            }
        }

        /// <summary>
        /// 公２レセ負担額
        /// 
        /// </summary>
        public int? Kohi2ReceFutan
        {
            get { return ReceInf.Kohi2ReceFutan; }
            set
            {
                if (ReceInf.Kohi2ReceFutan == value) return;
                ReceInf.Kohi2ReceFutan = value;
            }
        }

        /// <summary>
        /// 公２レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi2ReceKyufu
        {
            get { return ReceInf.Kohi2ReceKyufu; }
            set
            {
                if (ReceInf.Kohi2ReceKyufu == value) return;
                ReceInf.Kohi2ReceKyufu = value;
            }
        }

        /// <summary>
        /// 公２レセ給付対象額10円単位
        /// 
        /// </summary>
        public int? Kohi2ReceKyufu10en;

        /// <summary>
        /// 公３レセ点数
        /// 
        /// </summary>
        public int? Kohi3ReceTensu
        {
            get { return ReceInf.Kohi3ReceTensu; }
            set
            {
                if (ReceInf.Kohi3ReceTensu == value) return;
                ReceInf.Kohi3ReceTensu = value;
            }
        }

        /// <summary>
        /// 公３レセ負担額
        /// 
        /// </summary>
        public int? Kohi3ReceFutan
        {
            get { return ReceInf.Kohi3ReceFutan; }
            set
            {
                if (ReceInf.Kohi3ReceFutan == value) return;
                ReceInf.Kohi3ReceFutan = value;
            }
        }

        /// <summary>
        /// 公３レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi3ReceKyufu
        {
            get { return ReceInf.Kohi3ReceKyufu; }
            set
            {
                if (ReceInf.Kohi3ReceKyufu == value) return;
                ReceInf.Kohi3ReceKyufu = value;
            }
        }

        /// <summary>
        /// 公３レセ給付対象額10円単位
        /// 
        /// </summary>
        public int? Kohi3ReceKyufu10en;

        /// <summary>
        /// 公４レセ点数
        /// 
        /// </summary>
        public int? Kohi4ReceTensu
        {
            get { return ReceInf.Kohi4ReceTensu; }
            set
            {
                if (ReceInf.Kohi4ReceTensu == value) return;
                ReceInf.Kohi4ReceTensu = value;
            }
        }

        /// <summary>
        /// 公４レセ負担額
        /// 
        /// </summary>
        public int? Kohi4ReceFutan
        {
            get { return ReceInf.Kohi4ReceFutan; }
            set
            {
                if (ReceInf.Kohi4ReceFutan == value) return;
                ReceInf.Kohi4ReceFutan = value;
            }
        }

        /// <summary>
        /// 公４レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi4ReceKyufu
        {
            get { return ReceInf.Kohi4ReceKyufu; }
            set
            {
                if (ReceInf.Kohi4ReceKyufu == value) return;
                ReceInf.Kohi4ReceKyufu = value;
            }
        }

        /// <summary>
        /// 公４レセ給付対象額10円単位
        /// 
        /// </summary>
        public int? Kohi4ReceKyufu10en;

        /// <summary>
        /// 保険実日数
        /// 
        /// </summary>
        public int? HokenNissu
        {
            get { return ReceInf.HokenNissu; }
            set
            {
                if (ReceInf.HokenNissu == value) return;
                ReceInf.HokenNissu = value;
            }
        }

        /// <summary>
        /// 公１実日数
        /// 
        /// </summary>
        public int? Kohi1Nissu
        {
            get { return ReceInf.Kohi1Nissu; }
            set
            {
                if (ReceInf.Kohi1Nissu == value) return;
                ReceInf.Kohi1Nissu = value;
            }
        }

        /// <summary>
        /// 公２実日数
        /// 
        /// </summary>
        public int? Kohi2Nissu
        {
            get { return ReceInf.Kohi2Nissu; }
            set
            {
                if (ReceInf.Kohi2Nissu == value) return;
                ReceInf.Kohi2Nissu = value;
            }
        }

        /// <summary>
        /// 公３実日数
        /// 
        /// </summary>
        public int? Kohi3Nissu
        {
            get { return ReceInf.Kohi3Nissu; }
            set
            {
                if (ReceInf.Kohi3Nissu == value) return;
                ReceInf.Kohi3Nissu = value;
            }
        }

        /// <summary>
        /// 公４実日数
        /// 
        /// </summary>
        public int? Kohi4Nissu
        {
            get { return ReceInf.Kohi4Nissu; }
            set
            {
                if (ReceInf.Kohi4Nissu == value) return;
                ReceInf.Kohi4Nissu = value;
            }
        }

        /// <summary>
        /// 公１レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public bool Kohi1ReceKisai
        {
            get { return ReceInf.Kohi1ReceKisai == 1; }
            set
            {
                if (ReceInf.Kohi1ReceKisai == Convert.ToInt32(value)) return;
                ReceInf.Kohi1ReceKisai = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// 公２レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public bool Kohi2ReceKisai
        {
            get { return ReceInf.Kohi2ReceKisai == 1; }
            set
            {
                if (ReceInf.Kohi2ReceKisai == Convert.ToInt32(value)) return;
                ReceInf.Kohi2ReceKisai = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// 公３レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public bool Kohi3ReceKisai
        {
            get { return ReceInf.Kohi3ReceKisai == 1; }
            set
            {
                if (ReceInf.Kohi3ReceKisai == Convert.ToInt32(value)) return;
                ReceInf.Kohi3ReceKisai = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// 公４レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public bool Kohi4ReceKisai
        {
            get { return ReceInf.Kohi4ReceKisai == 1; }
            set
            {
                if (ReceInf.Kohi4ReceKisai == Convert.ToInt32(value)) return;
                ReceInf.Kohi4ReceKisai = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// 公１制度略号
        /// 
        /// </summary>
        public string Kohi1NameCd
        {
            get { return ReceInf.Kohi1NameCd; }
            set
            {
                if (ReceInf.Kohi1NameCd == value) return;
                ReceInf.Kohi1NameCd = value;
            }
        }

        /// <summary>
        /// 公２制度略号
        /// 
        /// </summary>
        public string Kohi2NameCd
        {
            get { return ReceInf.Kohi2NameCd; }
            set
            {
                if (ReceInf.Kohi2NameCd == value) return;
                ReceInf.Kohi2NameCd = value;
            }
        }

        /// <summary>
        /// 公３制度略号
        /// 
        /// </summary>
        public string Kohi3NameCd
        {
            get { return ReceInf.Kohi3NameCd; }
            set
            {
                if (ReceInf.Kohi3NameCd == value) return;
                ReceInf.Kohi3NameCd = value;
            }
        }

        /// <summary>
        /// 公４制度略号
        /// 
        /// </summary>
        public string Kohi4NameCd
        {
            get { return ReceInf.Kohi4NameCd; }
            set
            {
                if (ReceInf.Kohi4NameCd == value) return;
                ReceInf.Kohi4NameCd = value;
            }
        }

        /// <summary>
        /// 請求区分
        /// 1:月遅れ 2:返戻 3:オンライン返戻
        /// </summary>
        public int SeikyuKbn
        {
            get { return ReceInf.SeikyuKbn; }
            set
            {
                if (ReceInf.SeikyuKbn == value) return;
                ReceInf.SeikyuKbn = value;
            }
        }

        /// <summary>
        /// 特記事項
        /// 
        /// </summary>
        public string Tokki
        {
            get { return ReceInf.Tokki; }
            set
            {
                if (ReceInf.Tokki == value) return;
                ReceInf.Tokki = value;
            }
        }

        /// <summary>
        /// 特記事項１
        /// 
        /// </summary>
        public string Tokki1
        {
            get { return ReceInf.Tokki1; }
            set
            {
                if (ReceInf.Tokki1 == value) return;
                ReceInf.Tokki1 = value;
            }
        }

        /// <summary>
        /// 特記事項２
        /// 
        /// </summary>
        public string Tokki2
        {
            get { return ReceInf.Tokki2; }
            set
            {
                if (ReceInf.Tokki2 == value) return;
                ReceInf.Tokki2 = value;
            }
        }

        /// <summary>
        /// 特記事項３
        /// 
        /// </summary>
        public string Tokki3
        {
            get { return ReceInf.Tokki3; }
            set
            {
                if (ReceInf.Tokki3 == value) return;
                ReceInf.Tokki3 = value;
            }
        }

        /// <summary>
        /// 特記事項４
        /// 
        /// </summary>
        public string Tokki4
        {
            get { return ReceInf.Tokki4; }
            set
            {
                if (ReceInf.Tokki4 == value) return;
                ReceInf.Tokki4 = value;
            }
        }

        /// <summary>
        /// 特記事項５
        /// 
        /// </summary>
        public string Tokki5
        {
            get { return ReceInf.Tokki5; }
            set
            {
                if (ReceInf.Tokki5 == value) return;
                ReceInf.Tokki5 = value;
            }
        }

        /// <summary>
        /// 患者の状態
        /// 
        /// </summary>
        public string PtStatus
        {
            get { return ReceInf.PtStatus; }
            set
            {
                if (ReceInf.PtStatus == value) return;
                ReceInf.PtStatus = value;
            }
        }

        /// <summary>
        /// 労災イ点負担額
        /// 
        /// </summary>
        public int RousaiIFutan
        {
            get { return ReceInf.RousaiIFutan; }
            set
            {
                if (ReceInf.RousaiIFutan == value) return;
                ReceInf.RousaiIFutan = value;
            }
        }

        /// <summary>
        /// 労災ロ円負担額
        /// 
        /// </summary>
        public int RousaiRoFutan
        {
            get { return ReceInf.RousaiRoFutan; }
            set
            {
                if (ReceInf.RousaiRoFutan == value) return;
                ReceInf.RousaiRoFutan = value;
            }
        }

        /// <summary>
        /// 自賠イ技術点数
        /// 
        /// </summary>
        public int JibaiITensu
        {
            get { return ReceInf.JibaiITensu; }
            set
            {
                if (ReceInf.JibaiITensu == value) return;
                ReceInf.JibaiITensu = value;
            }
        }

        /// <summary>
        /// 自賠ロ薬剤点数
        /// 
        /// </summary>
        public int JibaiRoTensu
        {
            get { return ReceInf.JibaiRoTensu; }
            set
            {
                if (ReceInf.JibaiRoTensu == value) return;
                ReceInf.JibaiRoTensu = value;
            }
        }

        /// <summary>
        /// 自賠ハ円診察負担額
        /// 
        /// </summary>
        public int JibaiHaFutan
        {
            get { return ReceInf.JibaiHaFutan; }
            set
            {
                if (ReceInf.JibaiHaFutan == value) return;
                ReceInf.JibaiHaFutan = value;
            }
        }

        /// <summary>
        /// 自賠ニ円他負担額
        /// 
        /// </summary>
        public int JibaiNiFutan
        {
            get { return ReceInf.JibaiNiFutan; }
            set
            {
                if (ReceInf.JibaiNiFutan == value) return;
                ReceInf.JibaiNiFutan = value;
            }
        }

        /// <summary>
        /// 自賠ホ診断書料
        /// 
        /// </summary>
        public int JibaiHoSindan
        {
            get { return ReceInf.JibaiHoSindan; }
            set
            {
                if (ReceInf.JibaiHoSindan == value) return;
                ReceInf.JibaiHoSindan = value;
            }
        }

        /// <summary>
        /// 自賠ホ診断書料枚数
        /// 
        /// </summary>
        public int JibaiHoSindanCount
        {
            get { return ReceInf.JibaiHoSindanCount; }
            set
            {
                if (ReceInf.JibaiHoSindanCount == value) return;
                ReceInf.JibaiHoSindanCount = value;
            }
        }

        /// <summary>
        /// 自賠ヘ明細書料
        /// 
        /// </summary>
        public int JibaiHeMeisai
        {
            get { return ReceInf.JibaiHeMeisai; }
            set
            {
                if (ReceInf.JibaiHeMeisai == value) return;
                ReceInf.JibaiHeMeisai = value;
            }
        }

        /// <summary>
        /// 自賠ヘ明細書料枚数
        /// 
        /// </summary>
        public int JibaiHeMeisaiCount
        {
            get { return ReceInf.JibaiHeMeisaiCount; }
            set
            {
                if (ReceInf.JibaiHeMeisaiCount == value) return;
                ReceInf.JibaiHeMeisaiCount = value;
            }
        }

        /// <summary>
        /// 自賠Ａ負担額
        /// 
        /// </summary>
        public int JibaiAFutan
        {
            get { return ReceInf.JibaiAFutan; }
            set
            {
                if (ReceInf.JibaiAFutan == value) return;
                ReceInf.JibaiAFutan = value;
            }
        }

        /// <summary>
        /// 自賠Ｂ負担額
        /// 
        /// </summary>
        public int JibaiBFutan
        {
            get { return ReceInf.JibaiBFutan; }
            set
            {
                if (ReceInf.JibaiBFutan == value) return;
                ReceInf.JibaiBFutan = value;
            }
        }

        /// <summary>
        /// 自賠Ｃ負担額
        /// 
        /// </summary>
        public int JibaiCFutan
        {
            get { return ReceInf.JibaiCFutan; }
            set
            {
                if (ReceInf.JibaiCFutan == value) return;
                ReceInf.JibaiCFutan = value;
            }
        }

        /// <summary>
        /// 自賠Ｄ負担額
        /// 
        /// </summary>
        public int JibaiDFutan
        {
            get { return ReceInf.JibaiDFutan; }
            set
            {
                if (ReceInf.JibaiDFutan == value) return;
                ReceInf.JibaiDFutan = value;
            }
        }

        /// <summary>
        /// 自賠健保点数
        /// 
        /// </summary>
        public int JibaiKenpoTensu
        {
            get { return ReceInf.JibaiKenpoTensu; }
            set
            {
                if (ReceInf.JibaiKenpoTensu == value) return;
                ReceInf.JibaiKenpoTensu = value;
            }
        }

        /// <summary>
        /// 自賠健保負担額
        /// 
        /// </summary>
        public int JibaiKenpoFutan
        {
            get { return ReceInf.JibaiKenpoFutan; }
            set
            {
                if (ReceInf.JibaiKenpoFutan == value) return;
                ReceInf.JibaiKenpoFutan = value;
            }
        }

        /// <summary>
        /// 新継再別
        /// 
        /// </summary>
        public int Sinkei
        {
            get { return ReceInf.Sinkei; }
            set
            {
                if (ReceInf.Sinkei == value) return;
                ReceInf.Sinkei = value;
            }
        }

        /// <summary>
        /// 転帰事由
        /// 
        /// </summary>
        public int Tenki
        {
            get { return ReceInf.Tenki; }
            set
            {
                if (ReceInf.Tenki == value) return;
                ReceInf.Tenki = value;
            }
        }

        /// <summary>
        /// 診療科ID
        /// 
        /// </summary>
        public int KaId
        {
            get { return ReceInf.KaId; }
            set
            {
                if (ReceInf.KaId == value) return;
                ReceInf.KaId = value;
            }
        }

        /// <summary>
        /// 担当医ID
        /// 
        /// </summary>
        public int TantoId
        {
            get { return ReceInf.TantoId; }
            set
            {
                if (ReceInf.TantoId == value) return;
                ReceInf.TantoId = value;
            }
        }

        /// <summary>
        /// テスト患者区分
        ///     1:テスト患者区分
        /// </summary>
        public int IsTester
        {
            get { return ReceInf.IsTester; }
            set
            {
                if (ReceInf.IsTester == value) return;
                ReceInf.IsTester = value;
            }
        }

        /// <summary>
        /// 在医総フラグ
        ///     1:在医総管又は在医総
        /// </summary>
        public int IsZaiiso
        {
            get { return ReceInf.IsZaiiso; }
            set
            {
                if (ReceInf.IsZaiiso == value) return;
                ReceInf.IsZaiiso = value;
            }
        }

        /// <summary>
        /// マル長フラグ
        ///     0:なし
        ///     1:あり(上限未満)
        ///     2:あり(上限超)
        /// </summary>
        public int ChokiKbn
        {
            get { return ReceInf.ChokiKbn; }
            set
            {
                if (ReceInf.ChokiKbn == value) return;
                ReceInf.ChokiKbn = value;
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return ReceInf.CreateDate; }
            set
            {
                if (ReceInf.CreateDate == value) return;
                ReceInf.CreateDate = value;
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return ReceInf.CreateId; }
            set
            {
                if (ReceInf.CreateId == value) return;
                ReceInf.CreateId = value;
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return ReceInf.CreateMachine ?? string.Empty; }
            set
            {
                if (ReceInf.CreateMachine == value) return;
                ReceInf.CreateMachine = value;
            }
        }

        /// <summary>
        /// 公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi1Priority { get; set; } = string.Empty;
        /// <summary>
        /// 公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi2Priority { get; set; } = string.Empty;
        /// <summary>
        /// 公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi3Priority { get; set; } = string.Empty;
        /// <summary>
        /// 公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi4Priority { get; set; } = string.Empty;


        /// <summary>
        /// 保険種別区分
        ///  0:保険なし 
        ///  1:主保険   
        ///  2:マル長   
        ///  3:労災  
        ///  4:自賠
        ///  5:生活保護 
        ///  6:分点公費
        ///  7:一般公費  
        ///  8:自費
        /// </summary>
        public int Kohi1HokenSbtKbn { get; set; }

        /// <summary>
        /// 保険種別区分
        ///  0:保険なし 
        ///  1:主保険   
        ///  2:マル長   
        ///  3:労災  
        ///  4:自賠
        ///  5:生活保護 
        ///  6:分点公費
        ///  7:一般公費  
        ///  8:自費
        /// </summary>
        public int Kohi2HokenSbtKbn { get; set; }

        /// <summary>
        /// 保険種別区分
        ///  0:保険なし 
        ///  1:主保険   
        ///  2:マル長   
        ///  3:労災  
        ///  4:自賠
        ///  5:生活保護 
        ///  6:分点公費
        ///  7:一般公費  
        ///  8:自費
        /// </summary>
        public int Kohi3HokenSbtKbn { get; set; }

        /// <summary>
        /// 保険種別区分
        ///  0:保険なし 
        ///  1:主保険   
        ///  2:マル長   
        ///  3:労災  
        ///  4:自賠
        ///  5:生活保護 
        ///  6:分点公費
        ///  7:一般公費  
        ///  8:自費
        /// </summary>
        public int Kohi4HokenSbtKbn { get; set; }

        /// <summary>
        /// 妊婦フラグ
        /// 1:妊婦
        /// </summary>
        public int IsNinpu { get; set; }


        /// <summary>
        /// 公費IDの取得
        /// </summary>
        public int GetKohiId(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Id;
                case 2:
                    return Kohi2Id;
                case 3:
                    return Kohi3Id;
                case 4:
                    return Kohi4Id;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費IDの設定
        /// </summary>
        public void SetKohiId(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1Id = value;
                    break;
                case 2:
                    Kohi2Id = value;
                    break;
                case 3:
                    Kohi3Id = value;
                    break;
                case 4:
                    Kohi4Id = value;
                    break;
            }
        }

        /// <summary>
        /// 公費優先順位の取得
        /// </summary>
        public string GetKohiPriority(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Priority ?? "".PadLeft(7, '9');
                case 2:
                    return Kohi2Priority ?? "".PadLeft(7, '9');
                case 3:
                    return Kohi3Priority ?? "".PadLeft(7, '9');
                case 4:
                    return Kohi4Priority ?? "".PadLeft(7, '9');
                default:
                    return "".PadLeft(7, '9');
            }
        }

        /// <summary>
        /// 公費優先順位の設定
        /// </summary>
        public void SetKohiPriority(int kohiNo, string value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1Priority = value;
                    break;
                case 2:
                    Kohi2Priority = value;
                    break;
                case 3:
                    Kohi3Priority = value;
                    break;
                case 4:
                    Kohi4Priority = value;
                    break;
            }
        }

        /// <summary>
        /// 公費法別番号の取得
        /// </summary>
        public string GetKohiHoubetu(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Houbetu ?? "";
                case 2:
                    return Kohi2Houbetu ?? "";
                case 3:
                    return Kohi3Houbetu ?? "";
                case 4:
                    return Kohi4Houbetu ?? "";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 公費法別番号の設定
        /// </summary>
        public void SetKohiHoubetu(int kohiNo, string value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1Houbetu = value;
                    break;
                case 2:
                    Kohi2Houbetu = value;
                    break;
                case 3:
                    Kohi3Houbetu = value;
                    break;
                case 4:
                    Kohi4Houbetu = value;
                    break;
            }
        }

        /// <summary>
        /// 高額療養費限度額の取得
        /// </summary>
        public int GetKogakuLimit(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return KogakuKohi1Limit;
                case 2:
                    return KogakuKohi2Limit;
                case 3:
                    return KogakuKohi3Limit;
                case 4:
                    return KogakuKohi4Limit;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 高額療養費限度額の設定
        /// </summary>
        public void SetKogakuLimit(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    KogakuKohi1Limit = value;
                    break;
                case 2:
                    KogakuKohi2Limit = value;
                    break;
                case 3:
                    KogakuKohi3Limit = value;
                    break;
                case 4:
                    KogakuKohi4Limit = value;
                    break;
            }
        }

        /// <summary>
        /// 公費負担限度額の取得
        /// </summary>
        public int GetKohiLimit(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Limit;
                case 2:
                    return Kohi2Limit;
                case 3:
                    return Kohi3Limit;
                case 4:
                    return Kohi4Limit;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費負担限度額の設定
        /// </summary>
        public void SetKohiLimit(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1Limit = value;
                    break;
                case 2:
                    Kohi2Limit = value;
                    break;
                case 3:
                    Kohi3Limit = value;
                    break;
                case 4:
                    Kohi4Limit = value;
                    break;
            }
        }

        /// <summary>
        /// 他院負担額の取得
        /// </summary>
        public int GetOtherFutan(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1OtherFutan;
                case 2:
                    return Kohi2OtherFutan;
                case 3:
                    return Kohi3OtherFutan;
                case 4:
                    return Kohi4OtherFutan;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 他院負担額の設定
        /// </summary>
        public void SetOtherFutan(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1OtherFutan = value;
                    break;
                case 2:
                    Kohi2OtherFutan = value;
                    break;
                case 3:
                    Kohi3OtherFutan = value;
                    break;
                case 4:
                    Kohi4OtherFutan = value;
                    break;
            }
        }

        /// <summary>
        /// 公費負担額の取得
        /// </summary>
        public int GetKohiFutan(int kohiNo, int futan10en = 0)
        {
            switch (kohiNo)
            {
                case 1:
                    return futan10en == 1 ? Kohi1Futan10en : Kohi1Futan;
                case 2:
                    return futan10en == 1 ? Kohi2Futan10en : Kohi2Futan;
                case 3:
                    return futan10en == 1 ? Kohi3Futan10en : Kohi3Futan;
                case 4:
                    return futan10en == 1 ? Kohi4Futan10en : Kohi4Futan;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費負担額の設定
        /// </summary>
        public void SetKohiFutan(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1Futan = value;
                    break;
                case 2:
                    Kohi2Futan = value;
                    break;
                case 3:
                    Kohi3Futan = value;
                    break;
                case 4:
                    Kohi4Futan = value;
                    break;
            }
        }

        /// <summary>
        /// 公費負担額(10円単位)の取得
        /// </summary>
        public int GetKohiFutan10en(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Futan10en;
                case 2:
                    return Kohi2Futan10en;
                case 3:
                    return Kohi3Futan10en;
                case 4:
                    return Kohi4Futan10en;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費負担額(10円単位)の設定
        /// </summary>
        public void SetKohiFutan10en(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1Futan10en = value;
                    break;
                case 2:
                    Kohi2Futan10en = value;
                    break;
                case 3:
                    Kohi3Futan10en = value;
                    break;
                case 4:
                    Kohi4Futan10en = value;
                    break;
            }
        }

        /// <summary>
        /// 公費分点数の設定
        /// </summary>
        /// <param name="kohiId"></param>
        /// <param name="value"></param>
        public void AddKohiTensu(int kohiId, int value)
        {
            if (kohiId == 0) return;

            if (kohiId == Kohi1Id)
            {
                Kohi1Tensu += value;
            }
            else if (kohiId == Kohi2Id)
            {
                Kohi2Tensu += value;
            }
            else if (kohiId == Kohi3Id)
            {
                Kohi3Tensu += value;
            }
            else if (kohiId == Kohi4Id)
            {
                Kohi4Tensu += value;
            }

        }

        /// <summary>
        /// 公費分一部負担額の設定
        /// </summary>
        /// <param name="kohiId"></param>
        /// <param name="value"></param>
        public void AddKohiIchibuFutan(int kohiId, int value)
        {
            if (kohiId == 0) return;

            if (kohiId == Kohi1Id)
            {
                Kohi1IchibuFutan += value;
            }
            else if (kohiId == Kohi2Id)
            {
                Kohi2IchibuFutan += value;
            }
            else if (kohiId == Kohi3Id)
            {
                Kohi3IchibuFutan += value;
            }
            else if (kohiId == Kohi4Id)
            {
                Kohi4IchibuFutan += value;
            }

        }

        /// <summary>
        /// 公費分一部負担相当額の設定
        /// </summary>
        /// <param name="kohiId"></param>
        /// <param name="value"></param>
        public void AddKohiIchibuSotogaku(int kohiId, int value)
        {
            if (kohiId == 0) return;

            if (kohiId == Kohi1Id)
            {
                Kohi1IchibuSotogaku += value;
            }
            else if (kohiId == Kohi2Id)
            {
                Kohi2IchibuSotogaku += value;
            }
            else if (kohiId == Kohi3Id)
            {
                Kohi3IchibuSotogaku += value;
            }
            else if (kohiId == Kohi4Id)
            {
                Kohi4IchibuSotogaku += value;
            }

        }

        /// <summary>
        /// 公費分一部負担相当額10円単位の設定
        /// </summary>
        /// <param name="kohiId"></param>
        /// <param name="value"></param>
        public void AddKohiIchibuSotogaku10en(int kohiId, int value)
        {
            if (kohiId == 0) return;

            if (kohiId == Kohi1Id)
            {
                Kohi1IchibuSotogaku10en += value;
            }
            else if (kohiId == Kohi2Id)
            {
                Kohi2IchibuSotogaku10en += value;
            }
            else if (kohiId == Kohi3Id)
            {
                Kohi3IchibuSotogaku10en += value;
            }
            else if (kohiId == Kohi4Id)
            {
                Kohi4IchibuSotogaku10en += value;
            }

        }

        /// <summary>
        /// 公費の保険種別区分の取得
        /// </summary>
        public int GetKohiHokenSbtKbn(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1HokenSbtKbn;
                case 2:
                    return Kohi2HokenSbtKbn;
                case 3:
                    return Kohi3HokenSbtKbn;
                case 4:
                    return Kohi4HokenSbtKbn;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費の保険種別区分の設定
        /// </summary>
        public void SetKohiHokenSbtKbn(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1HokenSbtKbn = value;
                    break;
                case 2:
                    Kohi2HokenSbtKbn = value;
                    break;
                case 3:
                    Kohi3HokenSbtKbn = value;
                    break;
                case 4:
                    Kohi4HokenSbtKbn = value;
                    break;
            }
        }

        public int GetKohiReceTensu(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1ReceTensu ?? 0;
                case 2:
                    return Kohi2ReceTensu ?? 0;
                case 3:
                    return Kohi3ReceTensu ?? 0;
                case 4:
                    return Kohi4ReceTensu ?? 0;
                default:
                    return 0;
            }
        }

        public int GetKohiReceFutan(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1ReceFutan ?? 0;
                case 2:
                    return Kohi2ReceFutan ?? 0;
                case 3:
                    return Kohi3ReceFutan ?? 0;
                case 4:
                    return Kohi4ReceFutan ?? 0;
                default:
                    return 0;
            }
        }

        public bool GetKohiReceKisai(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1ReceKisai;
                case 2:
                    return Kohi2ReceKisai;
                case 3:
                    return Kohi3ReceKisai;
                case 4:
                    return Kohi4ReceKisai;
                default:
                    return false;
            }
        }

        public bool TokkiContains(string tokkiCd)
        {
            return
                CIUtil.Copy(Tokki, 1, 2) == tokkiCd ||
                CIUtil.Copy(Tokki, 3, 2) == tokkiCd ||
                CIUtil.Copy(Tokki, 5, 2) == tokkiCd ||
                CIUtil.Copy(Tokki, 7, 2) == tokkiCd ||
                CIUtil.Copy(Tokki, 9, 2) == tokkiCd;
        }

        /// <summary>
        /// 特記事項の設定
        /// </summary>
        public void SetTokki(int seqNo, string value)
        {
            switch (seqNo)
            {
                case 1:
                    Tokki1 = value;
                    break;
                case 2:
                    Tokki2 = value;
                    break;
                case 3:
                    Tokki3 = value;
                    break;
                case 4:
                    Tokki4 = value;
                    break;
                case 5:
                    Tokki5 = value;
                    break;
            }

            Tokki = string.Empty;
            if (Tokki1 != null && Tokki1.Length >= 2) Tokki += Tokki1.Substring(0, 2);
            if (Tokki2 != null && Tokki2.Length >= 2) Tokki += Tokki2.Substring(0, 2);
            if (Tokki3 != null && Tokki3.Length >= 2) Tokki += Tokki3.Substring(0, 2);
            if (Tokki4 != null && Tokki4.Length >= 2) Tokki += Tokki4.Substring(0, 2);
            if (Tokki5 != null && Tokki5.Length >= 2) Tokki += Tokki5.Substring(0, 2);
        }

        /// <summary>
        /// 公費分点数の取得
        /// </summary>
        /// <param name="kohiNo"></param>
        /// <returns></returns>
        public int GetKohiTensu(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Tensu;
                case 2:
                    return Kohi2Tensu;
                case 3:
                    return Kohi3Tensu;
                case 4:
                    return Kohi4Tensu;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費分点数の取得
        /// </summary>
        public void SetKohiTensu(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1Tensu = value;
                    break;
                case 2:
                    Kohi2Tensu = value;
                    break;
                case 3:
                    Kohi3Tensu = value;
                    break;
                case 4:
                    Kohi4Tensu = value;
                    break;
            }
        }

        /// <summary>
        /// 公費分一部負担額の取得
        /// </summary>
        /// <param name="kohiNo"></param>
        /// <returns></returns>
        public int GetKohiIchibuFutan(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1IchibuFutan;
                case 2:
                    return Kohi2IchibuFutan;
                case 3:
                    return Kohi3IchibuFutan;
                case 4:
                    return Kohi4IchibuFutan;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費分一部負担相当額の取得
        /// </summary>
        /// <param name="kohiNo"></param>
        /// <returns></returns>
        public int GetKohiIchibuSotogaku(int kohiNo, int futan10en = 0)
        {
            switch (kohiNo)
            {
                case 1:
                    return futan10en == 1 ? Kohi1IchibuSotogaku10en : Kohi1IchibuSotogaku;
                case 2:
                    return futan10en == 1 ? Kohi2IchibuSotogaku10en : Kohi2IchibuSotogaku;
                case 3:
                    return futan10en == 1 ? Kohi3IchibuSotogaku10en : Kohi3IchibuSotogaku;
                case 4:
                    return futan10en == 1 ? Kohi4IchibuSotogaku10en : Kohi4IchibuSotogaku;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費分一部負担額の取得
        /// </summary>
        public void SetKohiIchibuFutan(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1IchibuFutan = value;
                    break;
                case 2:
                    Kohi2IchibuFutan = value;
                    break;
                case 3:
                    Kohi3IchibuFutan = value;
                    break;
                case 4:
                    Kohi4IchibuFutan = value;
                    break;
            }
        }

        /// <summary>
        /// 公費分一部負担相当額の取得
        /// </summary>
        public void SetKohiIchibuSotogaku(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1IchibuSotogaku = value;
                    break;
                case 2:
                    Kohi2IchibuSotogaku = value;
                    break;
                case 3:
                    Kohi3IchibuSotogaku = value;
                    break;
                case 4:
                    Kohi4IchibuSotogaku = value;
                    break;
            }
        }

        /// <summary>
        /// 公費分一部負担相当額10円単位の取得
        /// </summary>
        /// <param name="kohiNo"></param>
        /// <returns></returns>
        public int GetKohiIchibuSotogaku10en(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1IchibuSotogaku10en;
                case 2:
                    return Kohi2IchibuSotogaku10en;
                case 3:
                    return Kohi3IchibuSotogaku10en;
                case 4:
                    return Kohi4IchibuSotogaku10en;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費分一部負担相当額10円単位の取得
        /// </summary>
        public void SetKohiIchibuSotogaku10en(int kohiNo, int value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1IchibuSotogaku10en = value;
                    break;
                case 2:
                    Kohi2IchibuSotogaku10en = value;
                    break;
                case 3:
                    Kohi3IchibuSotogaku10en = value;
                    break;
                case 4:
                    Kohi4IchibuSotogaku10en = value;
                    break;
            }
        }

        /// <summary>
        /// 公費IDから番号を取得
        /// </summary>
        /// <param name="kohiId"></param>
        /// <returns></returns>
        public int GetKohiNo(int kohiId)
        {
            if (Kohi1Id == kohiId) return 1;
            if (Kohi2Id == kohiId) return 2;
            if (Kohi3Id == kohiId) return 3;
            if (Kohi4Id == kohiId) return 4;
            return 0;
        }

        /// <summary>
        /// 主保険の有無
        /// </summary>
        public bool IsNoHoken
        {
            get
            {
                if (HokenKbn == 1 && ReceSbt?.Substring(1, 1) == "2")
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 前期一般（指定公費あり）
        /// 
        /// </summary>
        public bool IsSiteiKohi
        {
            get
            {
                return
                    HokenRate == 10 &&
                    ReceSbt.Substring(1, 1) != "3" && ReceSbt.Substring(3, 1) == "8";
            }
        }

        /// <summary>
        /// 高齢者
        /// 
        /// </summary>
        public bool IsElder
        {
            get
            {
                return ReceSbt.Substring(3, 1) == "0" || ReceSbt.Substring(3, 1) == "8";
            }
        }

        /// <summary>
        /// 後期高齢者
        /// 
        /// </summary>
        public bool IsKouki
        {
            get
            {
                return
                    ReceSbt.Substring(1, 1) == "3" &&
                    (ReceSbt.Substring(3, 1) == "0" || ReceSbt.Substring(3, 1) == "8");
            }
        }

        public bool IsKokuhoKumiai
        {
            get
            {
                return
                    HokensyaNo.Length < 6 ? false :
                        HokenKbn == Helper.Constants.HokenKbn.Kokho &&
                        ReceSbt.Substring(1, 1) != "3" &&   //後期以外
                        HokensyaNo.Substring(HokensyaNo.Length - 6, 6).Substring(2, 1) == "3";
            }
        }


        /// <summary>
        /// 給付対象額の取得
        /// </summary>
        public int GetKohiKyufu(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1ReceKyufu ?? 0;
                case 2:
                    return Kohi2ReceKyufu ?? 0;
                case 3:
                    return Kohi3ReceKyufu ?? 0;
                case 4:
                    return Kohi4ReceKyufu ?? 0;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 給付対象額の設定
        /// </summary>
        public void SetKohiKyufu(int kohiNo, int value)
        {
            if (value == 0) return;

            switch (kohiNo)
            {
                case 1:
                    Kohi1ReceKyufu = value;
                    break;
                case 2:
                    Kohi2ReceKyufu = value;
                    break;
                case 3:
                    Kohi3ReceKyufu = value;
                    break;
                case 4:
                    Kohi4ReceKyufu = value;
                    break;
            }
        }

        /// <summary>
        /// 給付対象額の設定
        /// </summary>
        public void AddKohiKyufuKohiId(int kohiId, int value)
        {
            if (kohiId == 0 || value == 0) return;

            if (kohiId == Kohi1Id)
            {
                Kohi1ReceKyufu += value;
            }
            else if (kohiId == Kohi2Id)
            {
                Kohi2ReceKyufu += value;
            }
            else if (kohiId == Kohi3Id)
            {
                Kohi3ReceKyufu += value;
            }
            else if (kohiId == Kohi4Id)
            {
                Kohi4ReceKyufu += value;
            }
        }

        /// <summary>
        /// 高齢者
        /// 
        /// </summary>
        public int AgeKbn
        {
            get
            {
                return Convert.ToInt32(
                    ReceInf.ReceSbt.Substring(3, 1) == "0" || ReceInf.ReceSbt.Substring(3, 1) == "8"
                );
            }
        }

        #region '公費４種でマル長を省略した場合の一時格納用'
        public int Kohi0Id { get; set; }
        public int Kohi0Limit { get; set; }
        #endregion

        /// <summary>
        /// マル長
        /// </summary>
        //public bool IsMarucyo;

        #region 会計計算用
        /// <summary>
        /// 総実日数
        /// </summary>
        public int Nissu { get; set; }

        /// <summary>
        /// 被保険者証記号
        /// </summary>
        public string Kigo { get; set; } = string.Empty;
        /// <summary>
        /// 被保険者証番号
        /// </summary>
        public string Bango { get; set; } = string.Empty;

        public string EdaNo { get; set; } = string.Empty;

        /// <summary>
        /// 公１負担者番号
        /// </summary>
        public string Kohi1FutansyaNo { get; set; } = string.Empty;
        /// <summary>
        /// 公２負担者番号
        /// </summary>
        public string Kohi2FutansyaNo { get; set; } = string.Empty;
        /// <summary>
        /// 公３負担者番号
        /// </summary>
        public string Kohi3FutansyaNo { get; set; } = string.Empty;
        /// <summary>
        /// 公４負担者番号
        /// </summary>
        public string Kohi4FutansyaNo { get; set; } = string.Empty;

        /// <summary>
        /// 公１受給者番号
        /// </summary>
        public string Kohi1JyukyusyaNo { get; set; } = string.Empty;
        /// <summary>
        /// 公２受給者番号
        /// </summary>
        public string Kohi2JyukyusyaNo { get; set; } = string.Empty;
        /// <summary>
        /// 公３受給者番号
        /// </summary>
        public string Kohi3JyukyusyaNo { get; set; } = string.Empty;
        /// <summary>
        /// 公４受給者番号
        /// </summary>
        public string Kohi4JyukyusyaNo { get; set; } = string.Empty;
        /// <summary>
        /// 公１特殊番号
        /// </summary>
        public string Kohi1TokusyuNo { get; set; } = string.Empty;
        /// <summary>
        /// 公２特殊番号
        /// </summary>
        public string Kohi2TokusyuNo { get; set; } = string.Empty;
        /// <summary>
        /// 公３特殊番号
        /// </summary>
        public string Kohi3TokusyuNo { get; set; } = string.Empty;
        /// <summary>
        /// 公４特殊番号
        /// </summary>
        public string Kohi4TokusyuNo { get; set; } = string.Empty;

        /// <summary>
        /// 公費の負担者番号の設定
        /// </summary>
        public void SetKohiFutansyaNo(int kohiNo, string value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1FutansyaNo = value;
                    break;
                case 2:
                    Kohi2FutansyaNo = value;
                    break;
                case 3:
                    Kohi3FutansyaNo = value;
                    break;
                case 4:
                    Kohi4FutansyaNo = value;
                    break;
            }
        }

        /// <summary>
        /// 公費の受給者番号の設定
        /// </summary>
        public void SetKohiJyukyusyaNo(int kohiNo, string value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1JyukyusyaNo = value;
                    break;
                case 2:
                    Kohi2JyukyusyaNo = value;
                    break;
                case 3:
                    Kohi3JyukyusyaNo = value;
                    break;
                case 4:
                    Kohi4JyukyusyaNo = value;
                    break;
            }
        }

        /// <summary>
        /// 公費の特殊番号の設定
        /// </summary>
        public void SetKohiTokusyuNo(int kohiNo, string value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1TokusyuNo = value;
                    break;
                case 2:
                    Kohi2TokusyuNo = value;
                    break;
                case 3:
                    Kohi3TokusyuNo = value;
                    break;
                case 4:
                    Kohi4TokusyuNo = value;
                    break;
            }
        }

        /// <summary>
        /// 公費制度略称の設定
        /// </summary>
        public void SetKohiNameCd(int kohiNo, string value)
        {
            switch (kohiNo)
            {
                case 1:
                    Kohi1NameCd = value;
                    break;
                case 2:
                    Kohi2NameCd = value;
                    break;
                case 3:
                    Kohi3NameCd = value;
                    break;
                case 4:
                    Kohi3NameCd = value;
                    break;
            }
        }


        /// <summary>
        /// 高額療養費の指定公費計算用（保険分一部負担額）
        /// </summary>
        public int KogakuIchibuFutan { get; set; }


        /// <summary>
        /// 高額療養費の指定公費計算用（２割換算の一部負担額）
        /// </summary>
        public int SiteiKohiIchibuFutan
        {
            get
            {
                return
                    KogakuIchibuFutan * 2 +
                    (Kohi1HokenSbtKbn == HokenSbtKbn.Bunten ? Kohi1ReceFutan ?? Kohi1IchibuSotogaku * 2 : 0) +
                    (Kohi2HokenSbtKbn == HokenSbtKbn.Bunten ? Kohi2ReceFutan ?? Kohi2IchibuSotogaku * 2 : 0) +
                    (Kohi3HokenSbtKbn == HokenSbtKbn.Bunten ? Kohi3ReceFutan ?? Kohi3IchibuSotogaku * 2 : 0) +
                    (Kohi4HokenSbtKbn == HokenSbtKbn.Bunten ? Kohi4ReceFutan ?? Kohi4IchibuSotogaku * 2 : 0);
            }
        }
        #endregion
    }

}
