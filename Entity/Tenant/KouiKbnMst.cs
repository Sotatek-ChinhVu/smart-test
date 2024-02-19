using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 行為区分マスタ
    /// </summary>
    [Table(name: "koui_kbn_mst")]
    public class KouiKbnMst
    {
        /// <summary>
        /// 行為区分ID
        /// </summary>

        [Column("koui_kbn_id")]
        public int KouiKbnId { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Required]
        [Column("sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// 行為区分１
        /// </summary>
        [Required]
        [Column("koui_kbn1")]
        public int KouiKbn1 { get; set; }

        /// <summary>
        /// 行為区分２
        /// </summary>
        [Required]
        [Column("koui_kbn2")]
        public int KouiKbn2 { get; set; }

        /// <summary>
        /// 行為グループ名
        /// </summary>
        [Column("koui_grp_name")]
        [MaxLength(20)]
        public string? KouiGrpName { get; set; } = string.Empty;

        /// <summary>
        /// 行為グループ名
        /// </summary>
        [Required]
        [Column("koui_name")]
        [MaxLength(20)]
        public string? KouiName { get; set; } = string.Empty;

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

        [Column("exc_koui_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int ExcKouiKbn { get; set; }

        [Column("oya_koui_kbn_id")]
        [CustomAttribute.DefaultValue(0)]
        public int OyaKouiKbnId { get; set; }
    }
}