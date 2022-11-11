using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
	/// <summary>
	/// 保険マスタ
	///     ※暫定版（負担金計算作成時に詳細を詰める）
	/// </summary>
	[Table(name: "HOKEN_MST")]
    public class HokenMst : EmrCloneable<HokenMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column(name: "HP_ID", Order = 1)]
        public int HpId { get; set; }

		/// <summary>
		/// 都道府県番号
		/// </summary>
		[Key]
        [Column(name: "PREF_NO", Order = 2)]
        public int PrefNo { get; set; }

		/// <summary>
		/// 保険番号
		/// </summary>
		[Key]
        [Column(name: "HOKEN_NO", Order = 3)]
        public int HokenNo { get; set; }

		/// <summary>
		/// 保険番号枝番
		/// </summary>
		[Key]
        [Column(name: "HOKEN_EDA_NO", Order = 4)]
        public int HokenEdaNo { get; set; }

		/// <summary>
		/// 適用開始日
		/// </summary>
		[Key]
        [Column(name: "START_DATE", Order = 5)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

		/// <summary>
		/// 適用終了日
		/// </summary>
		[Column(name: "END_DATE")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

		/// <summary>
		/// 並び順
		/// </summary>
		[Column(name: "SORT_NO")]
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
		[Column(name: "HOKEN_SBT_KBN")]
        public int HokenSbtKbn { get; set; }

        /// <summary>
        /// 公費主補区分
        /// 0:主公費 1:補助公費 2:主補公費
        /// </summary>
        [Column(name: "HOKEN_KOHI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenKohiKbn { get; set; }

        /// <summary>
        /// 法別番号
        /// </summary>
        [Column(name: "HOUBETU")]
        [MaxLength(3)]
        public string Houbetu { get; set; } = string.Empty;

		/// <summary>
		/// 制度名
		///		例) 難病医療		
		/// </summary>
		[Column(name: "HOKEN_NAME")]
        [MaxLength(100)]
        public string HokenName { get; set; } = string.Empty;

		/// <summary>
		/// 制度略称
		///		例) 難病2500		
		/// </summary>
		[Column(name: "HOKEN_SNAME")]
        [MaxLength(20)]
        public string HokenSname { get; set; } = string.Empty;

		/// <summary>
		/// 制度略号
		///		例) 難病		
		/// </summary>
		[Column(name: "HOKEN_NAME_CD")]
        [MaxLength(5)]
        public string HokenNameCd { get; set; } = string.Empty;


		/// <summary>
		/// 検証番号チェック区分
		///		0:チェックしない 
		///		1:チェックする
		/// </summary>
		[Column(name: "CHECK_DIGIT")]
        [CustomAttribute.DefaultValue(1)]
        public int CheckDigit { get; set; }

		/// <summary>
		/// 受給者検証番号チェック区分
		///		0:チェックしない 
		///		1:チェックする
		/// </summary>
		[Column(name: "JYUKYU_CHECK_DIGIT")]
        [CustomAttribute.DefaultValue(1)]
        public int JyukyuCheckDigit { get; set; }

		/// <summary>
		/// 負担者番号入力チェック
		/// </summary>
		[Column(name: "IS_FUTANSYA_NO_CHECK")]
        [CustomAttribute.DefaultValue(0)]
        public int IsFutansyaNoCheck { get; set; }

		/// <summary>
		/// 受給者番号入力チェック
		/// </summary>
		[Column(name: "IS_JYUKYUSYA_NO_CHECK")]
        [CustomAttribute.DefaultValue(0)]
        public int IsJyukyusyaNoCheck { get; set; }

		/// <summary>
		/// 特殊番号入力チェック
		/// </summary>
		[Column(name: "IS_TOKUSYU_NO_CHECK")]
        [CustomAttribute.DefaultValue(0)]
        public int IsTokusyuNoCheck { get; set; }

		/// <summary>
		/// 上限管理区分
		///		0:なし 
		///		1:あり
		/// </summary>
		[Column(name: "IS_LIMIT_LIST")]
        [CustomAttribute.DefaultValue(0)]
        public int IsLimitList { get; set; }

		/// <summary>
		/// 上限管理総額表示区分
		///		0:なし 
		///		1:あり
		/// </summary>
		[Column(name: "IS_LIMIT_LIST_SUM")]
        [CustomAttribute.DefaultValue(0)]
        public int IsLimitListSum { get; set; }

		/// <summary>
		/// 他県公費有効フラグ
		///		0:無効 
		///		1:有効
		/// </summary>
		[Column(name: "IS_OTHER_PREF_VALID")]
        [CustomAttribute.DefaultValue(0)]
        public int IsOtherPrefValid { get; set; }

		/// <summary>
		/// 年齢条件開始
		/// </summary>
		[Column(name: "AGE_START")]
        [CustomAttribute.DefaultValue(0)]
        public int AgeStart { get; set; }

		/// <summary>
		/// 年齢条件終了
		/// </summary>
		[Column(name: "AGE_END")]
        [CustomAttribute.DefaultValue(999)]
        public int AgeEnd { get; set; }

		/// <summary>
		/// 点数単価
		///		点数1点あたりの単価を円で表す
		/// </summary>
		[Column(name: "EN_TEN")]
        [CustomAttribute.DefaultValue(10)]
        public int EnTen { get; set; }

		/// <summary>
		/// 請求年月フラグ			
		/// </summary>
		[Column(name: "SEIKYU_YM")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuYm { get; set; }

		/// <summary>
		/// レセプト特殊処理区分				
		/// </summary>
		[Column(name: "RECE_SP_KBN")]
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
		[Column(name: "RECE_SEIKYU_KBN")]
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
		[Column(name: "RECE_FUTAN_ROUND")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceFutanRound { get; set; }

        /// <summary>
        /// レセプト記載   
        ///  0:記載あり
        ///  1:上限未満記載なし
        ///  2:上限以下記載なし
        ///  3:記載なし
        /// </summary>
        [Column(name: "RECE_KISAI")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceKisai { get; set; }

        /// <summary>
        /// レセプト記載２
        ///  0:一部負担相当額なし記載なし
        ///  1:一部負担相当額なし記載あり
        /// </summary>
        [Column(name: "RECE_KISAI2")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceKisai2 { get; set; }

        /// <summary>
        /// レセプト0円記載			
        ///		0:記載なし
        ///		1:記載あり
        /// </summary>
        [Column(name: "RECE_ZERO_KISAI")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceZeroKisai { get; set; }

        /// <summary>
        /// レセプト負担金記載
        ///		0:記載あり
        ///		1:記載なし
        /// </summary>
        [Column(name: "RECE_FUTAN_HIDE")]
        [CustomAttribute.DefaultValue(0)]
        public int ReceFutanHide { get; set; }

        /// <summary>
        /// レセプト負担区分			
        ///		0:窓口負担に準じる
        ///		1:xxx
        ///		2:xxx
        ///		3:xxx
        /// </summary>
        [Column(name: "RECE_FUTAN_KBN")]
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
        [Column(name: "RECE_TEN_KISAI")]
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
        [Column(name: "KOGAKU_TOTAL_KBN")]
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
		[Column(name: "KOGAKU_TOTAL_ALL")]
		[CustomAttribute.DefaultValue(0)]
		public int KogakuTotalAll { get; set; }

		/// <summary>
		/// 計算特殊処理区分			
		/// </summary>
		[Column(name: "CALC_SP_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int CalcSpKbn { get; set; }

        /// <summary>
        /// 高額療養費適用区分			
        /// </summary>
        [Column(name: "KOGAKU_TEKIYO")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuTekiyo { get; set; }

        /// <summary>
        /// 高額療養費優先区分			
        /// </summary>
        [Column(name: "FUTAN_YUSEN")]
        [CustomAttribute.DefaultValue(0)]
        public int FutanYusen { get; set; }

		/// <summary>
		/// 上限区分	
		///		0:医療期間単位 
		///		1:レセプト単位								
		/// </summary>
		[Column(name: "LIMIT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int LimitKbn { get; set; }

		/// <summary>
		/// 回数集計区分								
		/// </summary>
		[Column(name: "COUNT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int CountKbn { get; set; }

		/// <summary>
		/// 負担区分		
		///		0:負担なし 
		///		1:負担あり										
		/// </summary>
		[Column(name: "FUTAN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int FutanKbn { get; set; }

		/// <summary>
		/// 回負担割合												
		/// </summary>
		[Column(name: "FUTAN_RATE")]
        [CustomAttribute.DefaultValue(0)]
        public int FutanRate { get; set; }

		/// <summary>
		/// 回固定額			
		/// </summary>
		[Column(name: "KAI_FUTANGAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int KaiFutangaku { get; set; }

		/// <summary>
		/// 回上限額			
		/// </summary>
		[Column(name: "KAI_LIMIT_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int KaiLimitFutan { get; set; }

		/// <summary>
		/// 日上限額			
		/// </summary>
		[Column(name: "DAY_LIMIT_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int DayLimitFutan { get; set; }

		/// <summary>
		/// 日上限回数			
		/// </summary>
		[Column(name: "DAY_LIMIT_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int DayLimitCount { get; set; }

		/// <summary>
		/// 月上限額			
		/// </summary>
		[Column(name: "MONTH_LIMIT_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int MonthLimitFutan { get; set; }

        /// <summary>
        /// 月上限額（特殊計算用）
        /// </summary>
        [Column(name: "MONTH_SP_LIMIT")]
        [CustomAttribute.DefaultValue(0)]
        public int MonthSpLimit { get; set; }

        /// <summary>
        /// 月上限回数			
        /// </summary>
        [Column(name: "MONTH_LIMIT_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int MonthLimitCount { get; set; }

		/// <summary>
		/// 作成日時	
		/// </summary>
		[Column("CREATE_DATE")]
		[CustomAttribute.DefaultValueSql("current_timestamp")]
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// 作成者		
		/// </summary>
		[Column(name: "CREATE_ID")]
		[CustomAttribute.DefaultValue(0)]
		public int CreateId { get; set; }

		/// <summary>
		/// 作成端末			
		/// </summary>
		[Column(name: "CREATE_MACHINE")]
		[MaxLength(60)]
		public string? CreateMachine { get; set; } = string.Empty;

		/// <summary>
		/// 更新日時			
		/// </summary>
		[Column("UPDATE_DATE")]
		public DateTime UpdateDate { get; set; }

		/// <summary>
		/// 更新者			
		/// </summary>
		[Column(name: "UPDATE_ID")]
		[CustomAttribute.DefaultValue(0)]
		public int UpdateId { get; set; }

		/// <summary>
		/// 更新端末			
		/// </summary>
		[Column(name: "UPDATE_MACHINE")]
		[MaxLength(60)]
		public string? UpdateMachine { get; set; }  = string.Empty;

		/// <summary>
		/// レセプト記載（国保用）   
		///  0:記載あり
		///  1:上限未満記載なし
		///  2:上限以下記載なし
		///  3:記載なし
		/// </summary>
		[Column(name: "RECE_KISAI_KOKHO")]
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
		[Column(name: "KOGAKU_HAIRYO_KBN")]
		[CustomAttribute.DefaultValue(0)]
		public int KogakuHairyoKbn { get; set; }
	}
}