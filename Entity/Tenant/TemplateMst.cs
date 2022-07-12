using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Serializable()]
    [Table(name: "TEMPLATE_MST")]
    public class TemplateMst : EmrCloneable<TemplateMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        //[Index("TEMPLATE_MST_PKEY", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// テンプレートコード
        /// 
        /// </summary>
        //[Key]
        [Column("TEMPLATE_CD", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Index("TEMPLATE_MST_PKEY", 2)]
        public int TemplateCd { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Index("TEMPLATE_MST_PKEY", 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("SORT_NO")]
        public int SortNo { get; set; }

        /// <summary>
        /// 挿入先
        /// </summary>
        [Column("INSERTION_DESTINATION")]
        public int InsertionDestination { get; set; }

        /// <summary>
        /// タイトル
        /// 
        /// </summary>
        [Column("TITLE")]
        [MaxLength(40)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 削除フラグ
        /// 
        /// </summary>
        [Column("IS_DELETED")]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
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
        public string CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
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
        public string UpdateMachine { get; set; }  = string.Empty;

    }
}
