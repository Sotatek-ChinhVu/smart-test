using Entity.Tenant;
using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class ReceInfModel 
    {
        public ReceInf ReceInf { get; } = null;
        public PtInf PtInf { get; } = null;
        public PtHokenInf PtHokenInf { get; } = null;
        public HokenMst HokenMst { get; } = null;

        public ReceSeikyu ReceSeikyu { get; } = null;

        public ReceStatus ReceStatus { get; } = null;

        public PtGrpInf PtGrpInf { get; } = null;

        private int _rousaiCount;

        public ReceInfModel(ReceInf receInf, PtInf ptInf, PtHokenInf ptHokenInf,HokenMst hokenMst, ReceSeikyu receSeikyu, ReceStatus receStatus, PtGrpInf ptGrpInf, int rousaiCount)
        {
            ReceInf = receInf;
            PtInf = ptInf;
            PtHokenInf = ptHokenInf;
            HokenMst = hokenMst;
            ReceSeikyu = receSeikyu;
            ReceStatus = receStatus;
            //if(ReceStatus == null)
            //{
            //    ReceStatus = new ReceStatus();
            //    ReceStatus.HpId = ReceInf.HpId;
            //    ReceStatus.PtId = ReceInf.PtId;
            //    ReceStatus.SinYm = ReceInf.SinYm;
            //    ReceStatus.SeikyuYm = ReceInf.SeikyuYm;
            //    ReceStatus.HokenId = ReceInf.HokenId;
            //    ReceStatusAddNew = true;
            //}
            PtGrpInf = ptGrpInf;
            _rousaiCount = rousaiCount;
        }

        public bool ReceStatusAddNew = false;

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return ReceInf.HpId; }
        }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        public int SeikyuYm
        {
            get { return ReceInf.SeikyuYm; }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return ReceInf.PtId; }
        }

        public long PtNum
        {
            get { return PtInf == null ? 0 : PtInf.PtNum; }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return ReceInf.SinYm; }
        }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return ReceInf.HokenId; }
        }
        /// <summary>
        /// 主保険保険ID2
        /// 
        /// </summary>
        public int HokenId2
        {
            get { return ReceInf.HokenId2; }
        }
        /// <summary>
        /// 公費１保険ID
        /// 
        /// </summary>
        public int Kohi1Id
        {
            get { return ReceInf.Kohi1Id; }
        }

        /// <summary>
        /// 公費２保険ID
        /// 
        /// </summary>
        public int Kohi2Id
        {
            get { return ReceInf.Kohi2Id; }
        }

        /// <summary>
        /// 公費３保険ID
        /// 
        /// </summary>
        public int Kohi3Id
        {
            get { return ReceInf.Kohi3Id; }
        }

        /// <summary>
        /// 公費４保険ID
        /// 
        /// </summary>
        public int Kohi4Id
        {
            get { return ReceInf.Kohi4Id; }
        }

        public int KohiId(int index)
        {
            int ret = 0;

            switch(index)
            {
                case 1:
                    ret = Kohi1Id;
                    break;
                case 2:
                    ret = Kohi2Id;
                    break;
                case 3:
                    ret = Kohi3Id;
                    break;
                case 4:
                    ret = Kohi4Id;
                    break;
            }

            return ret;
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
            get { return ReceInf.ReceSbt ?? string.Empty; }
            set { ReceInf.ReceSbt = value; }
        }

        /// <summary>
        /// 保険者番号
        /// 
        /// </summary>
        public string HokensyaNo
        {
            get { return ReceInf.HokensyaNo ?? ""; }
        }

        /// <summary>
        /// 法別番号
        /// 
        /// </summary>
        public string Houbetu
        {
            get { return ReceInf.Houbetu ?? ""; }
        }

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        public string Kohi1Houbetu
        {
            get { return ReceInf.Kohi1Houbetu ?? ""; }
        }

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        public string Kohi2Houbetu
        {
            get { return ReceInf.Kohi2Houbetu ?? ""; }
        }

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        public string Kohi3Houbetu
        {
            get { return ReceInf.Kohi3Houbetu ?? ""; }
        }

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        public string Kohi4Houbetu
        {
            get { return ReceInf.Kohi4Houbetu ?? ""; }
        }
        /// <summary>
        /// 公費法別番号
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string KohiHoubetu(int index)
        {
            string ret = "";

            switch (index)
            {
                case 1:
                    ret = Kohi1Houbetu;
                    break;
                case 2:
                    ret = Kohi2Houbetu;
                    break;
                case 3:
                    ret = Kohi3Houbetu;
                    break;
                case 4:
                    ret = Kohi4Houbetu;
                    break;
            }

            return ret;
        }
        /// <summary>
        /// 本人家族区分
        /// 1:本人 2:家族
        /// </summary>
        public int HonkeKbn
        {
            get { return ReceInf.HonkeKbn; }
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
        }

        /// <summary>
        /// 高額療養費適用区分
        /// 
        /// </summary>
        public int KogakuTekiyoKbn
        {
            get { return ReceInf.KogakuTekiyoKbn; }
        }

        /// <summary>
        /// 限度額特例フラグ
        /// 
        /// </summary>
        public int IsTokurei
        {
            get { return ReceInf.IsTokurei; }
        }

        /// <summary>
        /// 多数回該当フラグ
        /// 
        /// </summary>
        public int IsTasukai
        {
            get { return ReceInf.IsTasukai; }
        }

        /// <summary>
        /// 高額療養費公１限度額
        /// 
        /// </summary>
        public int KogakuKohi1Limit
        {
            get { return ReceInf.KogakuKohi1Limit; }
        }

        /// <summary>
        /// 高額療養費公２限度額
        /// 
        /// </summary>
        public int KogakuKohi2Limit
        {
            get { return ReceInf.KogakuKohi2Limit; }
        }

        /// <summary>
        /// 高額療養費公３限度額
        /// 
        /// </summary>
        public int KogakuKohi3Limit
        {
            get { return ReceInf.KogakuKohi3Limit; }
        }

        /// <summary>
        /// 高額療養費公４限度額
        /// 
        /// </summary>
        public int KogakuKohi4Limit
        {
            get { return ReceInf.KogakuKohi4Limit; }
        }

        /// <summary>
        /// 高額療養費限度額(合算)
        /// 
        /// </summary>
        public int TotalKogakuLimit
        {
            get { return ReceInf.TotalKogakuLimit; }
        }

        /// <summary>
        /// 国保減免区分
        ///     1:減額 2:免除 3:支払猶予 4:自立支援減免
        /// </summary>
        public int GenmenKbn
        {
            get { return ReceInf.GenmenKbn; }
        }

        /// <summary>
        /// 保険負担率
        /// 
        /// </summary>
        public int HokenRate
        {
            get 
            { 
                int ret = ReceInf.HokenRate;
            
                if(ReceInf.HokenRate == 10 &&
                    ReceInf.ReceSbt.Substring(1, 1) != "3" && ReceInf.ReceSbt.Substring(3, 1) == "8")
                {
                    ret = 20;
                }

                return ret;
            }
        }

        /// <summary>
        /// 患者負担率
        /// 
        /// </summary>
        public int PtRate
        {
            get { return ReceInf.PtRate; }
        }

        /// <summary>
        /// 公１負担限度額
        /// 
        /// </summary>
        public int Kohi1Limit
        {
            get { return ReceInf.Kohi1Limit; }
        }

        /// <summary>
        /// 公２負担限度額
        /// 
        /// </summary>
        public int Kohi2Limit
        {
            get { return ReceInf.Kohi2Limit; }
        }

        /// <summary>
        /// 公３負担限度額
        /// 
        /// </summary>
        public int Kohi3Limit
        {
            get { return ReceInf.Kohi3Limit; }
        }

        /// <summary>
        /// 公４負担限度額
        /// 
        /// </summary>
        public int Kohi4Limit
        {
            get { return ReceInf.Kohi4Limit; }
        }

        /// <summary>
        /// 診療点数
        /// 
        /// </summary>
        public int Tensu
        {
            get { return ReceInf.Tensu; }
        }

        /// <summary>
        /// 保険分点数
        /// 
        /// </summary>
        public int HokenTensu
        {
            get { return ReceInf.HokenTensu; }
        }

        /// <summary>
        /// 公１分点数
        /// 
        /// </summary>
        public int Kohi1Tensu
        {
            get { return ReceInf.Kohi1Tensu; }
        }

        /// <summary>
        /// 公２分点数
        /// 
        /// </summary>
        public int Kohi2Tensu
        {
            get { return ReceInf.Kohi2Tensu; }
        }

        /// <summary>
        /// 公３分点数
        /// 
        /// </summary>
        public int Kohi3Tensu
        {
            get { return ReceInf.Kohi3Tensu; }
        }

        /// <summary>
        /// 公４分点数
        /// 
        /// </summary>
        public int Kohi4Tensu
        {
            get { return ReceInf.Kohi4Tensu; }
        }

        /// <summary>
        /// 総医療費
        /// 
        /// </summary>
        public int TotalIryohi
        {
            get { return ReceInf.TotalIryohi; }
        }

        /// <summary>
        /// 保険負担額
        /// 
        /// </summary>
        public int HokenFutan
        {
            get { return ReceInf.HokenFutan; }
        }

        /// <summary>
        /// 高額負担額
        /// 
        /// </summary>
        public int KogakuFutan
        {
            get { return ReceInf.KogakuFutan; }
        }

        /// <summary>
        /// 公１負担額
        /// 
        /// </summary>
        public int Kohi1Futan
        {
            get { return ReceInf.Kohi1Futan; }
        }

        /// <summary>
        /// 公２負担額
        /// 
        /// </summary>
        public int Kohi2Futan
        {
            get { return ReceInf.Kohi2Futan; }
        }

        /// <summary>
        /// 公３負担額
        /// 
        /// </summary>
        public int Kohi3Futan
        {
            get { return ReceInf.Kohi3Futan; }
        }

        /// <summary>
        /// 公４負担額
        /// 
        /// </summary>
        public int Kohi4Futan
        {
            get { return ReceInf.Kohi4Futan; }
        }

        /// <summary>
        /// 一部負担額
        /// 
        /// </summary>
        public int IchibuFutan
        {
            get { return ReceInf.IchibuFutan; }
        }

        /// <summary>
        /// 減免額
        /// 
        /// </summary>
        public int GenmenGaku
        {
            get { return ReceInf.GenmenGaku; }
        }

        /// <summary>
        /// 保険負担額10円単位
        /// 
        /// </summary>
        public int HokenFutan10en
        {
            get { return ReceInf.HokenFutan10en; }
        }

        /// <summary>
        /// 高額負担額10円単位
        /// 
        /// </summary>
        public int KogakuFutan10en
        {
            get { return ReceInf.KogakuFutan10en; }
        }

        /// <summary>
        /// 公１負担額10円単位
        /// 
        /// </summary>
        public int Kohi1Futan10en
        {
            get { return ReceInf.Kohi1Futan10en; }
        }

        /// <summary>
        /// 公２負担額10円単位
        /// 
        /// </summary>
        public int Kohi2Futan10en
        {
            get { return ReceInf.Kohi2Futan10en; }
        }

        /// <summary>
        /// 公３負担額10円単位
        /// 
        /// </summary>
        public int Kohi3Futan10en
        {
            get { return ReceInf.Kohi3Futan10en; }
        }

        /// <summary>
        /// 公４負担額10円単位
        /// 
        /// </summary>
        public int Kohi4Futan10en
        {
            get { return ReceInf.Kohi4Futan10en; }
        }

        /// <summary>
        /// 一部負担額10円単位
        /// 
        /// </summary>
        public int IchibuFutan10en
        {
            get { return ReceInf.IchibuFutan10en; }
        }

        /// <summary>
        /// 減免額10円単位
        /// 
        /// </summary>
        public int GenmenGaku10en
        {
            get { return ReceInf.GenmenGaku10en; }
        }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        public int PtFutan
        {
            get { return ReceInf.PtFutan; }
        }

        /// <summary>
        /// 高額療養費超過区分
        /// 
        /// </summary>
        public int KogakuOverKbn
        {
            get { return ReceInf.KogakuOverKbn; }
        }

        /// <summary>
        /// 保険レセ点数
        /// 
        /// </summary>
        public int? HokenReceTensu
        {
            get { return ReceInf.HokenReceTensu; }
        }

        /// <summary>
        /// 保険レセ負担額
        /// 
        /// </summary>
        public int? HokenReceFutan
        {
            get { return ReceInf.HokenReceFutan; }
        }

        /// <summary>
        /// 公１レセ点数
        /// 
        /// </summary>
        public int? Kohi1ReceTensu
        {
            get { return ReceInf.Kohi1ReceTensu; }
        }

        /// <summary>
        /// 公１レセ負担額
        /// 
        /// </summary>
        public int? Kohi1ReceFutan
        {
            get { return ReceInf.Kohi1ReceFutan; }
        }

        /// <summary>
        /// 公１レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi1ReceKyufu
        {
            get { return ReceInf.Kohi1ReceKyufu; }
        }

        /// <summary>
        /// 公２レセ点数
        /// 
        /// </summary>
        public int? Kohi2ReceTensu
        {
            get { return ReceInf.Kohi2ReceTensu; }
        }

        /// <summary>
        /// 公２レセ負担額
        /// 
        /// </summary>
        public int? Kohi2ReceFutan
        {
            get { return ReceInf.Kohi2ReceFutan; }
        }

        /// <summary>
        /// 公２レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi2ReceKyufu
        {
            get { return ReceInf.Kohi2ReceKyufu; }
        }

        /// <summary>
        /// 公３レセ点数
        /// 
        /// </summary>
        public int? Kohi3ReceTensu
        {
            get { return ReceInf.Kohi3ReceTensu; }
        }

        /// <summary>
        /// 公３レセ負担額
        /// 
        /// </summary>
        public int? Kohi3ReceFutan
        {
            get { return ReceInf.Kohi3ReceFutan; }
        }

        /// <summary>
        /// 公３レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi3ReceKyufu
        {
            get { return ReceInf.Kohi3ReceKyufu; }
        }

        /// <summary>
        /// 公４レセ点数
        /// 
        /// </summary>
        public int? Kohi4ReceTensu
        {
            get { return ReceInf.Kohi4ReceTensu; }
        }

        /// <summary>
        /// 公４レセ負担額
        /// 
        /// </summary>
        public int? Kohi4ReceFutan
        {
            get { return ReceInf.Kohi4ReceFutan; }
        }

        /// <summary>
        /// 公４レセ給付対象額
        /// 
        /// </summary>
        public int? Kohi4ReceKyufu
        {
            get { return ReceInf.Kohi4ReceKyufu; }
        }

        public int? KohiReceTensu(int index)
        {
            int? ret = null;

            switch (index)
            {
                case 1:
                    ret = Kohi1ReceTensu;
                    break;
                case 2:
                    ret = Kohi2ReceTensu;
                    break;
                case 3:
                    ret = Kohi3ReceTensu;
                    break;
                case 4:
                    ret = Kohi4ReceTensu;
                    break;
            }

            return ret;
        }
        public int? KohiTensu(int index)
        {
            int? ret = null;

            switch (index)
            {
                case 1:
                    ret = Kohi1Tensu;
                    break;
                case 2:
                    ret = Kohi2Tensu;
                    break;
                case 3:
                    ret = Kohi3Tensu;
                    break;
                case 4:
                    ret = Kohi4Tensu;
                    break;
            }

            return ret;
        }
        public int? KohiReceKyufu(int index)
        {
            int? ret = null;

            switch (index)
            {
                case 1:
                    ret = Kohi1ReceKyufu;
                    break;
                case 2:
                    ret = Kohi2ReceKyufu;
                    break;
                case 3:
                    ret = Kohi3ReceKyufu;
                    break;
                case 4:
                    ret = Kohi4ReceKyufu;
                    break;
            }

            return ret;
        }

        public int? KohiReceFutan(int index)
        {
            int? ret = null;

            switch (index)
            {
                case 1:
                    ret = Kohi1ReceFutan;
                    break;
                case 2:
                    ret = Kohi2ReceFutan;
                    break;
                case 3:
                    ret = Kohi3ReceFutan;
                    break;
                case 4:
                    ret = Kohi4ReceFutan;
                    break;
            }

            return ret;
        }
        /// <summary>
        /// 公費負担額
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int? KohiFutan(int index)
        {
            int? ret = null;

            switch (index)
            {
                case 1:
                    ret = Kohi1Futan;
                    break;
                case 2:
                    ret = Kohi2Futan;
                    break;
                case 3:
                    ret = Kohi3Futan;
                    break;
                case 4:
                    ret = Kohi4Futan;
                    break;
            }

            return ret;
        }
        /// <summary>
        /// 公費負担額10円単位
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int? KohiFutan10en(int index)
        {
            int? ret = null;

            switch (index)
            {
                case 1:
                    ret = Kohi1Futan10en;
                    break;
                case 2:
                    ret = Kohi2Futan10en;
                    break;
                case 3:
                    ret = Kohi3Futan10en;
                    break;
                case 4:
                    ret = Kohi4Futan10en;
                    break;
            }

            return ret;
        }
        /// <summary>
        /// 保険実日数
        /// 
        /// </summary>
        public int? HokenNissu
        {
            get { return ReceInf.HokenNissu; }
        }

        /// <summary>
        /// 公１実日数
        /// 
        /// </summary>
        public int? Kohi1Nissu
        {
            get { return ReceInf.Kohi1Nissu; }
        }

        /// <summary>
        /// 公２実日数
        /// 
        /// </summary>
        public int? Kohi2Nissu
        {
            get { return ReceInf.Kohi2Nissu; }
        }

        /// <summary>
        /// 公３実日数
        /// 
        /// </summary>
        public int? Kohi3Nissu
        {
            get { return ReceInf.Kohi3Nissu; }
        }

        /// <summary>
        /// 公４実日数
        /// 
        /// </summary>
        public int? Kohi4Nissu
        {
            get { return ReceInf.Kohi4Nissu; }
        }

        public int? KohiNissu(int index)
        {
            int? ret = null;

            switch(index)
            {
                case 1:
                    ret = Kohi1Nissu;
                    break;
                case 2:
                    ret = Kohi2Nissu;
                    break;
                case 3:
                    ret = Kohi3Nissu;
                    break;
                case 4:
                    ret = Kohi4Nissu;
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 公１レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public int Kohi1ReceKisai
        {
            get { return ReceInf.Kohi1ReceKisai; }
            set { ReceInf.Kohi1ReceKisai = value; }
        }

        /// <summary>
        /// 公２レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public int Kohi2ReceKisai
        {
            get { return ReceInf.Kohi2ReceKisai; }
            set { ReceInf.Kohi2ReceKisai = value; }
        }

        /// <summary>
        /// 公３レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public int Kohi3ReceKisai
        {
            get { return ReceInf.Kohi3ReceKisai; }
            set { ReceInf.Kohi3ReceKisai = value; }
        }

        /// <summary>
        /// 公４レセ記載
        /// 0:記載しない 1:記載する
        /// </summary>
        public int Kohi4ReceKisai
        {
            get { return ReceInf.Kohi4ReceKisai; }
            set { ReceInf.Kohi4ReceKisai = value; }
        }

        /// <summary>
        /// 公費レセ記載
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int KohiReceKisai(int index)
        {
            int ret = 0;

            switch (index)
            {
                case 1:
                    ret = Kohi1ReceKisai;
                    break;
                case 2:
                    ret = Kohi2ReceKisai;
                    break;
                case 3:
                    ret = Kohi3ReceKisai;
                    break;
                case 4:
                    ret = Kohi4ReceKisai;
                    break;
            }

            return ret;
        }
        public int KohiReceKisai(int index, int value)
        {
            int ret = 0;

            switch (index)
            {
                case 1:
                    Kohi1ReceKisai = value;
                    break;
                case 2:
                    Kohi2ReceKisai = value;
                    break;
                case 3:
                    Kohi3ReceKisai = value;
                    break;
                case 4:
                    Kohi4ReceKisai = value;
                    break;
            }

            return ret;
        }
        /// <summary>
        /// 公１制度略号
        /// 
        /// </summary>
        public string Kohi1NameCd
        {
            get { return ReceInf.Kohi1NameCd ?? string.Empty; }
        }

        /// <summary>
        /// 公２制度略号
        /// 
        /// </summary>
        public string Kohi2NameCd
        {
            get { return ReceInf.Kohi2NameCd ?? string.Empty; }
        }

        /// <summary>
        /// 公３制度略号
        /// 
        /// </summary>
        public string Kohi3NameCd
        {
            get { return ReceInf.Kohi3NameCd ?? string.Empty; }
        }

        /// <summary>
        /// 公４制度略号
        /// 
        /// </summary>
        public string Kohi4NameCd
        {
            get { return ReceInf.Kohi4NameCd ?? string.Empty; }
        }

        /// <summary>
        /// 請求区分
        /// 1:月遅れ 2:返戻 3:オンライン返戻
        /// </summary>
        public int SeikyuKbn
        {
            get { return ReceInf.SeikyuKbn; }
        }

        /// <summary>
        /// 特記事項
        /// 
        /// </summary>
        public string Tokki
        {
            get { return ReceInf.Tokki ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項１
        /// 
        /// </summary>
        public string Tokki1
        {
            get { return ReceInf.Tokki1 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項２
        /// 
        /// </summary>
        public string Tokki2
        {
            get { return ReceInf.Tokki2 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項３
        /// 
        /// </summary>
        public string Tokki3
        {
            get { return ReceInf.Tokki3 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項４
        /// 
        /// </summary>
        public string Tokki4
        {
            get { return ReceInf.Tokki4 ?? string.Empty; }
        }

        /// <summary>
        /// 特記事項５
        /// 
        /// </summary>
        public string Tokki5
        {
            get { return ReceInf.Tokki5 ?? string.Empty; }
        }

        public bool TokkiContains(string tokkiCd)
        {
            return
                CIUtil.Copy(ReceInf.Tokki, 1, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki, 3, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki, 5, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki, 7, 2) == tokkiCd ||
                CIUtil.Copy(ReceInf.Tokki, 9, 2) == tokkiCd;
        }

        /// <summary>
        /// 患者の状態
        /// 
        /// </summary>
        public string PtStatus
        {
            get { return ReceInf.PtStatus ?? string.Empty; }
        }

        /// <summary>
        /// 労災イ点負担額
        /// 
        /// </summary>
        public int RousaiIFutan
        {
            get { return ReceInf.RousaiIFutan; }
        }

        /// <summary>
        /// 労災ロ円負担額
        /// 
        /// </summary>
        public int RousaiRoFutan
        {
            get { return ReceInf.RousaiRoFutan; }
        }

        /// <summary>
        /// 自賠イ技術点数
        /// 
        /// </summary>
        public int JibaiITensu
        {
            get { return ReceInf.JibaiITensu; }
        }

        /// <summary>
        /// 自賠ロ薬剤点数
        /// 
        /// </summary>
        public int JibaiRoTensu
        {
            get { return ReceInf.JibaiRoTensu; }
        }

        /// <summary>
        /// 自賠ハ円診察負担額
        /// 
        /// </summary>
        public int JibaiHaFutan
        {
            get { return ReceInf.JibaiHaFutan; }
        }

        /// <summary>
        /// 自賠ニ円他負担額
        /// 
        /// </summary>
        public int JibaiNiFutan
        {
            get { return ReceInf.JibaiNiFutan; }
        }

        /// <summary>
        /// 自賠ホ診断書料
        /// 
        /// </summary>
        public int JibaiHoSindan
        {
            get { return ReceInf.JibaiHoSindan; }
        }

        /// <summary>
        /// 自賠ヘ明細書料
        /// 
        /// </summary>
        public int JibaiHeMeisai
        {
            get { return ReceInf.JibaiHeMeisai; }
        }

        /// <summary>
        /// 自賠Ａ負担額
        /// 
        /// </summary>
        public int JibaiAFutan
        {
            get { return ReceInf.JibaiAFutan; }
        }

        /// <summary>
        /// 自賠Ｂ負担額
        /// 
        /// </summary>
        public int JibaiBFutan
        {
            get { return ReceInf.JibaiBFutan; }
        }

        /// <summary>
        /// 自賠Ｃ負担額
        /// 
        /// </summary>
        public int JibaiCFutan
        {
            get { return ReceInf.JibaiCFutan; }
        }

        /// <summary>
        /// 自賠Ｄ負担額
        /// 
        /// </summary>
        public int JibaiDFutan
        {
            get { return ReceInf.JibaiDFutan; }
        }

        /// <summary>
        /// 自賠健保点数
        /// </summary>
        public int JibaiKenpoTensu
        {
            get { return ReceInf.JibaiKenpoTensu; }
        }
        /// <summary>
        /// 自賠健保負担額
        /// 
        /// </summary>
        public int JibaiKenpoFutan
        {
            get { return ReceInf.JibaiKenpoFutan; }
        }

        /// <summary>
        /// 新継再別
        /// 
        /// </summary>
        public int Sinkei
        {
            get { return ReceInf.Sinkei; }
        }

        /// <summary>
        /// 転帰事由
        /// 
        /// </summary>
        public int Tenki
        {
            get { return ReceInf.Tenki; }
        }

        /// <summary>
        /// 診療科ID
        /// 
        /// </summary>
        public int KaId
        {
            get { return ReceInf.KaId; }
        }

        /// <summary>
        /// 担当医ID
        /// 
        /// </summary>
        public int TantoId
        {
            get { return ReceInf.TantoId; }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return ReceInf.CreateDate; }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return ReceInf.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return ReceInf.CreateMachine ?? string.Empty; }
        }

        public int RousaiCount
        {
            get { return _rousaiCount; }
        }

        /// <summary>
        /// 労働局コード
        /// </summary>
        public string RoudoukyokuCd
        {
            get
            {
                string ret = "";

                if(PtHokenInf != null)
                {
                    ret = PtHokenInf.RousaiRoudouCd;
                }

                return ret;
            }
        }

        /// <summary>
        /// 監督署コード
        /// </summary>
        public string KantokusyoCd
        {
            get
            {
                string ret = "";

                if (PtHokenInf != null)
                {
                    ret = PtHokenInf.RousaiKantokuCd;
                }

                return ret;
            }
        }

        /// <summary>
        /// 旧保険ID
        /// </summary>
        public int PreHokenId
        {
            get
            {
                return ReceSeikyu == null ? 0 : ReceSeikyu.PreHokenId;
            }
        }

        /// <summary>
        /// レセプト出力フラグ
        /// 1:出力済み
        /// </summary>
        public int Output
        {
            get { return ReceStatus == null ? 0 : ReceStatus.Output; }
            set
            {
                if (ReceStatus == null || ReceStatus.Output == value) return;
                ReceStatus.Output = value;
                //RaisePropertyChanged(() => Output);
            }
        }
        public DateTime ReceStatusCreateDate
        {
            get { return ReceStatus == null ? CIUtil.GetJapanDateTimeNow() : ReceStatus.CreateDate; }
            set
            {
                if (ReceStatus == null || ReceStatus.CreateDate == value) return;
                ReceStatus.CreateDate = value;
                //RaisePropertyChanged(() => ReceStatusCreateDate);
            }
        }
        public int ReceStatusCreateId
        {
            get { return ReceStatus == null ? 0 : ReceStatus.CreateId; }
            set
            {
                if (ReceStatus == null || ReceStatus.CreateId == value) return;
                ReceStatus.CreateId = value;
                //RaisePropertyChanged(() => ReceStatusCreateId);
            }
        }
        public string ReceStatusCreateMachine
        {
            get { return ReceStatus == null ? "" : ReceStatus.CreateMachine; }
            set
            {
                if (ReceStatus == null || ReceStatus.CreateMachine == value) return;
                ReceStatus.CreateMachine = value;
                //RaisePropertyChanged(() => ReceStatusCreateMachine);
            }
        }
        public DateTime ReceStatusUpdateDate
        {
            get { return ReceStatus == null ? CIUtil.GetJapanDateTimeNow() : ReceStatus.UpdateDate; }
            set
            {
                if (ReceStatus == null || ReceStatus.UpdateDate == value) return;
                ReceStatus.UpdateDate = value;
                //RaisePropertyChanged(() => ReceStatusUpdateDate);
            }
        }
        public int ReceStatusUpdateId
        {
            get { return ReceStatus == null ? 0  : ReceStatus.UpdateId; }
            set
            {
                if (ReceStatus == null || ReceStatus.UpdateId == value) return;
                ReceStatus.UpdateId = value;
                //RaisePropertyChanged(() => ReceStatusUpdateId);
            }
        }
        public string ReceStatusUpdateMachine
        {
            get { return ReceStatus == null ? "" : ReceStatus.UpdateMachine; }
            set
            {
                if (ReceStatus == null || ReceStatus.UpdateMachine == value) return;
                ReceStatus.UpdateMachine = value;
                //RaisePropertyChanged(() => ReceStatusUpdateMachine);
            }
        }
        /// <summary>
        /// 自賠受傷日
        /// </summary>
        public int JibaiJyusyouDate
        {
            get => PtHokenInf == null ? 0 : PtHokenInf.JibaiJyusyouDate;
        }
        /// <summary>
        /// 点数単価
        /// </summary>
        public int EnTen
        {
            get => HokenMst == null ? 0 : HokenMst.EnTen;
        }
        /// <summary>
        /// 自賠責保険会社名
        /// </summary>
        public string JibaiHokenName
        {
            get => PtHokenInf == null ? "" : PtHokenInf.JibaiHokenName;
        }
        /// <summary>
        /// 自賠責初診日
        /// </summary>
        public int JibaiSyosinDate
        {
            get => PtHokenInf == null ? 0 : PtHokenInf.RyoyoStartDate;
        }
        /// <summary>
        /// 自賠責診断書枚数
        /// </summary>
        public int JibaiHoSindanCount
        {
            get => ReceInf.JibaiHoSindanCount;
        }
        /// <summary>
        /// 自賠責明細書枚数
        /// </summary>
        public int JibaiHeMeisaiCount
        {
            get => ReceInf.JibaiHeMeisaiCount;
        }
        /// <summary>
        /// グループコード
        /// </summary>
        public string GrpCd
        {
            get => PtGrpInf != null ? (PtGrpInf.GroupCode ?? "") : "";
        }
    }
}
