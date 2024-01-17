using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// オーダー情報
    /// </summary>
    [Table("odr_inf")]
    [Serializable]
    [Index(nameof(HpId), nameof(PtId), nameof(SinDate), nameof(IsDeleted), Name = "odr_inf_idx01")]

    /// <summary>
    /// create index to speed up performance
    /// </summary>
    [Index(nameof(RaiinNo), nameof(OdrKouiKbn), nameof(InoutKbn), nameof(IsDeleted), Name = "odr_inf_raiin_no_idx")]
    public class OdrInf : EmrCloneable<OdrInf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>

        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        ///     yyyymmdd
        /// </summary>
        [Column("sin_date")]
        public int SinDate { get; set; }

        /// <summary>
        /// 来院番号
        /// </summary>

        [Column("raiin_no", Order = 2)]
        public long RaiinNo { get; set; }

        /// <summary>
        /// 剤番号
        /// </summary>

        [Column("rp_no", Order = 3)]
        [CustomAttribute.DefaultValue(1)]
        public long RpNo { get; set; }

        /// <summary>
        /// 剤枝番
        ///     剤に変更があった場合、カウントアップ
        /// </summary>

        [Column("rp_eda_no", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public long RpEdaNo { get; set; }

        /// <summary>
        /// ID
        /// </summary>

        [Column("id", Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 保険組合せID
        /// </summary>
        [Required]
        [Column("hoken_pid")]
        public int HokenPid { get; set; }

        /// <summary>
        /// オーダー行為区分
        /// </summary>
        [Required]
        [Column("odr_koui_kbn")]
        public int OdrKouiKbn { get; set; }

        /// <summary>
        /// 剤名称
        /// </summary>
        [Column("rp_name")]
        [MaxLength(240)]
        public string? RpName { get; set; } = string.Empty;

        /// <summary>
        /// 院内院外区分
        ///     0: 院内
        ///     1: 院外
        /// </summary>
        [Column("inout_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int InoutKbn { get; set; }

        /// <summary>
        /// 至急区分
        ///     0: 通常 
        ///     1: 至急
        /// </summary>
        [Column("sikyu_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SikyuKbn { get; set; }

        /// <summary>
        /// 処方種別
        ///     0: 日数判断
        ///     1: 臨時
        ///     2: 常態
        /// </summary>
        [Column("syoho_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int SyohoSbt { get; set; }

        /// <summary>
        /// 算定区分
        ///     1: 算定外
        ///     2: 自費算定
        /// </summary>
        [Column("santei_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiKbn { get; set; }

        /// <summary>
        /// 透析区分
        ///     0: 透析以外
        ///     1: 透析前
        ///     2: 透析後
        /// </summary>
        [Column("toseki_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int TosekiKbn { get; set; }

        /// <summary>
        /// 日数回数
        ///     処方日数
        /// </summary>
        [Column("days_cnt")]
        [CustomAttribute.DefaultValue(0)]
        public int DaysCnt { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        ///     2:未確定削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
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
