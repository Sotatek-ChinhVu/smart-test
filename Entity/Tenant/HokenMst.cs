using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 保険マスタ
    ///     ※暫定版（負担金計算作成時に詳細を詰める）
    /// </summary>
    [Table(name: "hoken_mst")]
    public class HokenMst : EmrCloneable<HokenMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column(name: "hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 都道府県番号
        /// </summary>
        
        [Column(name: "pref_no", Order = 2)]
        public int PrefNo { get; set; }

        /// <summary>
        /// 保険番号
        /// </summary>
        
        [Column(name: "hoken_no", Order = 3)]
        public int HokenNo { get; set; }

        /// <summary>
        /// 保険番号枝番
        /// </summary>
        
        [Column(name: "hoken_eda_no", Order = 4)]
        public int HokenEdaNo { get; set; }

        /// <summary>
        /// 適用開始日
        /// </summary>
        
        [Column(name: "start_date", Order = 5)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        /// </summary>
        [Column(name: "end_date")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column(name: "sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// 保険種別区分
        ///		0:保険なし 
        ///		1:主保険   
        ///		2:マル長   
        ///		3:労災  
        ///		4:自賠
        ///		5:生活保護 
        ///		6:分点公費
        ///		7:一般公費  
        ///		8:自費
        /// </summary>
        [Column(name: "hoken_sbt_kbn")]
        public int HokenSbtKbn { get; set; }

        /// <summary>
        /// 公費主補区分
        /// 0:主公費 1:補助公費 2:主補公費
        /// </summary>
        [Column(name: "hoken_kohi_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenKohiKbn { get; set; }

        /// <summary>
        /// 法別番号
        /// </summary>
        [Column(name: "houbetu")]
        [MaxLength(3)]
        public string? Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 制度名
        ///		例) 難病医療		
        /// </summary>
        [Column(name: "hoken_name")]
        [MaxLength(100)]
        public string? HokenName { get; set; } = string.Empty;

        /// <summary>
        /// 制度略称
        ///		例) 難病2500		
        /// </summary>
        [Column(name: "hoken_sname")]
        [MaxLength(20)]
        public string? HokenSname { get; set; } = string.Empty;

        /// <summary>
        /// 制度略号
        ///		例) 難病		
        /// </summary>
        [Column(name: "hoken_name_cd")]
        [MaxLength(5)]
        public string? HokenNameCd { get; set; } = string.Empty;


        /// <summary>
        /// 検証番号チェック区分
        ///		0:チェックしない 
        ///		1:チェックする
        /// </summary>
        [Column(name: "check_digit")]
        [CustomAttribute.DefaultValue(1)]
        public int CheckDigit { get; set; }

        /// <summary>
        /// 受給者検証番号チェック区分
        ///		0:チェックしない 
        ///		1:チェックする
        /// </summary>
        [Column(name: "jyukyu_check_digit")]
        [CustomAttribute.DefaultValue(1)]
        public int JyukyuCheckDigit { get; set; }

        /// <summary>
        /// 負担者番号入力チェック
        /// </summary>
        [Column(name: "is_futansya_no_check")]
        [CustomAttribute.DefaultValue(0)]
        public int IsFutansyaNoCheck { get; set; }

        /// <summary>
        /// 受給者番号入力チェック
        /// </summary>
        [Column(name: "is_jyukyusya_no_check")]
        [CustomAttribute.DefaultValue(0)]
        public int IsJyukyusyaNoCheck { get; set; }

        /// <summary>
        /// 特殊番号入力チェック
        /// </summary>
        [Column(name: "is_tokusyu_no_check")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTokusyuNoCheck { get; set; }

        /// <summary>
        /// 上限管理区分
        ///		0:なし 
        ///		1:あり
        /// </summary>
        [Column(name: "is_limit_list")]
        [CustomAttribute.DefaultValue(0)]
        public int IsLimitList { get; set; }

        /// <summary>
        /// 上限管理総額表示区分
        ///		0:なし 
        ///		1:あり
        /// </summary>
        [Column(name: "is_limit_list_sum")]
        [CustomAttribute.DefaultValue(0)]
        public int IsLimitListSum { get; set; }

        /// <summary>
        /// 他県公費有効フラグ
        ///		0:無効 
        ///		1:有効
        /// </summary>
        [Column(name: "is_other_pref_valid")]
        [CustomAttribute.DefaultValue(0)]
        public int IsOtherPrefValid { get; set; }

        /// <summary>
        /// 年齢条件開始
        /// </summary>
        [Column(name: "age_start")]
        [CustomAttribute.DefaultValue(0)]
        public int AgeStart { get; set; }

        /// <summary>
        /// 年齢条件終了
        /// </summary>
        [Column(name: "age_end")]
        [CustomAttribute.DefaultValue(999)]
        public int AgeEnd { get; set; }

        /// <summary>
        /// 点数単価
        ///		点数1点あたりの単価を円で表す
        /// </summary>
        [Column(name: "en_ten")]
        [CustomAttribute.DefaultValue(10)]
        public double EnTen { get; set; }

        /// <summary>
        /// 請求年月フラグ			
        /// </summary>
        [Column(name: "seikyu_ym")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuYm { get; set; }

        /// <summary>
        /// レセプト特殊処理区分				
        /// </summary>
        [Column(name: "rece_sp_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceSpKbn { get; set; }

        /// <summary>
        /// レセプト請求区分
        ///     0:社(併用)国(併用)
        ///     1:社(併用)国(単独)
        ///     2:社(単独)国(併用)
        ///     3:社(単独)国(単独)
        ///     4:社(併用)国(単独/組合併用)
        ///     5:社(単独)国(併用/組合単独)
        /// </summary>
		[Column(name: "rece_seikyu_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceSeikyuKbn { get; set; }

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
        [Column(name: "rece_futan_round")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceFutanRound { get; set; }

        /// <summary>
        /// レセプト記載   
        ///  0:記載あり
        ///  1:上限未満記載なし
        ///  2:上限以下記載なし
        ///  3:記載なし
        /// </summary>
        [Column(name: "rece_kisai")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceKisai { get; set; }

        /// <summary>
        /// レセプト記載２
        ///  0:一部負担相当額なし記載なし
        ///  1:一部負担相当額なし記載あり
        /// </summary>
        [Column(name: "rece_kisai2")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceKisai2 { get; set; }

        /// <summary>
        /// レセプト0円記載			
        ///		0:記載なし
        ///		1:記載あり
        /// </summary>
        [Column(name: "rece_zero_kisai")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceZeroKisai { get; set; }

        /// <summary>
        /// レセプト負担金記載
        ///		0:記載あり
        ///		1:記載なし
        /// </summary>
        [Column(name: "rece_futan_hide")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceFutanHide { get; set; }

        /// <summary>
        /// レセプト負担区分			
        ///		0:窓口負担に準じる
        ///		1:xxx
        ///		2:xxx
        ///		3:xxx
        /// </summary>
        [Column(name: "rece_futan_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceFutanKbn { get; set; }

        /// <summary>
        /// レセプト点数記載			
        ///     0: 社保(負担対象に準じる)   国保(負担対象に準じる)
        ///     1: 社保(公１点数を除く)     国保(負担対象に準じる)
        ///     2: 社保(負担対象に準じる)   国保(公１点数を除く)
        ///     3: 社保(公１点数を除く)     国保(公１点数を除く)
        ///     ※異点数の場合のみ総点数－公１点数を記載する
        /// </summary>
        [Column(name: "rece_ten_kisai")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceTenKisai { get; set; }

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
        [Column(name: "kogaku_total_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuTotalKbn { get; set; }

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
        [Column(name: "kogaku_total_all")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuTotalAll { get; set; }

        /// <summary>
        /// 計算特殊処理区分			
        /// </summary>
        [Column(name: "calc_sp_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int CalcSpKbn { get; set; }

        /// <summary>
        /// 高額療養費の合算時に公費負担を除く
        ///     0: 公費負担を含む
        ///     1: 公費負担を除く
        ///     
        /// 一般公費が対象（主補公費の場合に使用を想定）
        /// 分点公費や生活保護は、公費負担を除くようになっている
        /// </summary>
        [Column(name: "kogaku_total_exc_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuTotalExcFutan { get; set; }

        /// <summary>
        /// 高額療養費適用区分			
        /// </summary>
        [Column(name: "kogaku_tekiyo")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuTekiyo { get; set; }

        /// <summary>
        /// 高額療養費優先区分			
        /// </summary>
        [Column(name: "futan_yusen")]
        [CustomAttribute.DefaultValue(0)]
        public int FutanYusen { get; set; }

        /// <summary>
        /// 上限区分	
        ///		0:医療期間単位 
        ///		1:レセプト単位								
        /// </summary>
        [Column(name: "limit_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int LimitKbn { get; set; }

        /// <summary>
        /// 回数集計区分								
        /// </summary>
        [Column(name: "count_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int CountKbn { get; set; }

        /// <summary>
        /// 負担区分		
        ///		0:負担なし 
        ///		1:負担あり										
        /// </summary>
        [Column(name: "futan_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int FutanKbn { get; set; }

        /// <summary>
        /// 回負担割合												
        /// </summary>
        [Column(name: "futan_rate")]
        [CustomAttribute.DefaultValue(0)]
        public int FutanRate { get; set; }

        /// <summary>
        /// 回固定額			
        /// </summary>
        [Column(name: "kai_futangaku")]
        [CustomAttribute.DefaultValue(0)]
        public int KaiFutangaku { get; set; }

        /// <summary>
        /// 回上限額			
        /// </summary>
        [Column(name: "kai_limit_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int KaiLimitFutan { get; set; }

        /// <summary>
        /// 日上限額			
        /// </summary>
        [Column(name: "day_limit_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int DayLimitFutan { get; set; }

        /// <summary>
        /// 日上限回数			
        /// </summary>
        [Column(name: "day_limit_count")]
        [CustomAttribute.DefaultValue(0)]
        public int DayLimitCount { get; set; }

        /// <summary>
        /// 月上限額			
        /// </summary>
        [Column(name: "month_limit_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int MonthLimitFutan { get; set; }

        /// <summary>
        /// 月上限額（特殊計算用）
        /// </summary>
        [Column(name: "month_sp_limit")]
        [CustomAttribute.DefaultValue(0)]
        public int MonthSpLimit { get; set; }

        /// <summary>
        /// 月上限回数			
        /// </summary>
        [Column(name: "month_limit_count")]
        [CustomAttribute.DefaultValue(0)]
        public int MonthLimitCount { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;

        /// <summary>
        /// レセプト記載（国保用）   
        ///  0:記載あり
        ///  1:上限未満記載なし
        ///  2:上限以下記載なし
        ///  3:記載なし
        /// </summary>
        [Column(name: "rece_kisai_kokho")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceKisaiKokho { get; set; }

        /// <summary>
        /// 高額療養費 配慮措置適用区分
        ///     0: 県内(配慮措置なし)・県外(配慮措置なし)
        ///     1: 県内(配慮措置なし)・県外(配慮措置あり)
        ///     2: 県内(配慮措置あり)・県外(配慮措置なし)
        ///     3: 県内(配慮措置あり)・県外(配慮措置あり)
        /// </summary>
        //[NotMapped]
        [Column(name: "kogaku_hairyo_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuHairyoKbn { get; set; }
    }
}