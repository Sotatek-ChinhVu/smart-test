using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Serializable()]
    [Table(name: "template_menu_detail")]
    public class TemplateMenuDetail : EmrCloneable<TemplateMenuDetail>
    {
        /// <summary>
        /// 医療機関ID
        /// 
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        //[Index("template_menu_detail_pkey", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 選択肢区分
        /// 
        /// </summary>
        
        [Column("menu_kbn", Order = 2)]
        //[Index("template_menu_detail_pkey", 2)]
        public int MenuKbn { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Index("template_menu_detail_pkey", 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 項目名
        /// 
        /// </summary>
        [Column("item_name")]
        [MaxLength(100)]
        public string? ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 値
        /// </summary>
        [Column("val")]
        [CustomAttribute.DefaultValue(0)]
        public double? Val { get; set; }

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
