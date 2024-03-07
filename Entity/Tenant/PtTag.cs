using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "pt_tag")]
    [Index(nameof(HpId), nameof(PtId), nameof(StartDate), nameof(EndDate), nameof(IsDspUketuke), nameof(IsDspKarte), nameof(IsDspKaikei), nameof(IsDeleted), Name = "pt_tag_idx01")]
    public class PtTag : EmrCloneable<PtTag>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        
        [Column(name: "pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column(name: "seq_no", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// メモ
        /// 
        /// </summary>
        [Column(name: "memo")]
        public string? Memo { get; set; } = string.Empty;

        /// <summary>
        /// メモデータ
        /// 
        /// </summary>
        [Column(name: "memo_data")]
        public byte[]? MemoData { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// 適用開始日
        /// 
        /// </summary>
        [Column(name: "start_date")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        /// 
        /// </summary>
        [Column(name: "end_date")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 受付表示区分
        /// 1: 表示
        /// </summary>
        [Column(name: "is_dsp_uketuke")]
        [CustomAttribute.DefaultValue(1)]
        public int IsDspUketuke { get; set; }

        /// <summary>
        /// 診察表示区分
        /// 1: 表示
        /// </summary>
        [Column(name: "is_dsp_karte")]
        [CustomAttribute.DefaultValue(1)]
        public int IsDspKarte { get; set; }

        /// <summary>
        /// 会計表示区分
        /// 1: 表示
        /// </summary>
        [Column(name: "is_dsp_kaikei")]
        [CustomAttribute.DefaultValue(1)]
        public int IsDspKaikei { get; set; }

        /// <summary>
        /// レセ表示区分
        /// 1: 表示
        /// </summary>
        [Column(name: "is_dsp_rece")]
        [CustomAttribute.DefaultValue(1)]
        public int IsDspRece { get; set; }

        /// <summary>
        /// 背景色
        /// 
        /// </summary>
        [Column(name: "background_color")]
        [MaxLength(8)]
        public string? BackgroundColor { get; set; } = string.Empty;


        [Column(name: "tag_grp_cd")]
        [CustomAttribute.DefaultValue(0)]
        public int TagGrpCd { get; set; }

        /// <summary>
        /// 透明度
        /// 
        /// </summary>
        [Column(name: "alphablend_val")]
        [CustomAttribute.DefaultValue(200)]
        public int AlphablendVal { get; set; }

        /// <summary>
        /// フォントサイズ
        /// </summary>
        [Column(name: "fontsize")]
        [CustomAttribute.DefaultValue(0)]
        public int FontSize { get; set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        [Column(name: "is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 幅
        /// </summary>
        [Column(name: "width")]
        [CustomAttribute.DefaultValue(0)]
        public int Width { get; set; }

        /// <summary>
        /// 高さ
        /// </summary>
        [Column(name: "height")]
        [CustomAttribute.DefaultValue(0)]
        public int Height { get; set; }

        /// <summary>
        /// 左位置
        /// </summary>
        [Column(name: "left")]
        [CustomAttribute.DefaultValue(0)]
        public int Left { get; set; }

        /// <summary>
        /// 右位置
        /// </summary>
        [Column(name: "top")]
        [CustomAttribute.DefaultValue(0)]
        public int Top { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column(name: "create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column(name: "update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
