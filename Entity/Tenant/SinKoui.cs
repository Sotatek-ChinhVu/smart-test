using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "sin_koui")]
    public class SinKoui : EmrCloneable<SinKoui>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        //[Index("sin_koui_idx01", 1)]
        //[Index("sin_koui_idx02", 1)]
        //[Index("sin_koui_idx03", 1)]
        //[Index("sin_koui_idx04", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        //[Index("sin_koui_idx01", 2)]
        //[Index("sin_koui_idx02", 2)]
        //[Index("sin_koui_idx03", 2)]
        //[Index("sin_koui_idx04", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("sin_ym", Order = 3)]
        //[Index("sin_koui_idx01", 3)]
        //[Index("sin_koui_idx02", 3)]
        //[Index("sin_koui_idx03", 3)]
        //[Index("sin_koui_idx04", 3)]
        public int SinYm { get; set; }

        /// <summary>
        /// 剤番号
        /// SIN_RP_INF.RP_NO
        /// </summary>
        
        [Column("rp_no", Order = 4)]
        //[Index("sin_koui_idx02", 4)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 5)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        [Column("hoken_pid")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 保険ID
        /// </summary>
        [Column("hoken_id")]
        [CustomAttribute.DefaultValue(0)]
        //[Index("sin_koui_idx04", 4)]
        public int HokenId { get; set; }

        /// <summary>
        /// 点数欄集計先
        /// TEN_MST.SYUKEI_SAKI + 枝番 ※別シート参照
        /// </summary>
        [Column("syukei_saki")]
        [MaxLength(4)]
        public string? SyukeiSaki { get; set; } = string.Empty;

        /// <summary>
        /// 包括対象検査
        /// "0: 1～12以外の診療行為 
        /// 1: 血液化学検査の包括項目 
        /// 2: 内分泌学的検査の包括項目 
        /// 3: 肝炎ウイルス関連検査の包括項目 
        /// 5: 腫瘍マーカーの包括項目 
        /// 6: 出血・凝固検査の包括項目 
        /// 7: 自己抗体検査の包括項目 
        /// 8: 内分泌負荷試験の包括項目 
        /// 9: 感染症免疫学的検査のうち、ウイルス抗体価（定性・半定量・定量） 
        /// 10: 感染症免疫学的検査のうち、グロブリンクラス別ウイルス抗体価 
        /// 11:血漿蛋白免疫学的検査のうち、特異的ＩｇＥ半定量・定量及びアレルゲン刺激性遊離ヒスタミン（ＨＲＴ） 
        /// 12: 悪性腫瘍遺伝子検査の包括項目 "
        /// </summary>
        [Column("hokatu_kensa")]
        [CustomAttribute.DefaultValue(0)]
        public int HokatuKensa { get; set; }

        /// <summary>
        /// 合計点数
        /// 
        /// </summary>
        [Column("total_ten")]
        [CustomAttribute.DefaultValue(0)]
        public double TotalTen { get; set; }

        /// <summary>
        /// 点数小計
        /// 
        /// </summary>
        [Column("ten")]
        [CustomAttribute.DefaultValue(0)]
        public double Ten { get; set; }

        /// <summary>
        /// 消費税
        /// 
        /// </summary>
        [Column("zei")]
        [CustomAttribute.DefaultValue(0)]
        public double Zei { get; set; }

        /// <summary>
        /// 回数小計
        /// 
        /// </summary>
        [Column("count")]
        [CustomAttribute.DefaultValue(0)]
        public int Count { get; set; }

        /// <summary>
        /// 点数回数
        /// 
        /// </summary>
        [Column("ten_count")]
        [MaxLength(20)]
        public string? TenCount { get; set; } = string.Empty;

        /// <summary>
        /// 点数欄回数
        /// 
        /// </summary>
        [Column("ten_col_count")]
        [CustomAttribute.DefaultValue(0)]
        public int TenColCount { get; set; }

        /// <summary>
        /// レセ非表示区分
        /// 1:非表示
        /// </summary>
        [Column("is_nodsp_rece")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspRece { get; set; }

        /// <summary>
        /// 紙レセ非表示区分
        /// 1:非表示
        /// </summary>
        [Column("is_nodsp_paper_rece")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspPaperRece { get; set; }

        /// <summary>
        /// 院外処方区分
        /// 1:院外処方
        /// </summary>
        [Column("inout_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int InoutKbn { get; set; }

        /// <summary>
        /// 円点区分
        /// 0:点数 1:金額
        /// </summary>
        [Column("enten_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int EntenKbn { get; set; }

        /// <summary>
        /// コード区分
        /// 代表項目のTEN_MST.CD_KBN
        /// </summary>
        [Column("cd_kbn")]
        [MaxLength(2)]
        public string? CdKbn { get; set; } = string.Empty;

        /// <summary>
        /// 代表レコード識別
        /// 代表項目のレコード識別
        /// </summary>
        [Column("rec_id")]
        [MaxLength(2)]
        public string? RecId { get; set; } = string.Empty;

        /// <summary>
        /// 自費種別
        /// 代表項目のJIHI_SBT_MST.JIHI_SBT
        /// </summary>
        [Column("jihi_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiSbt { get; set; }

        /// <summary>
        /// 課税区分
        /// TEN_MST.KAZEI_KBN
        /// </summary>
        [Column("kazei_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KazeiKbn { get; set; }

        /// <summary>
        /// 詳細データ
        /// 詳細を文字列化したもの ※
        /// </summary>
        [Column("detail_data")]
        public string? DetailData { get; set; } = string.Empty;

        /// <summary>
        /// 算定日情報1
        /// 
        /// </summary>
        [Column("day1")]
        [CustomAttribute.DefaultValue(0)]
        public int Day1 { get; set; }

        /// <summary>
        /// 算定日情報2
        /// 
        /// </summary>
        [Column("day2")]
        [CustomAttribute.DefaultValue(0)]
        public int Day2 { get; set; }

        /// <summary>
        /// 算定日情報3
        /// 
        /// </summary>
        [Column("day3")]
        [CustomAttribute.DefaultValue(0)]
        public int Day3 { get; set; }

        /// <summary>
        /// 算定日情報4
        /// 
        /// </summary>
        [Column("day4")]
        [CustomAttribute.DefaultValue(0)]
        public int Day4 { get; set; }

        /// <summary>
        /// 算定日情報5
        /// 
        /// </summary>
        [Column("day5")]
        [CustomAttribute.DefaultValue(0)]
        public int Day5 { get; set; }

        /// <summary>
        /// 算定日情報6
        /// 
        /// </summary>
        [Column("day6")]
        [CustomAttribute.DefaultValue(0)]
        public int Day6 { get; set; }

        /// <summary>
        /// 算定日情報7
        /// 
        /// </summary>
        [Column("day7")]
        [CustomAttribute.DefaultValue(0)]
        public int Day7 { get; set; }

        /// <summary>
        /// 算定日情報8
        /// 
        /// </summary>
        [Column("day8")]
        [CustomAttribute.DefaultValue(0)]
        public int Day8 { get; set; }

        /// <summary>
        /// 算定日情報9
        /// 
        /// </summary>
        [Column("day9")]
        [CustomAttribute.DefaultValue(0)]
        public int Day9 { get; set; }

        /// <summary>
        /// 算定日情報10
        /// 
        /// </summary>
        [Column("day10")]
        [CustomAttribute.DefaultValue(0)]
        public int Day10 { get; set; }

        /// <summary>
        /// 算定日情報11
        /// 
        /// </summary>
        [Column("day11")]
        [CustomAttribute.DefaultValue(0)]
        public int Day11 { get; set; }

        /// <summary>
        /// 算定日情報12
        /// 
        /// </summary>
        [Column("day12")]
        [CustomAttribute.DefaultValue(0)]
        public int Day12 { get; set; }

        /// <summary>
        /// 算定日情報13
        /// 
        /// </summary>
        [Column("day13")]
        [CustomAttribute.DefaultValue(0)]
        public int Day13 { get; set; }

        /// <summary>
        /// 算定日情報14
        /// 
        /// </summary>
        [Column("day14")]
        [CustomAttribute.DefaultValue(0)]
        public int Day14 { get; set; }

        /// <summary>
        /// 算定日情報15
        /// 
        /// </summary>
        [Column("day15")]
        [CustomAttribute.DefaultValue(0)]
        public int Day15 { get; set; }

        /// <summary>
        /// 算定日情報16
        /// 
        /// </summary>
        [Column("day16")]
        [CustomAttribute.DefaultValue(0)]
        public int Day16 { get; set; }

        /// <summary>
        /// 算定日情報17
        /// 
        /// </summary>
        [Column("day17")]
        [CustomAttribute.DefaultValue(0)]
        public int Day17 { get; set; }

        /// <summary>
        /// 算定日情報18
        /// 
        /// </summary>
        [Column("day18")]
        [CustomAttribute.DefaultValue(0)]
        public int Day18 { get; set; }

        /// <summary>
        /// 算定日情報19
        /// 
        /// </summary>
        [Column("day19")]
        [CustomAttribute.DefaultValue(0)]
        public int Day19 { get; set; }

        /// <summary>
        /// 算定日情報20
        /// 
        /// </summary>
        [Column("day20")]
        [CustomAttribute.DefaultValue(0)]
        public int Day20 { get; set; }

        /// <summary>
        /// 算定日情報21
        /// 
        /// </summary>
        [Column("day21")]
        [CustomAttribute.DefaultValue(0)]
        public int Day21 { get; set; }

        /// <summary>
        /// 算定日情報22
        /// 
        /// </summary>
        [Column("day22")]
        [CustomAttribute.DefaultValue(0)]
        public int Day22 { get; set; }

        /// <summary>
        /// 算定日情報23
        /// 
        /// </summary>
        [Column("day23")]
        [CustomAttribute.DefaultValue(0)]
        public int Day23 { get; set; }

        /// <summary>
        /// 算定日情報24
        /// 
        /// </summary>
        [Column("day24")]
        [CustomAttribute.DefaultValue(0)]
        public int Day24 { get; set; }

        /// <summary>
        /// 算定日情報25
        /// 
        /// </summary>
        [Column("day25")]
        [CustomAttribute.DefaultValue(0)]
        public int Day25 { get; set; }

        /// <summary>
        /// 算定日情報26
        /// 
        /// </summary>
        [Column("day26")]
        [CustomAttribute.DefaultValue(0)]
        public int Day26 { get; set; }

        /// <summary>
        /// 算定日情報27
        /// 
        /// </summary>
        [Column("day27")]
        [CustomAttribute.DefaultValue(0)]
        public int Day27 { get; set; }

        /// <summary>
        /// 算定日情報28
        /// 
        /// </summary>
        [Column("day28")]
        [CustomAttribute.DefaultValue(0)]
        public int Day28 { get; set; }

        /// <summary>
        /// 算定日情報29
        /// 
        /// </summary>
        [Column("day29")]
        [CustomAttribute.DefaultValue(0)]
        public int Day29 { get; set; }

        /// <summary>
        /// 算定日情報30
        /// 
        /// </summary>
        [Column("day30")]
        [CustomAttribute.DefaultValue(0)]
        public int Day30 { get; set; }

        /// <summary>
        /// 算定日情報31
        /// 
        /// </summary>
        [Column("day31")]
        [CustomAttribute.DefaultValue(0)]
        public int Day31 { get; set; }

        /// <summary>
        /// 削除区分
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        //[Index("sin_koui_idx03", 4)]
        public int IsDeleted { get; set; }


        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;

        /// <summary>
        /// EF対象フラグ
        ///     1:EFファイル出力対象の削除項目   
        /// </summary>
        [Column("ef_flg")]
        [CustomAttribute.DefaultValue(0)]
        public int EfFlg { get; set; }

        /// <summary>
        /// EF用合計点数
        /// 
        /// </summary>
        [Column("ef_total_ten")]
        [CustomAttribute.DefaultValue(0)]
        public double EfTotalTen { get; set; }

        /// <summary>
        /// EF用点数小計
        /// 
        /// </summary>
        [Column("ef_ten")]
        [CustomAttribute.DefaultValue(0)]
        public double EfTen { get; set; }

        /// <summary>
        /// EF用点数回数
        /// 
        /// </summary>
        [Column("ef_ten_count")]
        [MaxLength(20)]
        public string EfTenCount { get; set; } = string.Empty;
    }
}
