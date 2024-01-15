using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Serializable()]
    [Table(name: "template_detail")]
    public class TemplateDetail : EmrCloneable<TemplateDetail>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        //[Index("template_detail_pkey", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// テンプレートコード
        /// 
        /// </summary>
        
        [Column("template_cd", Order = 2)]
        //[Index("template_detail_pkey", 2)]
        public int TemplateCd { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Index("template_detail_pkey", 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// コントロールID
        /// 
        /// </summary>
        
        [Column("control_id", Order = 4)]
        //[Index("template_detail_pkey", 4)]
        public int ControlId { get; set; }

        /// <summary>
        /// 親ＩＤ
        /// 
        /// </summary>
        [Column("oya_control_id")]
        public int? OyaControlId { get; set; }

        /// <summary>
        /// タイトル
        /// 
        /// </summary>
        [Column("title")]
        [MaxLength(200)]
        public string? Title { get; set; } = string.Empty;

        /// <summary>
        /// タイプ
        /// "0:タイトルラベル
        /// 1:見出しラベル
        /// 2:Edit
        /// 3:Combo
        /// 4:Check
        /// 5:Group"
        /// </summary>
        [Column("control_type")]
        public int ControlType { get; set; }

        /// <summary>
        /// 選択肢区分
        /// 
        /// </summary>
        [Column("menu_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int MenuKbn { get; set; }

        /// <summary>
        /// 初期値
        /// 
        /// </summary>
        [Column("default_val")]
        [MaxLength(200)]
        public string? DefaultVal { get; set; } = string.Empty;

        /// <summary>
        /// 単位名称
        /// 
        /// </summary>
        [Column("unit")]
        [MaxLength(20)]
        public string? Unit { get; set; } = string.Empty;

        /// <summary>
        /// 改行フラグ
        /// 1:改行
        /// </summary>
        [Column("new_line")]
        [CustomAttribute.DefaultValue(0)]
        public int NewLine { get; set; }

        /// <summary>
        /// 挿入先
        /// "0:未指定
        /// >0: KARTE_KBN_MST.KARTE_KBN"
        /// </summary>
        [Column("karte_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKbn { get; set; }

        /// <summary>
        /// 幅
        /// 
        /// </summary>
        [Column("control_width")]
        [CustomAttribute.DefaultValue(0)]
        public int ControlWidth { get; set; }

        /// <summary>
        /// タイトル幅
        /// 
        /// </summary>
        [Column("title_width")]
        [CustomAttribute.DefaultValue(0)]
        public int TitleWidth { get; set; }

        /// <summary>
        /// 単位幅
        /// 
        /// </summary>
        [Column("unit_width")]
        [CustomAttribute.DefaultValue(0)]
        public int UnitWidth { get; set; }

        /// <summary>
        /// 左余白
        /// 
        /// </summary>
        [Column("left_margin")]
        [CustomAttribute.DefaultValue(0)]
        public int LeftMargin { get; set; }

        /// <summary>
        /// 折り返し
        /// 1:折り返し
        /// </summary>
        [Column("wordwrap")]
        [CustomAttribute.DefaultValue(0)]
        public int Wordwrap { get; set; }

        /// <summary>
        /// 値
        /// </summary>
        [Column("val")]
        [CustomAttribute.DefaultValue(0)]
        public double? Val { get; set; }

        /// <summary>
        /// 式
        /// 
        /// </summary>
        [Column("formula")]
        [MaxLength(200)]
        public string? Formula { get; set; } = string.Empty;

        /// <summary>
        /// 小数桁
        /// 
        /// </summary>
        [Column("decimal")]
        [CustomAttribute.DefaultValue(0)]
        public int Decimal { get; set; }

        /// <summary>
        /// ＩＭＥ
        /// "1:Close 
        /// 2:半角カナ
        /// 3:全角ひらがな
        /// その他:Default"
        /// </summary>
        [Column("ime")]
        [CustomAttribute.DefaultValue(0)]
        public int Ime { get; set; }

        /// <summary>
        /// 列数
        /// 
        /// </summary>
        [Column("col_count")]
        [CustomAttribute.DefaultValue(0)]
        public int ColCount { get; set; }

        /// <summary>
        /// 連携コード
        /// 
        /// </summary>
        [Column("renkei_cd")]
        [MaxLength(20)]
        public string? RenkeiCd { get; set; } = string.Empty;

        /// <summary>
        /// 背景色
        /// ? カラーコード 初期値は白
        /// </summary>
        [Column("background_color")]
        [MaxLength(8)]
        public string? BackgroundColor { get; set; } = string.Empty;

        /// <summary>
        /// 文字色
        /// ? カラーコード 初期値は黒
        /// </summary>
        [Column("font_color")]
        [MaxLength(8)]
        public string? FontColor { get; set; } = string.Empty;

        /// <summary>
        /// 太字
        /// 1:太字
        /// </summary>
        [Column("font_bold")]
        [CustomAttribute.DefaultValue(0)]
        public int FontBold { get; set; }

        /// <summary>
        /// 斜字
        /// 1:斜字
        /// </summary>
        [Column("font_italic")]
        [CustomAttribute.DefaultValue(0)]
        public int FontItalic { get; set; }

        /// <summary>
        /// 下線
        /// 1:下線
        /// </summary>
        [Column("font_under_line")]
        [CustomAttribute.DefaultValue(0)]
        public int FontUnderLine { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
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
        /// 更新者
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
    }
}
