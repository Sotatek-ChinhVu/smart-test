using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Serializable()]
    [Table(name: "TEMPLATE_MENU_DETAIL")]
    public class TemplateMenuDetail : EmrCloneable<TemplateMenuDetail>
    {
        /// <summary>
        /// 医療機関ID
        /// 
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        //[Index("TEMPLATE_MENU_DETAIL_PKEY", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 選択肢区分
        /// 
        /// </summary>
        //[Key]
        [Column("MENU_KBN", Order = 2)]
        //[Index("TEMPLATE_MENU_DETAIL_PKEY", 2)]
        public int MenuKbn { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Index("TEMPLATE_MENU_DETAIL_PKEY", 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 項目名
        /// 
        /// </summary>
        [Column("ITEM_NAME")]
        [MaxLength(100)]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 値
        /// </summary>
        [Column("VAL")]
        [CustomAttribute.DefaultValue(0)]
        public double? Val { get; set; }

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
