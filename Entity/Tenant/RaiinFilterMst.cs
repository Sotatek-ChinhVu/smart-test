using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 来院一覧フィルター設定
    /// </summary>
    [Table("RAIIN_FILTER_MST")]
    [Index(nameof(HpId), nameof(FilterId), nameof(IsDeleted), Name = "RAIIN_FILTER_MST_IDX01")]
    public class RaiinFilterMst : EmrCloneable<RaiinFilterMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// フィルターID
        /// </summary>
        [Key]
        [Column("FILTER_ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilterId { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// フィルター名称
        /// </summary>
        [Column("FILTER_NAME")]
        public string FilterName { get; set; }

        /// <summary>
        /// 選択区分
        ///     0:状態に..
        ///     1:属性編集
        ///     2:カルテ作成
        ///     3:窓口精算
        /// </summary>
        [Column("SELECT_KBN")]
        public int SelectKbn { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// ショートカット
        /// </summary>
        [Column("SHORTCUT")]
        [MaxLength(10)]
        public string Shortcut { get; set; }

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
        public string CreateMachine { get; set; }

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
        public string UpdateMachine { get; set; }
    }
}
