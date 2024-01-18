using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "wrk_sin_koui_detail_del")]
    public class WrkSinKouiDetailDel : EmrCloneable<WrkSinKouiDetailDel>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("sin_date")]
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
        /// WRK_SIN_KOUI_DETAIL.RP_NO
        /// </summary>
        
        [Column("rp_no", Order = 4)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// WRK_SIN_KOUI_DETAIL.SEQ_NO
        /// </summary>
        
        [Column("seq_no", Order = 5)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 行番号
        /// WRK_SIN_KOUI_DETAIL.ROW_NO
        /// </summary>
        
        [Column("row_no", Order = 6)]
        public int RowNo { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Column("item_cd")]
        [MaxLength(10)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 項目連番
        /// 同一WRK_CALC_NO,RP_NO,ROW_NO内の連番
        /// </summary>
        
        [Column("item_seq_no", Order = 7)]
        [CustomAttribute.DefaultValue(1)]
        public int ItemSeqNo { get; set; }

        /// <summary>
        /// 削除項目コード
        /// 当該項目が削除される理由となった項目のITEM_CD
        /// 9999999999の場合、付随して削除になった項目
        /// </summary>
        [Column("del_item_cd")]
        [MaxLength(10)]
        public string? DelItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 削除項目算定日
        /// 削除項目の算定日
        /// 0の場合、当来院
        /// </summary>
        [Column("santei_date")]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiDate { get; set; }

        /// <summary>
        /// 削除種別
        /// 0:包括 1:背反
        /// </summary>
        [Column("del_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int DelSbt { get; set; }

        /// <summary>
        /// 警告
        /// 0:削除 1:警告
        /// </summary>
        [Column("is_warning")]
        [CustomAttribute.DefaultValue(0)]
        public int IsWarning { get; set; }

        /// <summary>
        /// チェック期間数
        /// TERM_SBTと組み合わせて使用
        /// ※TERM_SBT in (1,4)のときのみ有効
        /// 例）2日の場合、TERM_CNT=2, TERM_SBT=1と登録
        /// </summary>
        [Column("term_cnt")]
        [CustomAttribute.DefaultValue(0)]
        public int TermCnt { get; set; }

        /// <summary>
        /// チェック期間種別
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        [Column("term_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int TermSbt { get; set; }


        /// <summary>
        /// コード表用区分－区分
        ///     当該診療行為について医科点数表の章、部、区分番号及び項番を記録する。
        ///             区分（アルファベット部）：
        ///     　点数表の区分番号のアルファベット部を記録する。
        ///     　なお、介護老人保健施設入所者に係る診療料、医療観察法、入院時食事療養、入院時生活療養及び標準負担額については
        ///     　「－」（ハイホン）を、点数表に区分設定がないものは「＊」を記録する。
        ///     章：
        ///     部：
        ///     区分番号：
        ///     枝番：
        ///     項番：
        /// </summary>
        [Column("cd_kbn")]
        [MaxLength(1)]
        public string CdKbn { get; set; }

        /// <summary>
        /// コード表用区分－区分番号
        ///     コード表用区分－区分を参照。
        /// </summary>
        [Column("cd_kbnno")]
        [CustomAttribute.DefaultValue(0)]
        public int CdKbnno { get; set; }

        /// <summary>
        /// コード表用区分－区分番号－枝番
        ///     コード表用区分－区分を参照。
        /// </summary>
        [Column("cd_edano")]
        [CustomAttribute.DefaultValue(0)]
        public int CdEdano { get; set; }

        /// <summary>
        /// コード表用区分－項番
        ///     コード表用区分－区分を参照。
        /// </summary>
        [Column("cd_kouno")]
        [CustomAttribute.DefaultValue(0)]
        public int CdKouno { get; set; }

        /// <summary>
        /// 告示等識別区分（１）
        ///     当該診療行為についてコンピューター運用上の取扱い（磁気媒体に記録する際の取扱い）を表す。
        ///     　1: 基本項目（告示）　※基本項目
        ///     　3: 合成項目　　　　　※基本項目
        ///     　5: 準用項目（通知）　※基本項目
        ///     　7: 加算項目　　　　　※加算項目
        ///     　9: 通則加算項目　　　※加算項目
        ///       0: 診療行為以外（薬剤、特材等）
        ///       A: 入院基本料労災乗数項目又は四肢加算（手術）項目
        /// </summary>
        [Column("kokuji1")]
        [CustomAttribute.DefaultValue("0")]
        [MaxLength(1)]
        public string Kokuji1 { get; set; }

        /// <summary>
        /// 告示等識別区分（２）
        ///     当該診療行為について点数表上の取扱いを表す。
        ///     　1: 基本項目（告示）
        ///     　3: 合成項目
        ///     （削）5: 準用項目（通知）
        ///     　7: 加算項目（告示）
        ///     （削）9: 通則加算項目
        ///       0: 診療行為以外（薬剤、特材等）
        /// </summary>
        [Column("kokuji2")]
        [CustomAttribute.DefaultValue("0")]
        [MaxLength(1)]
        public string Kokuji2 { get; set; }
    }
}
