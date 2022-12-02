using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "WRK_SIN_KOUI_DETAIL")]
    public class WrkSinKouiDetail : EmrCloneable<WrkSinKouiDetail>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        //[Index("WRK_SIN_KOUI_DETAIL_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("PT_ID")]
        //[Index("WRK_SIN_KOUI_DETAIL_IDX01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("SIN_DATE")]
        //[Index("WRK_SIN_KOUI_DETAIL_IDX01", 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        //[Key]
        [Column("RAIIN_NO", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        //[Key]
        [Column("HOKEN_KBN", Order = 3)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 剤番号
        /// WRK_SIN_KOUI.RP_NO
        /// </summary>
        //[Key]
        [Column("RP_NO", Order = 4)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// WRK_SIN_KOUI.SEQ_NO
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 5)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 行番号
        /// 
        /// </summary>
        //[Key]
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
        /// 点数識別
        /// "1: 金額（整数部7桁、小数部2桁）
        /// 2: 都道府県購入価格
        /// 3: 点数（プラス）
        /// 4: 都道府県購入価格（点数）、金額（整数部のみ）
        /// 5: %加算
        /// 6: %減算
        /// 7: 減点診療行為
        /// 8: 点数（マイナス）
        /// 9: 乗算割合
        /// 10: 除算金額（金額を10で除す。） ※ベントナイト用
        /// 11: 乗算金額（金額を10で乗ずる。） ※ステミラック注用"
        /// </summary>
        [Column("TEN_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int TenId { get; set; }

        /// <summary>
        /// 点数
        /// 
        /// </summary>
        [Column("TEN")]
        [CustomAttribute.DefaultValue(0)]
        public double Ten { get; set; }

        /// <summary>
        /// コード表用区分－区分
        /// "当該診療行為について医科点数表の章、部、区分番号及び項番を記録する。
        /// 区分（アルファベット部）：
        /// 　点数表の区分番号のアルファベット部を記録する。
        /// 　なお、介護老人保健施設入所者に係る診療料、医療観察法、入院時食事療養、入院時生活療養及び標準負担額については
        /// 　「－」（ハイホン）を、点数表に区分設定がないものは「＊」を記録する。
        /// 
        /// 章：
        /// 部：
        /// 区分番号：
        /// 枝番：
        /// 項番："
        /// </summary>
        [Column("CD_KBN")]
        [MaxLength(1)]
        public string? CdKbn { get; set; } = string.Empty;

        /// <summary>
        /// コード表用区分－区分番号
        /// コード表用区分－区分を参照。
        /// </summary>
        [Column("CD_KBNNO")]
        [CustomAttribute.DefaultValue(0)]
        public int CdKbnno { get; set; }

        /// <summary>
        /// コード表用区分－区分番号－枝番
        /// コード表用区分－区分を参照。
        /// </summary>
        [Column("CD_EDANO")]
        [CustomAttribute.DefaultValue(0)]
        public int CdEdano { get; set; }

        /// <summary>
        /// コード表用区分－項番
        /// コード表用区分－区分を参照。
        /// </summary>
        [Column("CD_KOUNO")]
        [CustomAttribute.DefaultValue(0)]
        public int CdKouno { get; set; }

        /// <summary>
        /// 告示等識別区分１
        /// 
        /// </summary>
        [Column("KOKUJI1")]
        [MaxLength(1)]
        public string? Kokuji1 { get; set; } = string.Empty;

        /// <summary>
        /// 告示等識別区分２
        /// 
        /// </summary>
        [Column("KOKUJI2")]
        [MaxLength(1)]
        public string? Kokuji2 { get; set; } = string.Empty;

        /// <summary>
        /// 注加算コード
        /// 
        /// </summary>
        [Column("TYU_CD")]
        [MaxLength(4)]
        public string? TyuCd { get; set; } = string.Empty;

        /// <summary>
        /// 注加算通番
        /// 
        /// </summary>
        [Column("TYU_SEQ")]
        [MaxLength(1)]
        public string? TyuSeq { get; set; } = string.Empty;

        /// <summary>
        /// 通則年齢加算
        /// </summary>
        [Column("TUSOKU_AGE")]
        [CustomAttribute.DefaultValue(0)]
        public int TusokuAge { get; set; }
        /// <summary>
        /// 項目連番
        /// "コメント以外の項目で、オーダー順
        /// コメントの場合、付随する項目と同じ番号"
        /// </summary>
        [Column("ITEM_SEQ_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int ItemSeqNo { get; set; }

        /// <summary>
        /// 項目枝番
        /// ITEM_SEQ_NO内の連番（オーダー順）
        /// </summary>
        [Column("ITEM_EDA_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int ItemEdaNo { get; set; }

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
        /// 自動発生項目
        /// 1:自動発生項目
        /// </summary>
        [Column("IS_AUTO_ADD")]
        [CustomAttribute.DefaultValue(0)]
        public int IsAutoAdd { get; set; }

        /// <summary>
        /// コメント文
        /// 
        /// </summary>
        [Column("CMT_OPT")]
        [MaxLength(160)]
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
        [MaxLength(160)]
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
        [MaxLength(160)]
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
        [MaxLength(160)]
        public string? CmtOpt3 { get; set; } = string.Empty;

        /// <summary>
        /// 削除フラグ
        ///     1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }
    }
}
