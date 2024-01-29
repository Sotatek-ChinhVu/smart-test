using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "wrk_sin_koui_detail")]
    public class WrkSinKouiDetail : EmrCloneable<WrkSinKouiDetail>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        //[Index("wrk_sin_koui_detail_idx01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("pt_id")]
        //[Index("wrk_sin_koui_detail_idx01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("sin_date")]
        //[Index("wrk_sin_koui_detail_idx01", 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        
        [Column("raiin_no", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        
        [Column("hoken_kbn", Order = 3)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 剤番号
        /// WRK_SIN_KOUI.RP_NO
        /// </summary>
        
        [Column("rp_no", Order = 4)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// WRK_SIN_KOUI.SEQ_NO
        /// </summary>
        
        [Column("seq_no", Order = 5)]
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
        [Column("ten_id")]
        [CustomAttribute.DefaultValue(0)]
        public int TenId { get; set; }

        /// <summary>
        /// 点数
        /// 
        /// </summary>
        [Column("ten")]
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
        [Column("cd_kbn")]
        [MaxLength(1)]
        public string? CdKbn { get; set; } = string.Empty;

        /// <summary>
        /// コード表用区分－区分番号
        /// コード表用区分－区分を参照。
        /// </summary>
        [Column("cd_kbnno")]
        [CustomAttribute.DefaultValue(0)]
        public int CdKbnno { get; set; }

        /// <summary>
        /// コード表用区分－区分番号－枝番
        /// コード表用区分－区分を参照。
        /// </summary>
        [Column("cd_edano")]
        [CustomAttribute.DefaultValue(0)]
        public int CdEdano { get; set; }

        /// <summary>
        /// コード表用区分－項番
        /// コード表用区分－区分を参照。
        /// </summary>
        [Column("cd_kouno")]
        [CustomAttribute.DefaultValue(0)]
        public int CdKouno { get; set; }

        /// <summary>
        /// 告示等識別区分１
        /// 
        /// </summary>
        [Column("kokuji1")]
        [MaxLength(1)]
        public string? Kokuji1 { get; set; } = string.Empty;

        /// <summary>
        /// 告示等識別区分２
        /// 
        /// </summary>
        [Column("kokuji2")]
        [MaxLength(1)]
        public string? Kokuji2 { get; set; } = string.Empty;

        /// <summary>
        /// 注加算コード
        /// 
        /// </summary>
        [Column("tyu_cd")]
        [MaxLength(4)]
        public string? TyuCd { get; set; } = string.Empty;

        /// <summary>
        /// 注加算通番
        /// 
        /// </summary>
        [Column("tyu_seq")]
        [MaxLength(1)]
        public string? TyuSeq { get; set; } = string.Empty;

        /// <summary>
        /// 通則年齢加算
        /// </summary>
        [Column("tusoku_age")]
        [CustomAttribute.DefaultValue(0)]
        public int TusokuAge { get; set; }
        /// <summary>
        /// 項目連番
        /// "コメント以外の項目で、オーダー順
        /// コメントの場合、付随する項目と同じ番号"
        /// </summary>
        [Column("item_seq_no")]
        [CustomAttribute.DefaultValue(0)]
        public int ItemSeqNo { get; set; }

        /// <summary>
        /// 項目枝番
        /// ITEM_SEQ_NO内の連番（オーダー順）
        /// </summary>
        [Column("item_eda_no")]
        [CustomAttribute.DefaultValue(0)]
        public int ItemEdaNo { get; set; }

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
        /// 自動発生項目
        /// 1:自動発生項目
        /// </summary>
        [Column("is_auto_add")]
        [CustomAttribute.DefaultValue(0)]
        public int IsAutoAdd { get; set; }

        /// <summary>
        /// コメント文
        /// 
        /// </summary>
        [Column("cmt_opt")]
        [MaxLength(160)]
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
        [MaxLength(160)]
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
        [MaxLength(160)]
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
        [MaxLength(160)]
        public string? CmtOpt3 { get; set; } = string.Empty;

        /// <summary>
        /// 削除フラグ
        ///     1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }
    }
}
