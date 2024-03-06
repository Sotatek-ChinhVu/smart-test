using Entity.Tenant;

namespace Reporting.Receipt.Models
{
    public class CoHokenMstModel
    {
        public HokenMst HokenMst { get; }

        public CoHokenMstModel(HokenMst hokenMst)
        {
            HokenMst = hokenMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return HokenMst.HpId; }
        }

        /// <summary>
        /// 都道府県番号
        /// </summary>
        public int PrefNo
        {
            get { return HokenMst.PrefNo; }
        }

        /// <summary>
        /// 保険番号
        /// </summary>
        public int HokenNo
        {
            get { return HokenMst.HokenNo; }
        }

        /// <summary>
        /// 保険番号枝番
        /// </summary>
        public int HokenEdaNo
        {
            get { return HokenMst.HokenEdaNo; }
        }

        /// <summary>
        /// 適用開始日
        /// </summary>
        public int StartDate
        {
            get { return HokenMst.StartDate; }
        }

        /// <summary>
        /// 適用終了日
        /// </summary>
        public int EndDate
        {
            get { return HokenMst.EndDate; }
        }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo
        {
            get { return HokenMst.SortNo; }
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
        /// 公費主補区分
        /// 0:主公費 1:補助公費 2:主補公費
        /// </summary>
        public int HokenKohiKbn
        {
            get { return HokenMst.HokenKohiKbn; }
        }

        /// <summary>
        /// 法別番号
        /// </summary>
        public string Houbetu
        {
            get { return HokenMst.Houbetu ?? string.Empty; }
        }

        /// <summary>
        /// 制度名
        ///  例) 難病医療  
        /// </summary>
        public string HokenName
        {
            get { return HokenMst.HokenName ?? string.Empty; }
        }

        /// <summary>
        /// 制度略称
        ///  例) 難病2500  
        /// </summary>
        public string HokenSname
        {
            get { return HokenMst.HokenSname ?? string.Empty; }
        }

        /// <summary>
        /// 制度略号
        ///  例) 難病  
        /// </summary>
        public string HokenNameCd
        {
            get { return HokenMst.HokenNameCd ?? string.Empty; }
        }

        /// <summary>
        /// 検証番号チェック区分
        ///  0:チェックしない 
        ///  1:チェックする
        /// </summary>
        public int CheckDigit
        {
            get { return HokenMst.CheckDigit; }
        }

        /// <summary>
        /// 受給者検証番号チェック区分
        ///  0:チェックしない 
        ///  1:チェックする
        /// </summary>
        public int JyukyuCheckDigit
        {
            get { return HokenMst.JyukyuCheckDigit; }
        }

        /// <summary>
        /// 負担者番号入力チェック
        /// </summary>
        public int IsFutansyaNoCheck
        {
            get { return HokenMst.IsFutansyaNoCheck; }
        }

        /// <summary>
        /// 受給者番号入力チェック
        /// </summary>
        public int IsJyukyusyaNoCheck
        {
            get { return HokenMst.IsJyukyusyaNoCheck; }
        }

        /// <summary>
        /// 特殊番号入力チェック
        /// </summary>
        public int IsTokusyuNoCheck
        {
            get { return HokenMst.IsTokusyuNoCheck; }
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
        /// 他県公費有効フラグ
        ///  0:無効 
        ///  1:有効
        /// </summary>
        public int IsOtherPrefValid
        {
            get { return HokenMst.IsOtherPrefValid; }
        }

        /// <summary>
        /// 年齢条件開始
        /// </summary>
        public int AgeStart
        {
            get { return HokenMst.AgeStart; }
        }

        /// <summary>
        /// 年齢条件終了
        /// </summary>
        public int AgeEnd
        {
            get { return HokenMst.AgeEnd; }
        }

        /// <summary>
        /// 点数単価
        ///  点数1点あたりの単価を円で表す
        /// </summary>
        public double EnTen
        {
            get { return HokenMst.EnTen; }
        }

        /// <summary>
        /// 請求年月フラグ   
        /// </summary>
        public int SeikyuYm
        {
            get { return HokenMst.SeikyuYm; }
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
            get { return HokenMst.ReceKisai; }
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
        ///  0:記載あり
        ///  1:記載なし
        /// </summary>
        public int ReceFutanHide
        {
            get { return HokenMst.ReceFutanHide; }
        }

        /// <summary>
        /// レセプト負担区分   
        ///  0:窓口負担に準じる
        ///  1:xxx
        ///  2:xxx
        ///  3:xxx
        /// </summary>
        public int ReceFutanKbn
        {
            get { return HokenMst.ReceFutanKbn; }
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
        /// 回固定額   
        /// </summary>
        public int KaiFutangaku
        {
            get { return HokenMst.KaiFutangaku; }
        }

        /// <summary>
        /// 回上限額   
        /// </summary>
        public int KaiLimitFutan
        {
            get { return HokenMst.KaiLimitFutan; }
        }

        /// <summary>
        /// 日上限額   
        /// </summary>
        public int DayLimitFutan
        {
            get { return HokenMst.DayLimitFutan; }
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
            get { return HokenMst.MonthLimitFutan; }
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
    }
}
