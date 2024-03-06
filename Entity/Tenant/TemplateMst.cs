using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Serializable()]
    [Table(name: "template_mst")]
    public class TemplateMst : EmrCloneable<TemplateMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        //[Index("template_mst_pkey", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// テンプレートコード
        /// 
        /// </summary>
        
        [Column("template_cd", Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Index("template_mst_pkey", 2)]
        public int TemplateCd { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Index("template_mst_pkey", 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// 挿入先
        /// </summary>
        [Column("insertion_destination")]
        public int InsertionDestination { get; set; }

        /// <summary>
        /// タイトル
        /// 
        /// </summary>
        [Column("title")]
        [MaxLength(40)]
        public string? Title { get; set; } = string.Empty;

        /// <summary>
        /// 削除フラグ
        /// 
        /// </summary>
        [Column("is_deleted")]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
