using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 来院一覧フィルター並び順設定
    /// </summary>
    [Table("RAIIN_FILTER_SORT")]
    [Index(nameof(Id), nameof(HpId), nameof(FilterId), nameof(IsDeleted), Name = "KARTE_INF_IDX01")]
    public class RaiinFilterSort : EmrCloneable<RaiinFilterSort>
    {
        /// <summary>
        /// 連番
        /// </summary>
        [Key]
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// フィルターID
        /// </summary>
        [Column("FILTER_ID")]
        public int FilterId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("SEQ_NO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 優先度
        ///     ソート適用順
        /// </summary>
        [Column("PRIORITY")]
        public int Priority { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        [Column("COLUMN_NAME")]
        public string? ColumnName { get; set; } = string.Empty;

        /// <summary>
        /// 区分コード
        /// </summary>
        [Column("KBN_CD")]
        public int KbnCd { get; set; }

        /// <summary>
        /// 順序
        ///     0:昇順
        ///     1:降順
        /// </summary>
        [Column("SORT_KBN")]
        public int SortKbn { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
