using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "PT_TAG")]
    [Index(nameof(HpId), nameof(PtId), nameof(StartDate), nameof(EndDate), nameof(IsDspUketuke), nameof(IsDspKarte), nameof(IsDspKaikei), nameof(IsDeleted), Name = "PT_TAG_IDX01")]
    public class PtTag : EmrCloneable<PtTag>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        //[Key]
        [Column(name:"PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column(name: "SEQ_NO", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// メモ
        /// 
        /// </summary>
        [Column(name: "MEMO")]
        public string Memo { get; set; } = string.Empty;

        /// <summary>
        /// メモデータ
        /// 
        /// </summary>
        [Column(name: "MEMO_DATA")]
        public byte[] MemoData { get; set; } = default!;

        /// <summary>
        /// 適用開始日
        /// 
        /// </summary>
        [Column(name: "START_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        /// 
        /// </summary>
        [Column(name: "END_DATE")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 受付表示区分
        /// 1: 表示
        /// </summary>
        [Column(name: "IS_DSP_UKETUKE")]
        [CustomAttribute.DefaultValue(1)]
        public int IsDspUketuke { get; set; }

        /// <summary>
        /// 診察表示区分
        /// 1: 表示
        /// </summary>
        [Column(name: "IS_DSP_KARTE")]
        [CustomAttribute.DefaultValue(1)]
        public int IsDspKarte { get; set; }

        /// <summary>
        /// 会計表示区分
        /// 1: 表示
        /// </summary>
        [Column(name: "IS_DSP_KAIKEI")]
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
        public string BackgroundColor { get; set; } = string.Empty;


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
        /// フォントサイズ
        /// </summary>
        [Column(name: "FONTSIZE")]
        [CustomAttribute.DefaultValue(0)]
        public int FontSize { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column(name: "IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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

    }
}
