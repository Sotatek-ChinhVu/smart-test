using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 来院一覧フィルター設定
    /// </summary>
    [Table("raiin_filter_mst")]
    [Index(nameof(HpId), nameof(FilterId), nameof(IsDeleted), Name = "raiin_filter_mst_idx01")]
    public class RaiinFilterMst : EmrCloneable<RaiinFilterMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// フィルターID
        /// </summary>
        
        [Column("filter_id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilterId { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// フィルター名称
        /// </summary>
        [Column("filter_name")]
        public string? FilterName { get; set; } = string.Empty;

        /// <summary>
        /// 選択区分
        ///     0:状態に..
        ///     1:属性編集
        ///     2:カルテ作成
        ///     3:窓口精算
        /// </summary>
        [Column("select_kbn")]
        public int SelectKbn { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// ショートカット
        /// </summary>
        [Column("shortcut")]
        [MaxLength(10)]
        public string? Shortcut { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("create_date")]
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
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
