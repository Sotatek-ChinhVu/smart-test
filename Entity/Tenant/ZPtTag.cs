using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "Z_PT_TAG")]
    public class ZPtTag : EmrCloneable<ZPtTag>
    {
        [Key]
        [Column("OP_ID", Order = 1)]
        public long OpId { get; set; }

        [Column("OP_TYPE")]
        [MaxLength(10)]
        public string? OpType { get; set; } = string.Empty;

        [Column("OP_TIME")]
        public DateTime OpTime { get; set; }

        [Column("OP_ADDR")]
        [MaxLength(100)]
        public string? OpAddr { get; set; } = string.Empty;

        [Column("OP_HOSTNAME")]
        [MaxLength(100)]
        public string? OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("HP_ID")]
        //[Index("PT_TAG_IDX01", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column(name: "PT_ID")]
        //[Index("PT_TAG_IDX01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column(name: "SEQ_NO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// メモ
        /// 
        /// </summary>
        [Column(name: "MEMO")]
        public string? Memo { get; set; } = string.Empty;

        /// <summary>
        /// メモデータ
        /// 
        /// </summary>
        [Column(name: "MEMO_DATA")]
        public byte[]? MemoData { get; set; } = default!;

        /// <summary>
        /// 適用開始日
        /// 
        /// </summary>
        [Column(name: "START_DATE")]
        //[Index("PT_TAG_IDX01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        /// 
        /// </summary>
        [Column(name: "END_DATE")]
        //[Index("PT_TAG_IDX01", 4)]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 受付表示区分
        /// 1: 表示
        /// </summary>
        [Column(name: "IS_DSP_UKETUKE")]
        //[Index("PT_TAG_IDX01", 5)]
        [CustomAttribute.DefaultValue(1)]
        public int IsDspUketuke { get; set; }

        /// <summary>
        /// 診察表示区分
        /// 1: 表示
        /// </summary>
        [Column(name: "IS_DSP_KARTE")]
        //[Index("PT_TAG_IDX01", 6)]
        [CustomAttribute.DefaultValue(1)]
        public int IsDspKarte { get; set; }

        /// <summary>
        /// 会計表示区分
        /// 1: 表示
        /// </summary>
        [Column(name: "IS_DSP_KAIKEI")]
        //[Index("PT_TAG_IDX01", 7)]
        [CustomAttribute.DefaultValue(1)]
        public int IsDspKaikei { get; set; }

        /// <summary>
        /// レセ表示区分
        /// 1: 表示
        /// </summary>
        [Column(name: "IS_DSP_RECE")]
        [CustomAttribute.DefaultValue(1)]
        public int IsDspRece { get; set; }

        /// <summary>
        /// 背景色
        /// 
        /// </summary>
        [Column(name: "BACKGROUND_COLOR")]
        [MaxLength(8)]
        public string? BackgroundColor { get; set; } = string.Empty;

        [Column(name: "TAG_GRP_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int TagGrpCd { get; set; }

        /// <summary>
        /// 透明度
        /// 
        /// </summary>
        [Column(name: "ALPHABLEND_VAL")]
        [CustomAttribute.DefaultValue(200)]
        public int AlphablendVal { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column(name: "IS_DELETED")]
        //[Index("PT_TAG_IDX01", 8)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column(name: "CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column(name: "UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

        /// <summary>
        /// フォントサイズ
        /// </summary>
        [Column(name: "FONTSIZE")]
        [CustomAttribute.DefaultValue(0)]
        public int FontSize { get; set; }

        /// <summary>
        /// 幅
        /// </summary>
        [Column(name: "WIDTH")]
        [CustomAttribute.DefaultValue(0)]
        public int Width { get; set; }

        /// <summary>
        /// 高さ
        /// </summary>
        [Column(name: "HEIGHT")]
        [CustomAttribute.DefaultValue(0)]
        public int Height { get; set; }

        /// <summary>
        /// 左位置
        /// </summary>
        [Column(name: "LEFT")]
        [CustomAttribute.DefaultValue(0)]
        public int Left { get; set; }

        /// <summary>
        /// 右位置
        /// </summary>
        [Column(name: "TOP")]
        [CustomAttribute.DefaultValue(0)]
        public int Top { get; set; }
    }
}
