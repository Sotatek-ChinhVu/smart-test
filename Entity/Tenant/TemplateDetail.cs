using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Serializable()]
    [Table(name: "TEMPLATE_DETAIL")]
    public class TemplateDetail : EmrCloneable<TemplateDetail>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        //[Index("TEMPLATE_DETAIL_PKEY", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// テンプレートコード
        /// 
        /// </summary>
        //[Key]
        [Column("TEMPLATE_CD", Order = 2)]
        //[Index("TEMPLATE_DETAIL_PKEY", 2)]
        public int TemplateCd { get; set; } 

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Index("TEMPLATE_DETAIL_PKEY", 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// コントロールID
        /// 
        /// </summary>
        //[Key]
        [Column("CONTROL_ID", Order = 4)]
        //[Index("TEMPLATE_DETAIL_PKEY", 4)]
        public int ControlId { get; set; }

        /// <summary>
        /// 親ＩＤ
        /// 
        /// </summary>
        [Column("OYA_CONTROL_ID")]
        public int? OyaControlId { get; set; }

        /// <summary>
        /// タイトル
        /// 
        /// </summary>
        [Column("TITLE")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// タイプ
        /// "0:タイトルラベル
        /// 1:見出しラベル
        /// 2:Edit
        /// 3:Combo
        /// 4:Check
        /// 5:Group"
        /// </summary>
        [Column("CONTROL_TYPE")]
        public int ControlType { get; set; }

        /// <summary>
        /// 選択肢区分
        /// 
        /// </summary>
        [Column("MENU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int MenuKbn { get; set; }

        /// <summary>
        /// 初期値
        /// 
        /// </summary>
        [Column("DEFAULT_VAL")]
        [MaxLength(200)]
        public string DefaultVal { get; set; } = string.Empty;

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        [Column("UNIT")]
        [MaxLength(20)]
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// 改行フラグ
        /// 1:改行
        /// </summary>
        [Column("NEW_LINE")]
        [CustomAttribute.DefaultValue(0)]
        public int NewLine { get; set; }

        /// <summary>
        /// 挿入先
        /// "0:未指定
        /// >0: KARTE_KBN_MST.KARTE_KBN"
        /// </summary>
        [Column("KARTE_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKbn { get; set; }

        /// <summary>
        /// 幅
        /// 
        /// </summary>
        [Column("CONTROL_WIDTH")]
        [CustomAttribute.DefaultValue(0)]
        public int ControlWidth { get; set; }

        /// <summary>
        /// タイトル幅
        /// 
        /// </summary>
        [Column("TITLE_WIDTH")]
        [CustomAttribute.DefaultValue(0)]
        public int TitleWidth { get; set; }

        /// <summary>
        /// 単位幅
        /// 
        /// </summary>
        [Column("UNIT_WIDTH")]
        [CustomAttribute.DefaultValue(0)]
        public int UnitWidth { get; set; }

        /// <summary>
        /// 左余白
        /// 
        /// </summary>
        [Column("LEFT_MARGIN")]
        [CustomAttribute.DefaultValue(0)]
        public int LeftMargin { get; set; }

        /// <summary>
        /// 折り返し
        /// 1:折り返し
        /// </summary>
        [Column("WORDWRAP")]
        [CustomAttribute.DefaultValue(0)]
        public int Wordwrap { get; set; }

        /// <summary>
        /// 値
        /// </summary>
        [Column("VAL")]
        [CustomAttribute.DefaultValue(0)]
        public double? Val { get; set; }

        /// <summary>
        /// 式
        /// 
        /// </summary>
        [Column("FORMULA")]
        [MaxLength(200)]
        public string Formula { get; set; } = string.Empty;

        /// <summary>
        /// 小数桁
        /// 
        /// </summary>
        [Column("DECIMAL")]
        [CustomAttribute.DefaultValue(0)]
        public int Decimal { get; set; }

        /// <summary>
        /// ＩＭＥ
        /// "1:Close 
        /// 2:半角カナ
        /// 3:全角ひらがな
        /// その他:Default"
        /// </summary>
        [Column("IME")]
        [CustomAttribute.DefaultValue(0)]
        public int Ime { get; set; }

        /// <summary>
        /// 列数
        /// 
        /// </summary>
        [Column("COL_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int ColCount { get; set; }

        /// <summary>
        /// 連携コード
        /// 
        /// </summary>
        [Column("RENKEI_CD")]
        [MaxLength(20)]
        public string RenkeiCd { get; set; } = string.Empty;

        /// <summary>
        /// 背景色
        /// ? カラーコード 初期値は白
        /// </summary>
        [Column("BACKGROUND_COLOR")]
        [MaxLength(8)]
        public string BackgroundColor { get; set; } = string.Empty;

        /// <summary>
        /// 文字色
        /// ? カラーコード 初期値は黒
        /// </summary>
        [Column("FONT_COLOR")]
        [MaxLength(8)]
        public string FontColor { get; set; } = string.Empty;

        /// <summary>
        /// 太字
        /// 1:太字
        /// </summary>
        [Column("FONT_BOLD")]
        [CustomAttribute.DefaultValue(0)]
        public int FontBold { get; set; }

        /// <summary>
        /// 斜字
        /// 1:斜字
        /// </summary>
        [Column("FONT_ITALIC")]
        [CustomAttribute.DefaultValue(0)]
        public int FontItalic { get; set; }

        /// <summary>
        /// 下線
        /// 1:下線
        /// </summary>
        [Column("FONT_UNDER_LINE")]
        [CustomAttribute.DefaultValue(0)]
        public int FontUnderLine { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
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
        /// 更新者
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
