using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class PtKohiModel
    {
        public PtKohi PtKohi { get; }

        public HokenMst HokenMst { get; }

        public bool ExceptHokensya { get; } = false;

        /// <summary>
        /// 公費優先順位(KOHI_PRIORITY.PRIORITY_NO)
        /// </summary>
        public string PriorityNo { get; } = "99999";

        public PtKohiModel(PtKohi ptKohi, HokenMst hokenMst, bool exceptHokensya, string priorityNo)
        {
            PtKohi = ptKohi;
            HokenMst = hokenMst;
            ExceptHokensya = exceptHokensya;
            PriorityNo = priorityNo;
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
        /// 適用開始日
        ///  yyyymmdd
        /// </summary>
        public int StartDate
        {
            get { return PtKohi.StartDate; }
        }

        /// <summary>
        /// 適用終了日
        ///  yyyymmdd
        /// </summary>
        public int EndDate
        {
            get { return PtKohi.EndDate; }
        }

        /// <summary>
        /// 負担率
        ///  yyyymmdd
        /// </summary>
        public int Rate
        {
            get { return PtKohi.Rate; }
        }

        /// <summary>
        /// 一部負担限度額
        ///  yyyymmdd
        /// </summary>
        public int GendoGaku
        {
            get { return PtKohi.GendoGaku; }
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
        /// 上限管理区分
        ///  0:なし 
        ///  1:あり
        /// </summary>
        public int IsLimitList
        {
            get { return HokenMst.IsLimitList; }
        }

        /// <summary>
        /// 上限管理総額表示区分
        ///  0:なし 
        ///  1:あり
        /// </summary>
        public int IsLimitListSum
        {
            get { return HokenMst.IsLimitListSum; }
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

        /// <summary>
        /// 高額療養費の合算対象に21,000円未満を含めるかどうか
        ///     0: 社保(含めない) 国保(含めない)
        ///     1: 社保(含める)   国保(含めない)
        ///     2: 社保(含めない) 国保(含める)
        ///     3: 社保(含める)   国保(含める)
        ///     
        /// 公費併用と保険単独の療養が併せて行われている場合、
        /// 70歳未満では一部負担金等がそれぞれ21,000円以上で公費の費用徴収額があることが合算対象となる条件
        /// 上記条件を満たしていない場合でも合算対象とする場合に設定する
        /// </summary>
        public int KogakuTotalAll
        {
            get { return HokenMst.KogakuTotalAll; }
        }

        /// <summary>
        /// 高額療養費の合算時に公費負担を除く
        ///     0: 公費負担を含む
        ///     1: 公費負担を除く
		///     
        /// 一般公費が対象（主補公費の場合に使用を想定）
		/// 分点公費や生活保護は、公費負担を除くようになっている
        /// </summary>
        public int KogakuTotalExcFutan
        {
            get { return HokenMst.KogakuTotalExcFutan; }
        }

        /// <summary>
        /// 計算特殊処理区分   
        /// </summary>
        public int CalcSpKbn
        {
            get { return HokenMst.CalcSpKbn; }
        }

        /// <summary>
        /// 高額療養費適用区分   
        /// </summary>
        public int KogakuTekiyo
        {
            get { return HokenMst.KogakuTekiyo; }
        }

        /// <summary>
        /// 高額療養費優先区分   
        /// </summary>
        public int FutanYusen
        {
            get { return HokenMst.FutanYusen; }
        }

        /// <summary>
        /// 上限区分 
        ///  0:医療期間単位 
        ///  1:レセプト単位        
        /// </summary>
        public int LimitKbn
        {
            get { return HokenMst.LimitKbn; }
        }

        /// <summary>
        /// 回数集計区分        
        /// </summary>
        public int CountKbn
        {
            get { return HokenMst.CountKbn; }
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
        /// 回上限額   
        /// </summary>
        public int KaiLimitFutan
        {
            get
            {
                return HokenMst.KaiLimitFutan > 0 && PtKohi.GendoGaku > 0 ? PtKohi.GendoGaku :
                    HokenMst.KaiLimitFutan;
            }
        }

        /// <summary>
        /// 日上限額   
        /// </summary>
        public int DayLimitFutan
        {
            get
            {
                return HokenMst.DayLimitFutan > 0 && PtKohi.GendoGaku > 0 &&
                    HokenMst.KaiLimitFutan == 0 ? PtKohi.GendoGaku :
                    HokenMst.DayLimitFutan;
            }
        }

        /// <summary>
        /// 日上限回数   
        /// </summary>
        public int DayLimitCount
        {
            get { return HokenMst.DayLimitCount; }
        }

        /// <summary>
        /// 月上限額   
        /// </summary>
        public int MonthLimitFutan
        {
            get
            {
                return HokenMst.MonthLimitFutan > 0 && PtKohi.GendoGaku > 0 &&
                    HokenMst.KaiLimitFutan == 0 && HokenMst.DayLimitFutan == 0 ? PtKohi.GendoGaku :
                    HokenMst.MonthLimitFutan;
            }
        }

        /// <summary>
        /// 月上限額（特殊計算用）
        /// </summary>
        public int MonthSpLimit
        {
            get { return HokenMst.MonthSpLimit; }
        }

        /// <summary>
        /// 月上限回数   
        /// </summary>
        public int MonthLimitCount
        {
            get { return HokenMst.MonthLimitCount; }
        }

        /// <summary>
        /// 公費優先順位(優先順位+法別番号)
        /// </summary>
        public string Priority
        {
            get
            {
                return String.Format(
                    "{0}{1}",
                    PriorityNo?.PadLeft(5, '9') ?? "99999", HokenMst.Houbetu?.PadLeft(3, '0') ?? "999"
                );
            }
        }

        /// <summary>
        /// 高額療養費 配慮措置適用区分
        ///     0: 県内(配慮措置なし)・県外(配慮措置なし)
        ///     1: 県内(配慮措置なし)・県外(配慮措置あり)
        ///     2: 県内(配慮措置あり)・県外(配慮措置なし)
        ///     3: 県内(配慮措置あり)・県外(配慮措置あり)
        /// </summary>
        public int KogakuHairyoKbn
        {
            get => HokenMst.KogakuHairyoKbn;
        }
    }
}
