using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 高額療養費限度額
    /// </summary>
    [Table(name: "kogaku_limit")]
    public class KogakuLimit : EmrCloneable<KogakuLimit>
    {
        /// <summary>
        /// 年齢区分
        /// </summary>

        [Column("age_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int AgeKbn { get; set; }
        /// <summary>
        /// 高額療養費区分
        /// </summary>

        [Column("kogaku_kbn")]
        public int KogakuKbn { get; set; }
        /// <summary>
        /// 所得区分
        /// </summary>
        [Column("income_kbn")]
        [MaxLength(20)]
        public string? IncomeKbn { get; set; } = string.Empty;
        /// <summary>
        /// 開始日
        /// </summary>

        [Column("start_date")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }
        /// <summary>
        /// 終了日
        /// </summary>
        [Column("end_date")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }
        /// <summary>
        /// 基準額
        /// </summary>
        [Column("base_limit")]
        public int BaseLimit { get; set; }
        /// <summary>
        /// 調整額
        /// </summary>
        [Column("adjust_limit")]
        public int AdjustLimit { get; set; }
        /// <summary>
        /// 多数該当
        /// </summary>
        [Column("tasu_limit")]
        public int TasuLimit { get; set; }
        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 作成者
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }
        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;
        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 更新者
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }
        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
