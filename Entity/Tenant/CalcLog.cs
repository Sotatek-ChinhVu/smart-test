using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "CALC_LOG")]
    public class CalcLog : EmrCloneable<CalcLog>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Key]
        [Column("PT_ID", Order = 2)]
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
        [Key]
        [Column("RAIIN_NO", Order = 3)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Key]
        [Column("SEQ_NO", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public int SeqNo { get; set; }

        /// <summary>
        /// ログ種別
        /// 0:通常 1:注意 2:警告
        /// </summary>
        [Column("LOG_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int LogSbt { get; set; }

        /// <summary>
        /// ログ
        /// 
        /// </summary>
        [Column("TEXT")]
        [MaxLength(1000)]
        public string? Text { get; set; } = string.Empty;

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        [Column("HOKEN_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Column("ITEM_CD")]
        [MaxLength(10)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 削除項目コード
        /// 
        /// </summary>
        [Column("DEL_ITEM_CD")]
        [MaxLength(10)]
        public string? DelItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 削除種別
        /// 0:包括
        /// 1:背反
        /// 2:特殊
        /// 3:外来管理加算
        /// 4:優先順背反
        /// 5:注加算
        /// 6:外来管理加算（同一診療）
        /// 7:ある項目が存在しないために算定できない場合
        /// 8:ある項目が存在しないために算定できない場合（警告）
        /// 9:削除項目に付随して削除される項目
        /// 10:注加算項目で、同一Rp内に対応する基本項目がない項目
        /// 11:背反特殊
        /// 12:加算基本項目なし
        /// 13:加算基本項目なし（警告）
        /// 14:自賠・自費以外の保険で自賠文書料を算定
        /// 15:注射手技で薬剤がないため算定できない
        /// 100:算定回数上限
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
        /// ※TERM_SBT in (2,5,6)のときのみ有効
        /// 例）2日の場合、TERM_CNT=2, TERM_SBT=2と登録
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
        public string? UpdateMachine { get; set; }  = string.Empty;



    }
}
