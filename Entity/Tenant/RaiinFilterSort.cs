using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 来院一覧フィルター並び順設定
    /// </summary>
    [Table("raiin_filter_sort")]
    [Index(nameof(Id), nameof(HpId), nameof(FilterId), nameof(IsDeleted), Name = "karte_inf_idx01")]
    public class RaiinFilterSort : EmrCloneable<RaiinFilterSort>
    {
        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// フィルターID
        /// </summary>
        [Column("filter_id")]
        public int FilterId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("seq_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 優先度
        ///     ソート適用順
        /// </summary>
        [Column("priority")]
        public int Priority { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        [Column("column_name")]
        public string? ColumnName { get; set; } = string.Empty;

        /// <summary>
        /// 区分コード
        /// </summary>
        [Column("kbn_cd")]
        public int KbnCd { get; set; }

        /// <summary>
        /// 順序
        ///     0:昇順
        ///     1:降順
        /// </summary>
        [Column("sort_kbn")]
        public int SortKbn { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
