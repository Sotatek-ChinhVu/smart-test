using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "FILING_CATEGORY_MST")]
    public class FilingCategoryMst : EmrCloneable<FilingCategoryMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// カテゴリコード
        /// </summary>
        //[Key]
        [Column("CATEGORY_CD", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryCd { get; set; }

        /// <summary>
        /// カテゴリ名称
        /// </summary>
        [Column("CATEGORY_NAME")]
        [MaxLength(120)]
        public string CategoryName { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 患者属性表示
        ///        1:表示
        /// </summary>
        [Column("DSP_KANZOK")]
        [CustomAttribute.DefaultValue(0)]
        public int DspKanzok { get; set; }

        /// <summary>
        /// ファイル削除
        ///     1:削除
        /// </summary>
        [Column("IS_FILE_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsFileDeleted { get; set; }

        /// <summary>
        /// 削除フラグ
        ///     1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }
    }
}
