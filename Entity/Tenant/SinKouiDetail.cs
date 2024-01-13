using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "sin_koui_detail")]
    public class SinKouiDetail : EmrCloneable<SinKouiDetail>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        //[Index("sin_koui_detail_idx01", 1)]
        //[Index("sin_koui_detail_idx02", 1)]
        //[Index("sin_koui_detail_idx03", 1)]
        //[Index("sin_koui_detail_idx04", 1)]
        //[Index("sin_koui_detail_idx05", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        //[Index("sin_koui_detail_idx01", 2)]
        //[Index("sin_koui_detail_idx02", 2)]
        //[Index("sin_koui_detail_idx03", 2)]
        //[Index("sin_koui_detail_idx04", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("sin_ym", Order = 3)]
        //[Index("sin_koui_detail_idx01", 3)]
        //[Index("sin_koui_detail_idx02", 3)]
        //[Index("sin_koui_detail_idx03", 3)]
        //[Index("sin_koui_detail_idx04", 3)]
        //[Index("sin_koui_detail_idx05", 2)]
        public int SinYm { get; set; }

        /// <summary>
        /// 剤番号
        /// SIN_KOUI.RP_NO
        /// </summary>
        
        [Column("rp_no", Order = 4)]
        //[Index("sin_koui_detail_idx02", 4)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// SIN_KOUI.SEQ_NO
        /// </summary>
        
        [Column("seq_no", Order = 5)]
        //[Index("sin_koui_detail_idx02", 5)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 行番号
        /// 
        /// </summary>
        
        [Column("row_no", Order = 6)]
        public int RowNo { get; set; }

        /// <summary>
        /// レコード識別
        /// レセプト電算に記録するレコード識別
        /// </summary>
        [Column("rec_id")]
        [MaxLength(2)]
        public string? RecId { get; set; } = string.Empty;

        /// <summary>
        /// 項目種別
        /// 1:コメント
        /// </summary>
        [Column("item_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int ItemSbt { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Column("item_cd")]
        [MaxLength(10)]
        //[Index("sin_koui_detail_idx04", 4)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// オーダー項目コード
        /// 
        /// </summary>
        [Column("odr_item_cd")]
        [MaxLength(10)]
        public string? OdrItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 項目名称
        /// 
        /// </summary>
        [Column("item_name")]
        [MaxLength(1000)]
        public string? ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 数量
        /// 
        /// </summary>
        [Column("suryo")]
        [CustomAttribute.DefaultValue(0)]
        public double Suryo { get; set; }

        /// <summary>
        /// 数量２
        /// レセ電にのみ記載する数量（分、ｃｍ２など）
        /// </summary>
        [Column("suryo2")]
        [CustomAttribute.DefaultValue(0)]
        public double Suryo2 { get; set; }

        /// <summary>
        /// 書式区分
        /// 1: 列挙対象項目
        /// </summary>
        [Column("fmt_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int FmtKbn { get; set; }

        /// <summary>
        /// 単位コード
        /// 
        /// </summary>
        [Column("unit_cd")]
        [CustomAttribute.DefaultValue(0)]
        public int UnitCd { get; set; }

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        [Column("unit_name")]
        [MaxLength(20)]
        public string? UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 点数
        /// 当該項目の点数。金額項目の場合、10で割ったものを記録
        /// </summary>
        [Column("ten")]
        [CustomAttribute.DefaultValue(0)]
        public double Ten { get; set; }

        /// <summary>
        /// 消費税
        /// "自費の場合の税金分
        /// TEN+ZEIになるようにする
        /// 内税項目の場合は、単価/(1+税率)*税率
        /// 単価-消費税をTENとする"
        /// </summary>
        [Column("zei")]
        [CustomAttribute.DefaultValue(0)]
        public double Zei { get; set; }

        /// <summary>
        /// レセ非表示区分
        /// "1:非表示
        /// 2:電算のみ非表示"
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
        /// 領収証非表示区分
        /// 1:非表示
        /// </summary>
        [Column("is_nodsp_ryosyu")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspRyosyu { get; set; }

        /// <summary>
        /// コメント文
        /// 
        /// </summary>
        [Column("cmt_opt")]
        [MaxLength(240)]
        public string? CmtOpt { get; set; } = string.Empty;

        /// <summary>
        /// コメント１
        /// コメントコード１とコメント文１から成り立つ表示用文字列
        /// </summary>
        [Column("cmt1")]
        [MaxLength(1000)]
        public string? Cmt1 { get; set; } = string.Empty;

        /// <summary>
        /// コメントコード１
        /// 
        /// </summary>
        [Column("cmt_cd1")]
        [MaxLength(10)]
        public string? CmtCd1 { get; set; } = string.Empty;

        /// <summary>
        /// コメント文１
        /// 
        /// </summary>
        [Column("cmt_opt1")]
        [MaxLength(240)]
        public string? CmtOpt1 { get; set; } = string.Empty;

        /// <summary>
        /// コメント２
        /// コメントコード２とコメント文２から成り立つ表示用文字列
        /// </summary>
        [Column("cmt2")]
        [MaxLength(1000)]
        public string? Cmt2 { get; set; } = string.Empty;

        /// <summary>
        /// コメントコード２
        /// 
        /// </summary>
        [Column("cmt_cd2")]
        [MaxLength(10)]
        public string? CmtCd2 { get; set; } = string.Empty;

        /// <summary>
        /// コメント文２
        /// 
        /// </summary>
        [Column("cmt_opt2")]
        [MaxLength(240)]
        public string? CmtOpt2 { get; set; } = string.Empty;

        /// <summary>
        /// コメント３
        /// コメントコード３とコメント文３から成り立つ表示用文字列
        /// </summary>
        [Column("cmt3")]
        [MaxLength(1000)]
        public string? Cmt3 { get; set; } = string.Empty;

        /// <summary>
        /// コメントコード３
        /// 
        /// </summary>
        [Column("cmt_cd3")]
        [MaxLength(10)]
        public string? CmtCd3 { get; set; } = string.Empty;

        /// <summary>
        /// コメント文３
        /// 
        /// </summary>
        [Column("cmt_opt3")]
        [MaxLength(240)]
        public string? CmtOpt3 { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        //[Index("sin_koui_detail_idx03", 4)]
        public int IsDeleted { get; set; }
    }
}
