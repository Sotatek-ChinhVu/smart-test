using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "WRK_SIN_KOUI_DETAIL_DEL")]
    public class WrkSinKouiDetailDel : EmrCloneable<WrkSinKouiDetailDel>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("PT_ID")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("SIN_DATE")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        
        [Column("RAIIN_NO", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 保険区分
        /// 0:健保 1:労災 2:アフターケア 3:自賠 4:自費
        /// </summary>
        
        [Column("HOKEN_KBN", Order = 3)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 剤番号
        /// WRK_SIN_KOUI_DETAIL.RP_NO
        /// </summary>
        
        [Column("RP_NO", Order = 4)]
        public int RpNo { get; set; }

        /// <summary>
        /// 連番
        /// WRK_SIN_KOUI_DETAIL.SEQ_NO
        /// </summary>
        
        [Column("SEQ_NO", Order = 5)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 行番号
        /// WRK_SIN_KOUI_DETAIL.ROW_NO
        /// </summary>
        
        [Column("ROW_NO", Order = 6)]
        public int RowNo { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Column("ITEM_CD")]
        [MaxLength(10)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 項目連番
        /// 同一WRK_CALC_NO,RP_NO,ROW_NO内の連番
        /// </summary>
        
        [Column("ITEM_SEQ_NO", Order = 7)]
        [CustomAttribute.DefaultValue(1)]
        public int ItemSeqNo { get; set; }

        /// <summary>
        /// 削除項目コード
        /// 当該項目が削除される理由となった項目のITEM_CD
        /// 9999999999の場合、付随して削除になった項目
        /// </summary>
        [Column("DEL_ITEM_CD")]
        [MaxLength(10)]
        public string? DelItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 削除項目算定日
        /// 削除項目の算定日
        /// 0の場合、当来院
        /// </summary>
        [Column("SANTEI_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiDate { get; set; }

        /// <summary>
        /// 削除種別
        /// 0:包括 1:背反
        /// </summary>
        [Column("DEL_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int DelSbt { get; set; }

        /// <summary>
        /// 警告
        /// 0:削除 1:警告
        /// </summary>
        [Column("IS_WARNING")]
        [CustomAttribute.DefaultValue(0)]
        public int IsWarning { get; set; }

        /// <summary>
        /// チェック期間数
        /// TERM_SBTと組み合わせて使用
        /// ※TERM_SBT in (1,4)のときのみ有効
        /// 例）2日の場合、TERM_CNT=2, TERM_SBT=1と登録
        /// </summary>
        [Column("TERM_CNT")]
        [CustomAttribute.DefaultValue(0)]
        public int TermCnt { get; set; }

        /// <summary>
        /// チェック期間種別
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        [Column("TERM_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int TermSbt { get; set; }
    }
}
