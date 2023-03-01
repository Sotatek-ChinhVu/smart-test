using Domain.Constant;
using EmrCalculateApi.Constants;
using Entity.Tenant;
using Helper.Constants;

namespace EmrCalculateApi.Futan.Models
{
    public class PtHokenInfModel
    {
        public PtHokenInf PtHokenInf { get; }
        public HokenMst HokenMst { get; }

        private readonly int _hpPrefNo;

        public PtHokenInfModel(PtHokenInf ptHokenInf, HokenMst hokenMst, int hpPrefNo)
        {
            PtHokenInf = ptHokenInf;
            HokenMst = hokenMst;
            _hpPrefNo = hpPrefNo;
        }

        /// <summary>
        /// 保険ID
        ///  患者別に保険情報を識別するための固有の番号
        /// </summary>
        public int HokenId
        {
            get { return PtHokenInf.HokenId; }
        }

        /// <summary>
        /// 保険番号
        ///  保険マスタに登録された保険番号
        /// </summary>
        public int HokenNo
        {
            get { return PtHokenInf.HokenNo; }
        }

        /// <summary>
        /// 保険番号枝番
        ///  保険マスタに登録された保険番号枝番
        /// </summary>
        public int HokenEdaNo
        {
            get { return PtHokenInf.HokenEdaNo; }
        }

        /// <summary>
        /// 保険者番号
        /// </summary>
        public string HokensyaNo
        {
            get { return PtHokenInf.HokensyaNo ?? string.Empty; }
        }

        /// <summary>
        /// 国保組合
        /// </summary>
        public bool IsKokKumiai
        {
            get
            {
                if (PtHokenInf.HokensyaNo == null)
                {
                    return false;
                }

                return
                    PtHokenInf.HokenKbn == HokenKbn.Kokho &&
                    PtHokenInf.HokensyaNo.Substring(PtHokenInf.HokensyaNo.Length - 4, 1) == "3";
            }
        }

        /// <summary>
        /// 県内国保
        /// </summary>
        public bool IsKokPrefIn
        {
            get
            {
                if (PtHokenInf.HokensyaNo == null)
                {
                    return false;
                }

                return
                    PtHokenInf.HokenKbn == HokenKbn.Kokho &&
                    PtHokenInf.HokensyaNo.Substring(PtHokenInf.HokensyaNo.Length - 6, 2) == _hpPrefNo.ToString();
            }
        }

        /// <summary>
        /// 県外国保
        /// </summary>
        public bool IsKokPrefOut
        {
            get
            {
                if (PtHokenInf.HokensyaNo == null)
                {
                    return false;
                }

                return
                    PtHokenInf.HokenKbn == HokenKbn.Kokho &&
                    PtHokenInf.HokensyaNo.Substring(PtHokenInf.HokensyaNo.Length - 6, 2) != _hpPrefNo.ToString();
            }
        }

        /// <summary>
        /// 本人家族区分
        ///  1:本人
        ///  2:家族
        /// </summary>
        public int HonkeKbn
        {
            get { return PtHokenInf.HonkeKbn; }
        }

        /// <summary>
        /// 保険者番号の桁数
        ///  0桁: [0]自費保険
        ///  4桁: [01]社保
        ///  6桁: [100]国保
        ///  8桁: 保険者番号の前2桁
        ///   [67]退職国保
        ///   [39]後期高齢
        ///   [01]協会健保、[02]船員、[03,04]日雇、
        ///   [06]組合健保、[07]自衛官、
        ///   [31..34]共済組合
        ///    [31]国家公務員共済組合
        ///    [32]地方公務員等共済組合
        ///    [33]警察共済組合
        ///    [34]公立学校共済組合
        ///　    日本私立学校振興・共済事業団
        ///   [63,72..75]特定共済組合
        /// </summary>
        public string Houbetu
        {
            get { return PtHokenInf.Houbetu ?? string.Empty; }
        }

        /// <summary>
        /// 高額療養費区分
        ///  70歳以上
        ///   0:一般
        ///   3:上位(～2018/07)
        ///   4:低所Ⅱ
        ///   5:低所Ⅰ
        ///   6:特定収入(～2008/12)
        ///   26:現役Ⅲ
        ///   27:現役Ⅱ
        ///   28:現役Ⅰ
        ///  70歳未満
        ///   0:限度額認定証なし
        ///   17:上位[A] (～2014/12)
        ///   18:一般[B] (～2014/12)
        ///   19:低所[C] (～2014/12)
        ///   26:区ア／標準報酬月額83万円以上
        ///   27:区イ／標準報酬月額53..79万円
        ///   28:区ウ／標準報酬月額28..50万円
        ///   29:区エ／標準報酬月額26万円以下
        ///   30:区オ／低所得者
        /// </summary>
        public int KogakuKbn
        {
            get { return PtHokenInf.KogakuKbn; }
        }

        /// <summary>
        /// 高額療養費処理区分
        ///  1:高額委任払い
        ///  2:適用区分一般
        /// </summary>
        public int KogakuType
        {
            get { return PtHokenInf.KogakuType; }
        }

        /// <summary>
        /// 限度額特例対象年月１
        ///  yyyymm
        /// </summary>
        public int TokureiYm1
        {
            get { return PtHokenInf.TokureiYm1; }
        }

        /// <summary>
        /// 限度額特例対象年月２
        ///  yyyymm
        /// </summary>
        public int TokureiYm2
        {
            get { return PtHokenInf.TokureiYm2; }
        }

        /// <summary>
        /// 多数回該当適用開始年月
        ///  yyyymm
        /// </summary>
        public int TasukaiYm
        {
            get { return PtHokenInf.TasukaiYm; }
        }

        /// <summary>
        /// 職務上区分
        ///  1:職務上
        ///  2:下船後３月以内 
        ///  3:通勤災害
        /// </summary>
        public int SyokumuKbn
        {
            get { return PtHokenInf.SyokumuKbn; }
        }

        /// <summary>
        /// 国保減免区分
        ///  1:減額 
        ///  2:免除 
        ///  3:支払猶予
        /// </summary>
        public int GenmenKbn
        {
            get { return PtHokenInf.GenmenKbn; }
        }

        /// <summary>
        /// 国保減免割合
        /// </summary>
        public int GenmenRate
        {
            get { return PtHokenInf.GenmenRate; }
        }

        /// <summary>
        /// 国保減免金額
        /// </summary>
        public int GenmenGaku
        {
            get { return PtHokenInf.GenmenGaku; }
        }

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
        public int HokenSbtKbn
        {
            get { return HokenMst.HokenSbtKbn; }
        }

        /// <summary>
        /// 点数単価
        ///  点数1点あたりの単価を円で表す
        /// </summary>
        public int EnTen
        {
            get { return HokenMst.EnTen; }
        }

        /// <summary>
        /// 負担区分  
        ///  0:負担なし 
        ///  1:負担あり          
        /// </summary>
        public int FutanKbn
        {
            get { return HokenMst.FutanKbn; }
        }

        /// <summary>
        /// 回負担割合            
        /// </summary>
        public int FutanRate
        {
            get { return HokenMst.FutanRate; }
        }

        /// <summary>
        /// レセプト記載   
        ///  0:記載あり
        ///  1:上限未満記載なし
        ///  2:上限以下記載なし
        ///  3:記載なし
        /// </summary>
        public int ReceKisai
        {
            get { return HokenMst.ReceKisai; }
        }
    }
}
