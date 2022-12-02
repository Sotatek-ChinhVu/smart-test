using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SIN_KOUI")]
    public class SinKoui : EmrCloneable<SinKoui>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        //[Index("SIN_KOUI_IDX01", 1)]
        //[Index("SIN_KOUI_IDX02", 1)]
        //[Index("SIN_KOUI_IDX03", 1)]
        //[Index("SIN_KOUI_IDX04", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("PT_ID", Order = 2)]
        //[Index("SIN_KOUI_IDX01", 2)]
        //[Index("SIN_KOUI_IDX02", 2)]
        //[Index("SIN_KOUI_IDX03", 2)]
        //[Index("SIN_KOUI_IDX04", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("SIN_YM", Order = 3)]
        //[Index("SIN_KOUI_IDX01", 3)]
        //[Index("SIN_KOUI_IDX02", 3)]
        //[Index("SIN_KOUI_IDX03", 3)]
        //[Index("SIN_KOUI_IDX04", 3)]
        public int SinYm { get; set; }

        /// <summary>
        /// 剤番号
        /// SIN_RP_INF.RP_NO
        /// </summary>
        
        [Column("RP_NO", Order = 4)]
        //[Index("SIN_KOUI_IDX02", 4)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("SEQ_NO", Order = 5)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        [Column("HOKEN_PID")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 保険ID
        /// </summary>
        [Column("HOKEN_ID")]
        [CustomAttribute.DefaultValue(0)]
        //[Index("SIN_KOUI_IDX04", 4)]
        public int HokenId { get; set; }

        /// <summary>
        /// 点数欄集計先
        /// TEN_MST.SYUKEI_SAKI + 枝番 ※別シート参照
        /// </summary>
        [Column("SYUKEI_SAKI")]
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
        [Column("HOKATU_KENSA")]
        [CustomAttribute.DefaultValue(0)]
        public int HokatuKensa { get; set; }

        /// <summary>
        /// 合計点数
        /// 
        /// </summary>
        [Column("TOTAL_TEN")]
        [CustomAttribute.DefaultValue(0)]
        public double TotalTen { get; set; }

        /// <summary>
        /// 点数小計
        /// 
        /// </summary>
        [Column("TEN")]
        [CustomAttribute.DefaultValue(0)]
        public double Ten { get; set; }

        /// <summary>
        /// 消費税
        /// 
        /// </summary>
        [Column("ZEI")]
        [CustomAttribute.DefaultValue(0)]
        public double Zei { get; set; }

        /// <summary>
        /// 回数小計
        /// 
        /// </summary>
        [Column("COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int Count { get; set; }

        /// <summary>
        /// 点数回数
        /// 
        /// </summary>
        [Column("TEN_COUNT")]
        [MaxLength(20)]
        public string? TenCount { get; set; } = string.Empty;

        /// <summary>
        /// 点数欄回数
        /// 
        /// </summary>
        [Column("TEN_COL_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int TenColCount { get; set; }

        /// <summary>
        /// レセ非表示区分
        /// 1:非表示
        /// </summary>
        [Column("IS_NODSP_RECE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspRece { get; set; }

        /// <summary>
        /// 紙レセ非表示区分
        /// 1:非表示
        /// </summary>
        [Column("IS_NODSP_PAPER_RECE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspPaperRece { get; set; }

        /// <summary>
        /// 院外処方区分
        /// 1:院外処方
        /// </summary>
        [Column("INOUT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int InoutKbn { get; set; }

        /// <summary>
        /// 円点区分
        /// 0:点数 1:金額
        /// </summary>
        [Column("ENTEN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int EntenKbn { get; set; }

        /// <summary>
        /// コード区分
        /// 代表項目のTEN_MST.CD_KBN
        /// </summary>
        [Column("CD_KBN")]
        [MaxLength(2)]
        public string? CdKbn { get; set; } = string.Empty;

        /// <summary>
        /// 代表レコード識別
        /// 代表項目のレコード識別
        /// </summary>
        [Column("REC_ID")]
        [MaxLength(2)]
        public string? RecId { get; set; } = string.Empty;

        /// <summary>
        /// 自費種別
        /// 代表項目のJIHI_SBT_MST.JIHI_SBT
        /// </summary>
        [Column("JIHI_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiSbt { get; set; }

        /// <summary>
        /// 課税区分
        /// TEN_MST.KAZEI_KBN
        /// </summary>
        [Column("KAZEI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KazeiKbn { get; set; }

        /// <summary>
        /// 詳細データ
        /// 詳細を文字列化したもの ※
        /// </summary>
        [Column("DETAIL_DATA")]
        public string? DetailData { get; set; } = string.Empty;

        /// <summary>
        /// 算定日情報1
        /// 
        /// </summary>
        [Column("DAY1")]
        [CustomAttribute.DefaultValue(0)]
        public int Day1 { get; set; }

        /// <summary>
        /// 算定日情報2
        /// 
        /// </summary>
        [Column("DAY2")]
        [CustomAttribute.DefaultValue(0)]
        public int Day2 { get; set; }

        /// <summary>
        /// 算定日情報3
        /// 
        /// </summary>
        [Column("DAY3")]
        [CustomAttribute.DefaultValue(0)]
        public int Day3 { get; set; }

        /// <summary>
        /// 算定日情報4
        /// 
        /// </summary>
        [Column("DAY4")]
        [CustomAttribute.DefaultValue(0)]
        public int Day4 { get; set; }

        /// <summary>
        /// 算定日情報5
        /// 
        /// </summary>
        [Column("DAY5")]
        [CustomAttribute.DefaultValue(0)]
        public int Day5 { get; set; }

        /// <summary>
        /// 算定日情報6
        /// 
        /// </summary>
        [Column("DAY6")]
        [CustomAttribute.DefaultValue(0)]
        public int Day6 { get; set; }

        /// <summary>
        /// 算定日情報7
        /// 
        /// </summary>
        [Column("DAY7")]
        [CustomAttribute.DefaultValue(0)]
        public int Day7 { get; set; }

        /// <summary>
        /// 算定日情報8
        /// 
        /// </summary>
        [Column("DAY8")]
        [CustomAttribute.DefaultValue(0)]
        public int Day8 { get; set; }

        /// <summary>
        /// 算定日情報9
        /// 
        /// </summary>
        [Column("DAY9")]
        [CustomAttribute.DefaultValue(0)]
        public int Day9 { get; set; }

        /// <summary>
        /// 算定日情報10
        /// 
        /// </summary>
        [Column("DAY10")]
        [CustomAttribute.DefaultValue(0)]
        public int Day10 { get; set; }

        /// <summary>
        /// 算定日情報11
        /// 
        /// </summary>
        [Column("DAY11")]
        [CustomAttribute.DefaultValue(0)]
        public int Day11 { get; set; }

        /// <summary>
        /// 算定日情報12
        /// 
        /// </summary>
        [Column("DAY12")]
        [CustomAttribute.DefaultValue(0)]
        public int Day12 { get; set; }

        /// <summary>
        /// 算定日情報13
        /// 
        /// </summary>
        [Column("DAY13")]
        [CustomAttribute.DefaultValue(0)]
        public int Day13 { get; set; }

        /// <summary>
        /// 算定日情報14
        /// 
        /// </summary>
        [Column("DAY14")]
        [CustomAttribute.DefaultValue(0)]
        public int Day14 { get; set; }

        /// <summary>
        /// 算定日情報15
        /// 
        /// </summary>
        [Column("DAY15")]
        [CustomAttribute.DefaultValue(0)]
        public int Day15 { get; set; }

        /// <summary>
        /// 算定日情報16
        /// 
        /// </summary>
        [Column("DAY16")]
        [CustomAttribute.DefaultValue(0)]
        public int Day16 { get; set; }

        /// <summary>
        /// 算定日情報17
        /// 
        /// </summary>
        [Column("DAY17")]
        [CustomAttribute.DefaultValue(0)]
        public int Day17 { get; set; }

        /// <summary>
        /// 算定日情報18
        /// 
        /// </summary>
        [Column("DAY18")]
        [CustomAttribute.DefaultValue(0)]
        public int Day18 { get; set; }

        /// <summary>
        /// 算定日情報19
        /// 
        /// </summary>
        [Column("DAY19")]
        [CustomAttribute.DefaultValue(0)]
        public int Day19 { get; set; }

        /// <summary>
        /// 算定日情報20
        /// 
        /// </summary>
        [Column("DAY20")]
        [CustomAttribute.DefaultValue(0)]
        public int Day20 { get; set; }

        /// <summary>
        /// 算定日情報21
        /// 
        /// </summary>
        [Column("DAY21")]
        [CustomAttribute.DefaultValue(0)]
        public int Day21 { get; set; }

        /// <summary>
        /// 算定日情報22
        /// 
        /// </summary>
        [Column("DAY22")]
        [CustomAttribute.DefaultValue(0)]
        public int Day22 { get; set; }

        /// <summary>
        /// 算定日情報23
        /// 
        /// </summary>
        [Column("DAY23")]
        [CustomAttribute.DefaultValue(0)]
        public int Day23 { get; set; }

        /// <summary>
        /// 算定日情報24
        /// 
        /// </summary>
        [Column("DAY24")]
        [CustomAttribute.DefaultValue(0)]
        public int Day24 { get; set; }

        /// <summary>
        /// 算定日情報25
        /// 
        /// </summary>
        [Column("DAY25")]
        [CustomAttribute.DefaultValue(0)]
        public int Day25 { get; set; }

        /// <summary>
        /// 算定日情報26
        /// 
        /// </summary>
        [Column("DAY26")]
        [CustomAttribute.DefaultValue(0)]
        public int Day26 { get; set; }

        /// <summary>
        /// 算定日情報27
        /// 
        /// </summary>
        [Column("DAY27")]
        [CustomAttribute.DefaultValue(0)]
        public int Day27 { get; set; }

        /// <summary>
        /// 算定日情報28
        /// 
        /// </summary>
        [Column("DAY28")]
        [CustomAttribute.DefaultValue(0)]
        public int Day28 { get; set; }

        /// <summary>
        /// 算定日情報29
        /// 
        /// </summary>
        [Column("DAY29")]
        [CustomAttribute.DefaultValue(0)]
        public int Day29 { get; set; }

        /// <summary>
        /// 算定日情報30
        /// 
        /// </summary>
        [Column("DAY30")]
        [CustomAttribute.DefaultValue(0)]
        public int Day30 { get; set; }

        /// <summary>
        /// 算定日情報31
        /// 
        /// </summary>
        [Column("DAY31")]
        [CustomAttribute.DefaultValue(0)]
        public int Day31 { get; set; }

        /// <summary>
        /// 削除区分
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        //[Index("SIN_KOUI_IDX03", 4)]
        public int IsDeleted { get; set; }


        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;

    }
}
