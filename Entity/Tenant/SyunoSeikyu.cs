using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SYUNO_SEIKYU")]
    public class SyunoSeikyu : EmrCloneable<SyunoSeikyu>
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
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        
        [Column("SIN_DATE", Order = 3)]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        
        [Column("RAIIN_NO", Order = 4)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 入金区分
        /// 0:未精算 1:一部精算 2:免除 3:精算済
        /// </summary>
        [Column("NYUKIN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int NyukinKbn { get; set; }

        /// <summary>
        /// 請求点数
        /// 診療点数（KAIKEI_INF.TENSU）
        /// </summary>
        [Column("SEIKYU_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuTensu { get; set; }

        /// <summary>
        /// 調整額
        ///     KAIKEI_INF.ADJUST_FUTAN
        /// </summary>
        [Column("ADJUST_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int AdjustFutan { get; set; }

        /// <summary>
        /// 請求額
        /// 請求額 （KAIKEI_INF.TOTAL_PT_FUTAN）
        /// </summary>
        [Column("SEIKYU_GAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuGaku { get; set; }

        /// <summary>
        /// 請求詳細
        /// 診療明細（SIN_KOUI.DETAIL_DATA）
        /// </summary>
        [Column("SEIKYU_DETAIL")]
        public string? SeikyuDetail { get; set; } = string.Empty;

        /// <summary>
        /// 新請求点数
        ///     KAIKEI_INF.TENSU
        /// </summary>
        [Column("NEW_SEIKYU_TENSU")]
        [CustomAttribute.DefaultValue(0)]
        public int NewSeikyuTensu { get; set; }

        /// <summary>
        /// 新調整額
        ///     KAIKEI_INF.ADJUST_FUTAN
        /// </summary>
        [Column("NEW_ADJUST_FUTAN")]
        [CustomAttribute.DefaultValue(0)]
        public int NewAdjustFutan { get; set; }

        /// <summary>
        /// 新請求額
        ///     KAIKEI_INF.TOTAL_PT_FUTAN
        /// </summary>
        [Column("NEW_SEIKYU_GAKU")]
        [CustomAttribute.DefaultValue(0)]
        public int NewSeikyuGaku { get; set; }

        /// <summary>
        /// 新請求詳細
        ///     SIN_KOUI.DETAIL_DATA
        /// </summary>
        [Column("NEW_SEIKYU_DETAIL")]
        public string? NewSeikyuDetail { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
