using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SIN_KOUI_DETAIL")]
    public class SinKouiDetail : EmrCloneable<SinKouiDetail>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        //[Index("SIN_KOUI_DETAIL_IDX01", 1)]
        //[Index("SIN_KOUI_DETAIL_IDX02", 1)]
        //[Index("SIN_KOUI_DETAIL_IDX03", 1)]
        //[Index("SIN_KOUI_DETAIL_IDX04", 1)]
        //[Index("SIN_KOUI_DETAIL_IDX05", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column("PT_ID", Order = 2)]
        //[Index("SIN_KOUI_DETAIL_IDX01", 2)]
        //[Index("SIN_KOUI_DETAIL_IDX02", 2)]
        //[Index("SIN_KOUI_DETAIL_IDX03", 2)]
        //[Index("SIN_KOUI_DETAIL_IDX04", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        
        [Column("SIN_YM", Order = 3)]
        //[Index("SIN_KOUI_DETAIL_IDX01", 3)]
        //[Index("SIN_KOUI_DETAIL_IDX02", 3)]
        //[Index("SIN_KOUI_DETAIL_IDX03", 3)]
        //[Index("SIN_KOUI_DETAIL_IDX04", 3)]
        //[Index("SIN_KOUI_DETAIL_IDX05", 2)]
        public int SinYm { get; set; }

        /// <summary>
        /// 剤番号
        /// SIN_KOUI.RP_NO
        /// </summary>
        
        [Column("RP_NO", Order = 4)]
        //[Index("SIN_KOUI_DETAIL_IDX02", 4)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// SIN_KOUI.SEQ_NO
        /// </summary>
        
        [Column("SEQ_NO", Order = 5)]
        //[Index("SIN_KOUI_DETAIL_IDX02", 5)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 行番号
        /// 
        /// </summary>
        
        [Column("ROW_NO", Order = 6)]
        public int RowNo { get; set; }

        /// <summary>
        /// レコード識別
        /// レセプト電算に記録するレコード識別
        /// </summary>
        [Column("REC_ID")]
        [MaxLength(2)]
        public string? RecId { get; set; } = string.Empty;

        /// <summary>
        /// 項目種別
        /// 1:コメント
        /// </summary>
        [Column("ITEM_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int ItemSbt { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Column("ITEM_CD")]
        [MaxLength(10)]
        //[Index("SIN_KOUI_DETAIL_IDX04", 4)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// オーダー項目コード
        /// 
        /// </summary>
        [Column("ODR_ITEM_CD")]
        [MaxLength(10)]
        public string? OdrItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 項目名称
        /// 
        /// </summary>
        [Column("ITEM_NAME")]
        [MaxLength(1000)]
        public string? ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 数量
        /// 
        /// </summary>
        [Column("SURYO")]
        [CustomAttribute.DefaultValue(0)]
        public double Suryo { get; set; }

        /// <summary>
        /// 数量２
        /// レセ電にのみ記載する数量（分、ｃｍ２など）
        /// </summary>
        [Column("SURYO2")]
        [CustomAttribute.DefaultValue(0)]
        public double Suryo2 { get; set; }

        /// <summary>
        /// 書式区分
        /// 1: 列挙対象項目
        /// </summary>
        [Column("FMT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int FmtKbn { get; set; }

        /// <summary>
        /// 単位コード
        /// 
        /// </summary>
        [Column("UNIT_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int UnitCd { get; set; }

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        [Column("UNIT_NAME")]
        [MaxLength(20)]
        public string? UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 点数
        /// 当該項目の点数。金額項目の場合、10で割ったものを記録
        /// </summary>
        [Column("TEN")]
        [CustomAttribute.DefaultValue(0)]
        public double Ten { get; set; }

        /// <summary>
        /// 消費税
        /// "自費の場合の税金分
        /// TEN+ZEIになるようにする
        /// 内税項目の場合は、単価/(1+税率)*税率
        /// 単価-消費税をTENとする"
        /// </summary>
        [Column("ZEI")]
        [CustomAttribute.DefaultValue(0)]
        public double Zei { get; set; }

        /// <summary>
        /// レセ非表示区分
        /// "1:非表示
        /// 2:電算のみ非表示"
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
        /// 領収証非表示区分
        /// 1:非表示
        /// </summary>
        [Column("IS_NODSP_RYOSYU")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspRyosyu { get; set; }

        /// <summary>
        /// コメント文
        /// 
        /// </summary>
        [Column("CMT_OPT")]
        [MaxLength(240)]
        public string? CmtOpt { get; set; } = string.Empty;

        /// <summary>
        /// コメント１
        /// コメントコード１とコメント文１から成り立つ表示用文字列
        /// </summary>
        [Column("CMT1")]
        [MaxLength(1000)]
        public string? Cmt1 { get; set; } = string.Empty;

        /// <summary>
        /// コメントコード１
        /// 
        /// </summary>
        [Column("CMT_CD1")]
        [MaxLength(10)]
        public string? CmtCd1 { get; set; } = string.Empty;

        /// <summary>
        /// コメント文１
        /// 
        /// </summary>
        [Column("CMT_OPT1")]
        [MaxLength(240)]
        public string? CmtOpt1 { get; set; } = string.Empty;

        /// <summary>
        /// コメント２
        /// コメントコード２とコメント文２から成り立つ表示用文字列
        /// </summary>
        [Column("CMT2")]
        [MaxLength(1000)]
        public string? Cmt2 { get; set; } = string.Empty;

        /// <summary>
        /// コメントコード２
        /// 
        /// </summary>
        [Column("CMT_CD2")]
        [MaxLength(10)]
        public string? CmtCd2 { get; set; } = string.Empty;

        /// <summary>
        /// コメント文２
        /// 
        /// </summary>
        [Column("CMT_OPT2")]
        [MaxLength(240)]
        public string? CmtOpt2 { get; set; } = string.Empty;

        /// <summary>
        /// コメント３
        /// コメントコード３とコメント文３から成り立つ表示用文字列
        /// </summary>
        [Column("CMT3")]
        [MaxLength(1000)]
        public string? Cmt3 { get; set; } = string.Empty;

        /// <summary>
        /// コメントコード３
        /// 
        /// </summary>
        [Column("CMT_CD3")]
        [MaxLength(10)]
        public string? CmtCd3 { get; set; } = string.Empty;

        /// <summary>
        /// コメント文３
        /// 
        /// </summary>
        [Column("CMT_OPT3")]
        [MaxLength(240)]
        public string? CmtOpt3 { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        //[Index("SIN_KOUI_DETAIL_IDX03", 4)]
        public int IsDeleted { get; set; }
    }
}
