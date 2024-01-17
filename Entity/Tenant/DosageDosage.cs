using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m46_dosage_dosage")]
    public class DosageDosage : EmrCloneable<DosageDosage>
    {
        /// <summary>
        /// ＤＯＥＩコード
        /// </summary>
        
        [Column("doei_cd", Order = 1)]
        public string DoeiCd { get; set; } = string.Empty;

        /// <summary>
        /// ＤＯＥＩコード連番
        /// </summary>
        
        [Column("doei_seq_no", Order = 2)]
        public int DoeiSeqNo { get; set; }

        /// <summary>
        /// 効能効果コード
        /// </summary>
        [Column("konokoka_cd")]
        [MaxLength(7)]
        public string? KonokokaCd { get; set; } = string.Empty;

        /// <summary>
        /// 検査条件組合せコード
        /// 
        /// </summary>
        [Column("kensa_pcd")]
        [MaxLength(7)]
        public string? KensaPcd { get; set; } = string.Empty;

        /// <summary>
        /// 年齢条件_以上
        /// Y
        /// </summary>
        [Column("age_over")]
        public double AgeOver { get; set; }

        /// <summary>
        /// 年齢条件_未満
        /// Y
        /// </summary>
        [Column("age_under")]
        public double AgeUnder { get; set; }

        /// <summary>
        /// 年齢条件コード
        /// 1:歳(年齢) 2:月(月齢) 3:週(週齢) 4:日(日齢)
        /// </summary>
        [Column("age_cd")]
        [MaxLength(1)]
        public string? AgeCd { get; set; } = string.Empty;

        /// <summary>
        /// 体重条件_以上
        /// Y kg
        /// </summary>
        [Column("weight_over")]
        public double WeightOver { get; set; }

        /// <summary>
        /// 体重条件_未満
        /// Y kg
        /// </summary>
        [Column("weight_under")]
        public double WeightUnder { get; set; }

        /// <summary>
        /// 体表面積条件_以上
        /// Y ㎡
        /// </summary>
        [Column("body_over")]
        public double BodyOver { get; set; }

        /// <summary>
        /// 体表面積条件_未満
        /// Y ㎡
        /// </summary>
        [Column("body_under")]
        public double BodyUnder { get; set; }

        /// <summary>
        /// 投与経路
        /// 
        /// </summary>
        [Column("drug_route")]
        [MaxLength(40)]
        public string? DrugRoute { get; set; } = string.Empty;

        /// <summary>
        /// 頓用フラグ
        /// 1:頓用である NULL:頓用ではない
        /// </summary>
        [Column("use_flg")]
        [MaxLength(1)]
        public string? UseFlg { get; set; } = string.Empty;

        /// <summary>
        /// 投与要件
        /// </summary>
        [Column("drug_condition")]
        [MaxLength(400)]
        public string? DrugCondition { get; set; } = string.Empty;

        /// <summary>
        /// 効能効果
        /// </summary>
        [Column("konokoka")]
        public string? Konokoka { get; set; } = string.Empty;

        /// <summary>
        /// 用法用量
        /// </summary>
        [Column("usage_dosage")]
        public string? UsageDosage { get; set; } = string.Empty;

        /// <summary>
        /// 表ファイル名コード
        /// </summary>
        [Column("filename_cd")]
        [MaxLength(7)]
        public string? FilenameCd { get; set; } = string.Empty;

        /// <summary>
        /// 投与手技
        /// </summary>
        [Column("drug_syugi")]
        public string? DrugSyugi { get; set; } = string.Empty;

        /// <summary>
        /// 適応対象部位
        /// </summary>
        [Column("tekio_bui")]
        [MaxLength(300)]
        public string? TekioBui { get; set; } = string.Empty;

        /// <summary>
        /// 溶解希釈関連
        /// </summary>
        [Column("youkai_kisyaku")]
        [MaxLength(1500)]
        public string? YoukaiKisyaku { get; set; } = string.Empty;

        /// <summary>
        /// 推奨希釈液
        /// </summary>
        [Column("kisyakueki")]
        [MaxLength(500)]
        public string? Kisyakueki { get; set; } = string.Empty;

        /// <summary>
        /// 推奨溶解液
        /// 
        /// </summary>
        [Column("youkaieki")]
        [MaxLength(500)]
        public string? Youkaieki { get; set; } = string.Empty;

        /// <summary>
        /// 排他フラグ
        /// 1:推奨溶解液、推奨希釈液以外は全て混注に不適
        ///                    NULL:排他しない
        /// </summary>
        [Column("haita_flg")]
        [MaxLength(1)]
        public string? HaitaFlg { get; set; } = string.Empty;

        /// <summary>
        /// 不適切希釈液
        /// 
        /// </summary>
        [Column("ng_kisyakueki")]
        [MaxLength(500)]
        public string? NgKisyakueki { get; set; } = string.Empty;

        /// <summary>
        /// 不適切溶解液
        /// 
        /// </summary>
        [Column("ng_youkaieki")]
        [MaxLength(500)]
        public string? NgYoukaieki { get; set; } = string.Empty;

        /// <summary>
        /// 併用医薬品
        /// 
        /// </summary>
        [Column("combi_drug")]
        [MaxLength(200)]
        public string? CombiDrug { get; set; } = string.Empty;

        /// <summary>
        /// 投与連結コード
        /// 
        /// </summary>
        [Column("drug_link_cd")]
        public int DrugLinkCd { get; set; }

        /// <summary>
        /// 投与順序
        /// 
        /// </summary>
        [Column("drug_order")]
        public int DrugOrder { get; set; }

        /// <summary>
        /// 単回投与フラグ
        /// 1:単回投与 NULL:単回投与でない
        /// </summary>
        [Column("single_drug_flg")]
        [MaxLength(1)]
        public string? SingleDrugFlg { get; set; } = string.Empty;

        /// <summary>
        /// 休減薬コード
        /// 0:減薬量明示ありの減薬
        /// 1:減薬量明示なしの減薬
        /// 2:休薬
        /// </summary>
        [Column("kyugen_cd")]
        [MaxLength(1)]
        public string? KyugenCd { get; set; } = string.Empty;

        /// <summary>
        /// 投与量チェックフラグ
        /// 0:投与量チェック不可能 1:投与量チェック可能
        /// </summary>
        [Column("dosage_check_flg")]
        [MaxLength(1)]
        public string? DosageCheckFlg { get; set; } = string.Empty;

        /// <summary>
        /// 一回量最小値
        /// Y
        /// </summary>
        [Column("once_min")]
        public double OnceMin { get; set; }

        /// <summary>
        /// 一回量最大値
        /// Y
        /// </summary>
        [Column("once_max")]
        public double OnceMax { get; set; }

        /// <summary>
        /// 一回量単位
        /// 
        /// </summary>
        [Column("once_unit")]
        [MaxLength(30)]
        public string? OnceUnit { get; set; } = string.Empty;

        /// <summary>
        /// 一回量上限値
        /// Y
        /// </summary>
        [Column("once_limit")]
        public double OnceLimit { get; set; }

        /// <summary>
        /// 一回上限量単位
        /// 
        /// </summary>
        [Column("once_limit_unit")]
        [MaxLength(30)]
        public string? OnceLimitUnit { get; set; } = string.Empty;

        /// <summary>
        /// 一日投与量最小回数
        /// 
        /// </summary>
        [Column("day_min_cnt")]
        public double DayMinCnt { get; set; }

        /// <summary>
        /// 一日投与量最大回数
        /// 
        /// </summary>
        [Column("day_max_cnt")]
        public double DayMaxCnt { get; set; }

        /// <summary>
        /// 一日量最小値
        /// Y
        /// </summary>
        [Column("day_min")]
        public double DayMin { get; set; }

        /// <summary>
        /// 一日量最大値
        /// Y
        /// </summary>
        [Column("day_max")]
        public double DayMax { get; set; }

        /// <summary>
        /// 一日量単位
        /// 
        /// </summary>
        [Column("day_unit")]
        [MaxLength(30)]
        public string? DayUnit { get; set; } = string.Empty;

        /// <summary>
        /// 一日量上限値
        /// Y
        /// </summary>
        [Column("day_limit")]
        public double DayLimit { get; set; }

        /// <summary>
        /// 一日上限量単位
        /// 
        /// </summary>
        [Column("day_limit_unit")]
        [MaxLength(30)]
        public string? DayLimitUnit { get; set; } = string.Empty;

        /// <summary>
        /// 起床時
        /// </summary>
        [Column("rise")]
        public int Rise { get; set; }

        /// <summary>
        /// 朝
        /// </summary>
        [Column("morning")]
        public int Morning { get; set; }

        /// <summary>
        /// 昼
        /// 
        /// </summary>
        [Column("daytime")]
        public int Daytime { get; set; }

        /// <summary>
        /// 夕
        /// </summary>
        [Column("night")]
        public int Night { get; set; }

        /// <summary>
        /// ねる前
        /// </summary>
        [Column("sleep")]
        public int Sleep { get; set; }

        /// <summary>
        /// 食前
        /// </summary>
        [Column("before_meal")]
        public int BeforeMeal { get; set; }

        /// <summary>
        /// 食直前
        /// </summary>
        [Column("just_before_meal")]
        public int JustBeforeMeal { get; set; }

        /// <summary>
        /// 食後
        /// </summary>
        [Column("after_meal")]
        public int AfterMeal { get; set; }

        /// <summary>
        /// 食直後
        /// </summary>
        [Column("just_after_meal")]
        public int JustAfterMeal { get; set; }

        /// <summary>
        /// 食間
        /// </summary>
        [Column("between_meal")]
        public int BetweenMeal { get; set; }

        /// <summary>
        /// それ以外
        /// </summary>
        [Column("else_time")]
        public int ElseTime { get; set; }

        /// <summary>
        /// 上限投与量定義期間
        /// d(日) w(週) m(月) y(年)
        /// </summary>
        [Column("dosage_limit_term")]
        public int DosageLimitTerm { get; set; }

        /// <summary>
        /// 上限投与量単位
        /// </summary>
        [Column("dosage_limit_unit")]
        [MaxLength(1)]
        public string? DosageLimitUnit { get; set; } = string.Empty;

        /// <summary>
        /// 単位期間投与量上限値
        /// Y
        /// </summary>
        [Column("unitterm_limit")]
        public double UnittermLimit { get; set; }

        /// <summary>
        /// 単位期間投与量単位
        /// </summary>
        [Column("unitterm_unit")]
        [MaxLength(30)]
        public string? UnittermUnit { get; set; } = string.Empty;

        /// <summary>
        /// 用量追加フラグ
        /// 1:用量追加可 NULL:用量追加不可
        /// </summary>
        [Column("dosage_add_flg")]
        [MaxLength(1)]
        public string? DosageAddFlg { get; set; } = string.Empty;

        /// <summary>
        /// 適宜増減フラグ
        /// 1:適宜増減の記載あり NULL:適宜増減の記載なし
        /// </summary>
        [Column("inc_dec_flg")]
        [MaxLength(1)]
        public string? IncDecFlg { get; set; } = string.Empty;

        /// <summary>
        /// 適宜減量フラグ
        /// 1:適宜減量の記載あり NULL:適宜減量の記載なし
        /// </summary>
        [Column("dec_flg")]
        [MaxLength(1)]
        public string? DecFlg { get; set; } = string.Empty;

        /// <summary>
        /// 投与量増減間隔
        /// 
        /// </summary>
        [Column("inc_dec_interval")]
        public int IncDecInterval { get; set; }

        /// <summary>
        /// 投与量増減間隔単位
        /// d(日) w(週) m(月) y(年)
        /// </summary>
        [Column("inc_dec_interval_unit")]
        [MaxLength(1)]
        public string? IncDecIntervalUnit { get; set; } = string.Empty;

        /// <summary>
        /// 減量限界値
        /// Y
        /// </summary>
        [Column("dec_limit")]
        public double DecLimit { get; set; }

        /// <summary>
        /// 増量限界値
        /// Y
        /// </summary>
        [Column("inc_limit")]
        public double IncLimit { get; set; }

        /// <summary>
        /// 投与量増減限界単位
        /// 
        /// </summary>
        [Column("inc_dec_limit_unit")]
        [MaxLength(30)]
        public string? IncDecLimitUnit { get; set; } = string.Empty;

        /// <summary>
        /// 時間関連
        /// 
        /// </summary>
        [Column("time_depend")]
        [MaxLength(1000)]
        public string? TimeDepend { get; set; } = string.Empty;

        /// <summary>
        /// 標準判定投与期間
        /// 
        /// </summary>
        [Column("judge_term")]
        public int JudgeTerm { get; set; }

        /// <summary>
        /// 標準判定投与期間単位
        /// d(日) w(週) m(月) y(年)
        /// </summary>
        [Column("judge_term_unit")]
        [MaxLength(1)]
        public string? JudgeTermUnit { get; set; } = string.Empty;

        /// <summary>
        /// 延長容認フラグ
        /// 1:期間延長可 NULL:期間延長不可
        /// </summary>
        [Column("extend_flg")]
        [MaxLength(1)]
        public string? ExtendFlg { get; set; } = string.Empty;

        /// <summary>
        /// 追加期間
        /// 
        /// </summary>
        [Column("add_term")]
        public int AddTerm { get; set; }

        /// <summary>
        /// 追加期間単位
        /// d(日) w(週) m(月) y(年)
        /// </summary>
        [Column("add_term_unit")]
        [MaxLength(1)]
        public string? AddTermUnit { get; set; } = string.Empty;

        /// <summary>
        /// 投与間隔警告フラグ
        /// 1:連日投与ではない NULL:連日投与
        /// </summary>
        [Column("interval_warning_flg")]
        [MaxLength(1)]
        public string? IntervalWarningFlg { get; set; } = string.Empty;
    }
}
