using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "syuno_seikyu")]
    public class SyunoSeikyu : EmrCloneable<SyunoSeikyu>
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
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        
        [Column("sin_date", Order = 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        
        [Column("raiin_no", Order = 4)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 入金区分
        /// 0:未精算 1:一部精算 2:免除 3:精算済
        /// </summary>
        [Column("nyukin_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int NyukinKbn { get; set; }

        /// <summary>
        /// 請求点数
        /// 診療点数（KAIKEI_INF.TENSU）
        /// </summary>
        [Column("seikyu_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuTensu { get; set; }

        /// <summary>
        /// 調整額
        ///     KAIKEI_INF.ADJUST_FUTAN
        /// </summary>
        [Column("adjust_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustFutan { get; set; }

        /// <summary>
        /// 請求額
        /// 請求額 （KAIKEI_INF.TOTAL_PT_FUTAN）
        /// </summary>
        [Column("seikyu_gaku")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuGaku { get; set; }

        /// <summary>
        /// 請求詳細
        /// 診療明細（SIN_KOUI.DETAIL_DATA）
        /// </summary>
        [Column("seikyu_detail")]
        public string? SeikyuDetail { get; set; } = string.Empty;

        /// <summary>
        /// 新請求点数
        ///     KAIKEI_INF.TENSU
        /// </summary>
        [Column("new_seikyu_tensu")]
        [CustomAttribute.DefaultValue(0)]
        public int NewSeikyuTensu { get; set; }

        /// <summary>
        /// 新調整額
        ///     KAIKEI_INF.ADJUST_FUTAN
        /// </summary>
        [Column("new_adjust_futan")]
        [CustomAttribute.DefaultValue(0)]
        public int NewAdjustFutan { get; set; }

        /// <summary>
        /// 新請求額
        ///     KAIKEI_INF.TOTAL_PT_FUTAN
        /// </summary>
        [Column("new_seikyu_gaku")]
        [CustomAttribute.DefaultValue(0)]
        public int NewSeikyuGaku { get; set; }

        /// <summary>
        /// 新請求詳細
        ///     SIN_KOUI.DETAIL_DATA
        /// </summary>
        [Column("new_seikyu_detail")]
        public string? NewSeikyuDetail { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
