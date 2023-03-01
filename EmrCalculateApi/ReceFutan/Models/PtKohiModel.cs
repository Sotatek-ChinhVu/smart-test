using Entity.Tenant;
using Helper.Constants;

namespace EmrCalculateApi.ReceFutan.Models
{
    public class PtKohiModel
    {
        public PtKohi PtKohi { get; }
        public HokenMst HokenMst { get; }
        public bool ExceptHokensya { get; }

        private int hokenKbn;

        public PtKohiModel(PtKohi ptKohi, HokenMst hokenMst, bool exceptHokensya, int hokenKbn)
        {
            PtKohi = ptKohi;
            HokenMst = hokenMst;
            ExceptHokensya = exceptHokensya;
            this.hokenKbn = hokenKbn;
        }

        /// <summary>
        /// 患者ID
        ///  患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return PtKohi.PtId; }
        }

        /// <summary>
        /// 保険ID
        ///  患者別に保険情報を識別するための固有の番号
        /// </summary>
        public int HokenId
        {
            get { return PtKohi.HokenId; }
        }

        /// <summary>
        /// 都道府県番号
        ///  保険マスタの都道府県番号
        /// </summary>
        public int PrefNo
        {
            get { return PtKohi.PrefNo; }
        }

        /// <summary>
        /// 保険番号
        ///  保険マスタに登録された保険番号
        /// </summary>
        public int HokenNo
        {
            get { return PtKohi.HokenNo; }
        }

        /// <summary>
        /// 保険番号枝番
        ///  保険マスタに登録された保険番号枝番
        /// </summary>
        public int HokenEdaNo
        {
            get { return PtKohi.HokenEdaNo; }
        }

        /// <summary>
        /// 負担者番号
        /// </summary>
        public string FutansyaNo
        {
            get { return PtKohi.FutansyaNo ?? string.Empty; }
        }

        /// <summary>
        /// 受給者番号
        /// </summary>
        public string JyukyusyaNo
        {
            get { return PtKohi.JyukyusyaNo ?? string.Empty; }
        }

        /// <summary>
        /// 特殊受給者番号
        /// </summary>
        public string TokusyuNo
        {
            get { return PtKohi.TokusyuNo ?? string.Empty; }
        }

        /// <summary>
        /// 法別番号
        /// </summary>
        public string Houbetu
        {
            get { return HokenMst.Houbetu; }
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
        /// レセプト特殊処理区分    
        /// </summary>
        public int ReceSpKbn
        {
            get { return HokenMst.ReceSpKbn; }
        }

        /// <summary>
        /// レセプト請求区分
        ///     0:社(併用)国(併用)
        ///     1:社(併用)国(単独)
        ///     2:社(単独)国(併用)
        ///     3:社(単独)国(単独)
        ///     4:社(併用)国(単独/組合併用)
        ///     5:社(単独)国(併用/組合単独)
        /// </summary>
        public int ReceSeikyuKbn
        {
            get { return HokenMst.ReceSeikyuKbn; }
        }

        /// <summary>
        /// レセプト負担金額
        ///  0: 社・国(1円単位)
        ///  1: 社・国(10円単位[窓口])
        ///  2: 社・国(10円単位)
        ///  3: 社(1円単位)・国(10円単位[窓口])
        ///  4: 社(1円単位)・国(10円単位)
        ///  5: 社(10円単位[窓口])・国(1円単位)
        ///  6: 社(10円単位[窓口])・国(10円単位)
        ///  7: 社(10円単位)・国(1円単位)
        ///  8: 社(10円単位)・国(10円単位[窓口])
        /// </summary>
        public int ReceFutanRound
        {
            get { return HokenMst.ReceFutanRound; }
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
            get { return hokenKbn == HokenKbn.Kokho ? HokenMst.ReceKisaiKokho : HokenMst.ReceKisai; }
        }

        /// <summary>
        /// レセプト記載２
        ///  0:一部負担相当額なし記載なし
        ///  1:一部負担相当額なし記載あり
        /// </summary>
        public int ReceKisai2
        {
            get { return HokenMst.ReceKisai2; }
        }

        /// <summary>
        /// レセプト0円記載   
        ///  0:記載なし
        ///  1:記載あり
        /// </summary>
        public int ReceZeroKisai
        {
            get { return HokenMst.ReceZeroKisai; }
        }

        /// <summary>
        /// レセプト負担金記載
        ///		0:記載あり
        ///		1:記載なし
        /// </summary>
        public bool ReceFutanHide
        {
            get { return HokenMst.ReceFutanHide == 1; }
        }

        /// <summary>
        /// レセプト点数記載			
        ///     0: 社保(負担対象に準じる)   国保(負担対象に準じる)
        ///     1: 社保(公１点数を除く)     国保(負担対象に準じる)
        ///     2: 社保(負担対象に準じる)   国保(公１点数を除く)
        ///     3: 社保(公１点数を除く)     国保(公１点数を除く)
        ///     ※異点数の場合のみ総点数－公１点数を記載する
        /// </summary>
        public int ReceTenKisai
        {
            get { return HokenMst.ReceTenKisai; }
        }

        /// <summary>
        /// 制度略号
        /// </summary>
        public string HokenNameCd
        {
            get { return HokenMst.HokenNameCd; }
        }

        /// <summary>
        /// 高額療養費合算対象区分   
        ///     0: 社保(合算対象)   国保(合算対象)
        ///     1: 社保(合算対象)   国保(合算対象外)
        ///     2: 社保(合算対象外) 国保(合算対象)
        ///     3: 社保(合算対象外) 国保(合算対象外)
        ///     
        /// 公費併用と保険単独の療養が併せて行われている場合、
        /// 70歳未満では一部負担金等がそれぞれ21,000円以上で公費の費用徴収額があることが合算対象となる条件
        /// 上記条件を満たした場合でも合算対象としない場合に設定する
        /// </summary>
        public int KogakuTotalKbn
        {
            get { return HokenMst.KogakuTotalKbn; }
        }
    }

}
