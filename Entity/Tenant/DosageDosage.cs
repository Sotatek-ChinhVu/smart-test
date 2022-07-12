using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M46_DOSAGE_DOSAGE")]
    public class DosageDosage : EmrCloneable<DosageDosage>
    {
        /// <summary>
        /// ＤＯＥＩコード
        /// </summary>
        [Key]
        [Column("DOEI_CD", Order = 1)]
        public string DoeiCd { get; set; } = string.Empty;

        /// <summary>
        /// ＤＯＥＩコード連番
        /// </summary>
        //[Key]
        [Column("DOEI_SEQ_NO", Order = 2)]
        public int DoeiSeqNo { get; set; }

        /// <summary>
        /// 効能効果コード
        /// </summary>
        [Column("KONOKOKA_CD")]
        [MaxLength(7)]
        public string KonokokaCd { get; set; } = string.Empty;

        /// <summary>
        /// 検査条件組合せコード
        /// 
        /// </summary>
        [Column("KENSA_PCD")]
        [MaxLength(7)]
        public string KensaPcd { get; set; } = string.Empty;

        /// <summary>
        /// 年齢条件_以上
        /// Y
        /// </summary>
        [Column("AGE_OVER")]
        public double AgeOver { get; set; }

        /// <summary>
        /// 年齢条件_未満
        /// Y
        /// </summary>
        [Column("AGE_UNDER")]
        public double AgeUnder { get; set; }

        /// <summary>
        /// 年齢条件コード
        /// 1:歳(年齢) 2:月(月齢) 3:週(週齢) 4:日(日齢)
        /// </summary>
        [Column("AGE_CD")]
        [MaxLength(1)]
        public string AgeCd { get; set; } = string.Empty;

        /// <summary>
        /// 体重条件_以上
        /// Y kg
        /// </summary>
        [Column("WEIGHT_OVER")]
        public double WeightOver { get; set; }

        /// <summary>
        /// 体重条件_未満
        /// Y kg
        /// </summary>
        [Column("WEIGHT_UNDER")]
        public double WeightUnder { get; set; }

        /// <summary>
        /// 体表面積条件_以上
        /// Y ㎡
        /// </summary>
        [Column("BODY_OVER")]
        public double BodyOver { get; set; }

        /// <summary>
        /// 体表面積条件_未満
        /// Y ㎡
        /// </summary>
        [Column("BODY_UNDER")]
        public double BodyUnder { get; set; }

        /// <summary>
        /// 投与経路
        /// 
        /// </summary>
        [Column("DRUG_ROUTE")]
        [MaxLength(40)]
        public string DrugRoute { get; set; } = string.Empty;

        /// <summary>
        /// 頓用フラグ
        /// 1:頓用である NULL:頓用ではない
        /// </summary>
        [Column("USE_FLG")]
        [MaxLength(1)]
        public string UseFlg { get; set; } = string.Empty;

        /// <summary>
        /// 投与要件
        /// </summary>
        [Column("DRUG_CONDITION")]
        [MaxLength(400)]
        public string DrugCondition { get; set; } = string.Empty;

        /// <summary>
        /// 効能効果
        /// </summary>
        [Column("KONOKOKA")]
        public string Konokoka { get; set; } = string.Empty;

        /// <summary>
        /// 用法用量
        /// </summary>
        [Column("USAGE_DOSAGE")]
        public string UsageDosage { get; set; } = string.Empty;

        /// <summary>
        /// 表ファイル名コード
        /// </summary>
        [Column("FILENAME_CD")]
        [MaxLength(7)]
        public string FilenameCd { get; set; } = string.Empty;

        /// <summary>
        /// 投与手技
        /// </summary>
        [Column("DRUG_SYUGI")]
        public string DrugSyugi { get; set; } = string.Empty;

        /// <summary>
        /// 適応対象部位
        /// </summary>
        [Column("TEKIO_BUI")]
        [MaxLength(300)]
        public string TekioBui { get; set; } = string.Empty;

        /// <summary>
        /// 溶解希釈関連
        /// </summary>
        [Column("YOUKAI_KISYAKU")]
        [MaxLength(1500)]
        public string YoukaiKisyaku { get; set; } = string.Empty;

        /// <summary>
        /// 推奨希釈液
        /// </summary>
        [Column("KISYAKUEKI")]
        [MaxLength(500)]
        public string Kisyakueki { get; set; } = string.Empty;

        /// <summary>
        /// 推奨溶解液
        /// 
        /// </summary>
        [Column("YOUKAIEKI")]
        [MaxLength(500)]
        public string Youkaieki { get; set; } = string.Empty;

        /// <summary>
        /// 排他フラグ
        /// 1:推奨溶解液、推奨希釈液以外は全て混注に不適
        ///                    NULL:排他しない
        /// </summary>
        [Column("HAITA_FLG")]
        [MaxLength(1)]
        public string HaitaFlg { get; set; } = string.Empty;

        /// <summary>
        /// 不適切希釈液
        /// 
        /// </summary>
        [Column("NG_KISYAKUEKI")]
        [MaxLength(500)]
        public string NgKisyakueki { get; set; } = string.Empty;

        /// <summary>
        /// 不適切溶解液
        /// 
        /// </summary>
        [Column("NG_YOUKAIEKI")]
        [MaxLength(500)]
        public string NgYoukaieki { get; set; } = string.Empty;

        /// <summary>
        /// 併用医薬品
        /// 
        /// </summary>
        [Column("COMBI_DRUG")]
        [MaxLength(200)]
        public string CombiDrug { get; set; } = string.Empty;

        /// <summary>
        /// 投与連結コード
        /// 
        /// </summary>
        [Column("DRUG_LINK_CD")]
        public int DrugLinkCd { get; set; }

        /// <summary>
        /// 投与順序
        /// 
        /// </summary>
        [Column("DRUG_ORDER")]
        public int DrugOrder { get; set; }

        /// <summary>
        /// 単回投与フラグ
        /// 1:単回投与 NULL:単回投与でない
        /// </summary>
        [Column("SINGLE_DRUG_FLG")]
        [MaxLength(1)]
        public string SingleDrugFlg { get; set; } = string.Empty;

        /// <summary>
        /// 休減薬コード
        /// 0:減薬量明示ありの減薬
        /// 1:減薬量明示なしの減薬
        /// 2:休薬
        /// </summary>
        [Column("KYUGEN_CD")]
        [MaxLength(1)]
        public string KyugenCd { get; set; } = string.Empty;

        /// <summary>
        /// 投与量チェックフラグ
        /// 0:投与量チェック不可能 1:投与量チェック可能
        /// </summary>
        [Column("DOSAGE_CHECK_FLG")]
        [MaxLength(1)]
        public string DosageCheckFlg { get; set; } = string.Empty;

        /// <summary>
        /// 一回量最小値
        /// Y
        /// </summary>
        [Column("ONCE_MIN")]
        public double OnceMin { get; set; }

        /// <summary>
        /// 一回量最大値
        /// Y
        /// </summary>
        [Column("ONCE_MAX")]
        public double OnceMax { get; set; }

        /// <summary>
        /// 一回量単位
        /// 
        /// </summary>
        [Column("ONCE_UNIT")]
        [MaxLength(30)]
        public string OnceUnit { get; set; } = string.Empty;

        /// <summary>
        /// 一回量上限値
        /// Y
        /// </summary>
        [Column("ONCE_LIMIT")]
        public double OnceLimit { get; set; }

        /// <summary>
        /// 一回上限量単位
        /// 
        /// </summary>
        [Column("ONCE_LIMIT_UNIT")]
        [MaxLength(30)]
        public string OnceLimitUnit { get; set; } = string.Empty;

        /// <summary>
        /// 一日投与量最小回数
        /// 
        /// </summary>
        [Column("DAY_MIN_CNT")]
        public double DayMinCnt { get; set; }

        /// <summary>
        /// 一日投与量最大回数
        /// 
        /// </summary>
        [Column("DAY_MAX_CNT")]
        public double DayMaxCnt { get; set; }

        /// <summary>
        /// 一日量最小値
        /// Y
        /// </summary>
        [Column("DAY_MIN")]
        public double DayMin { get; set; }

        /// <summary>
        /// 一日量最大値
        /// Y
        /// </summary>
        [Column("DAY_MAX")]
        public double DayMax { get; set; }

        /// <summary>
        /// 一日量単位
        /// 
        /// </summary>
        [Column("DAY_UNIT")]
        [MaxLength(30)]
        public string DayUnit { get; set; } = string.Empty;

        /// <summary>
        /// 一日量上限値
        /// Y
        /// </summary>
        [Column("DAY_LIMIT")]
        public double DayLimit { get; set; }

        /// <summary>
        /// 一日上限量単位
        /// 
        /// </summary>
        [Column("DAY_LIMIT_UNIT")]
        [MaxLength(30)]
        public string DayLimitUnit { get; set; } = string.Empty;

        /// <summary>
        /// 起床時
        /// </summary>
        [Column("RISE")]
        public int Rise { get; set; }

        /// <summary>
        /// 朝
        /// </summary>
        [Column("MORNING")]
        public int Morning { get; set; }

        /// <summary>
        /// 昼
        /// 
        /// </summary>
        [Column("DAYTIME")]
        public int Daytime { get; set; }

        /// <summary>
        /// 夕
        /// </summary>
        [Column("NIGHT")]
        public int Night { get; set; }

        /// <summary>
        /// ねる前
        /// </summary>
        [Column("SLEEP")]
        public int Sleep { get; set; }

        /// <summary>
        /// 食前
        /// </summary>
        [Column("BEFORE_MEAL")]
        public int BeforeMeal { get; set; }

        /// <summary>
        /// 食直前
        /// </summary>
        [Column("JUST_BEFORE_MEAL")]
        public int JustBeforeMeal { get; set; }

        /// <summary>
        /// 食後
        /// </summary>
        [Column("AFTER_MEAL")]
        public int AfterMeal { get; set; }

        /// <summary>
        /// 食直後
        /// </summary>
        [Column("JUST_AFTER_MEAL")]
        public int JustAfterMeal { get; set; }

        /// <summary>
        /// 食間
        /// </summary>
        [Column("BETWEEN_MEAL")]
        public int BetweenMeal { get; set; }

        /// <summary>
        /// それ以外
        /// </summary>
        [Column("ELSE_TIME")]
        public int ElseTime { get; set; }

        /// <summary>
        /// 上限投与量定義期間
        /// d(日) w(週) m(月) y(年)
        /// </summary>
        [Column("DOSAGE_LIMIT_TERM")]
        public int DosageLimitTerm { get; set; }

        /// <summary>
        /// 上限投与量単位
        /// </summary>
        [Column("DOSAGE_LIMIT_UNIT")]
        [MaxLength(1)]
        public string DosageLimitUnit { get; set; } = string.Empty;

        /// <summary>
        /// 単位期間投与量上限値
        /// Y
        /// </summary>
        [Column("UNITTERM_LIMIT")]
        public double UnittermLimit { get; set; }

        /// <summary>
        /// 単位期間投与量単位
        /// </summary>
        [Column("UNITTERM_UNIT")]
        [MaxLength(30)]
        public string UnittermUnit { get; set; } = string.Empty;

        /// <summary>
        /// 用量追加フラグ
        /// 1:用量追加可 NULL:用量追加不可
        /// </summary>
        [Column("DOSAGE_ADD_FLG")]
        [MaxLength(1)]
        public string DosageAddFlg { get; set; } = string.Empty;

        /// <summary>
        /// 適宜増減フラグ
        /// 1:適宜増減の記載あり NULL:適宜増減の記載なし
        /// </summary>
        [Column("INC_DEC_FLG")]
        [MaxLength(1)]
        public string IncDecFlg { get; set; } = string.Empty;

        /// <summary>
        /// 適宜減量フラグ
        /// 1:適宜減量の記載あり NULL:適宜減量の記載なし
        /// </summary>
        [Column("DEC_FLG")]
        [MaxLength(1)]
        public string DecFlg { get; set; } = string.Empty;

        /// <summary>
        /// 投与量増減間隔
        /// 
        /// </summary>
        [Column("INC_DEC_INTERVAL")]
        public int IncDecInterval { get; set; }

        /// <summary>
        /// 投与量増減間隔単位
        /// d(日) w(週) m(月) y(年)
        /// </summary>
        [Column("INC_DEC_INTERVAL_UNIT")]
        [MaxLength(1)]
        public string IncDecIntervalUnit { get; set; } = string.Empty;

        /// <summary>
        /// 減量限界値
        /// Y
        /// </summary>
        [Column("DEC_LIMIT")]
        public double DecLimit { get; set; }

        /// <summary>
        /// 増量限界値
        /// Y
        /// </summary>
        [Column("INC_LIMIT")]
        public double IncLimit { get; set; }

        /// <summary>
        /// 投与量増減限界単位
        /// 
        /// </summary>
        [Column("INC_DEC_LIMIT_UNIT")]
        [MaxLength(30)]
        public string IncDecLimitUnit { get; set; } = string.Empty;

        /// <summary>
        /// 時間関連
        /// 
        /// </summary>
        [Column("TIME_DEPEND")]
        [MaxLength(1000)]
        public string TimeDepend { get; set; } = string.Empty;

        /// <summary>
        /// 標準判定投与期間
        /// 
        /// </summary>
        [Column("JUDGE_TERM")]
        public int JudgeTerm { get; set; }

        /// <summary>
        /// 標準判定投与期間単位
        /// d(日) w(週) m(月) y(年)
        /// </summary>
        [Column("JUDGE_TERM_UNIT")]
        [MaxLength(1)]
        public string JudgeTermUnit { get; set; } = string.Empty;

        /// <summary>
        /// 延長容認フラグ
        /// 1:期間延長可 NULL:期間延長不可
        /// </summary>
        [Column("EXTEND_FLG")]
        [MaxLength(1)]
        public string ExtendFlg { get; set; } = string.Empty;

        /// <summary>
        /// 追加期間
        /// 
        /// </summary>
        [Column("ADD_TERM")]
        public int AddTerm { get; set; }

        /// <summary>
        /// 追加期間単位
        /// d(日) w(週) m(月) y(年)
        /// </summary>
        [Column("ADD_TERM_UNIT")]
        [MaxLength(1)]
        public string AddTermUnit { get; set; } = string.Empty;

        /// <summary>
        /// 投与間隔警告フラグ
        /// 1:連日投与ではない NULL:連日投与
        /// </summary>
        [Column("INTERVAL_WARNING_FLG")]
        [MaxLength(1)]
        public string IntervalWarningFlg { get; set; } = string.Empty;
    }
}
