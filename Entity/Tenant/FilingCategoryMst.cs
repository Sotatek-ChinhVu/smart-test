using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "filing_category_mst")]
    public class FilingCategoryMst : EmrCloneable<FilingCategoryMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// カテゴリコード
        /// </summary>
        
        [Column("category_cd", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryCd { get; set; }

        /// <summary>
        /// カテゴリ名称
        /// </summary>
        [Column("category_name")]
        [MaxLength(120)]
        public string? CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// 患者属性表示
        ///        1:表示
        /// </summary>
        [Column("dsp_kanzok")]
        [CustomAttribute.DefaultValue(0)]
        public int DspKanzok { get; set; }

        /// <summary>
        /// ファイル削除
        ///     1:削除
        /// </summary>
        [Column("is_file_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsFileDeleted { get; set; }

        /// <summary>
        /// 削除フラグ
        ///     1:削除
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
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
