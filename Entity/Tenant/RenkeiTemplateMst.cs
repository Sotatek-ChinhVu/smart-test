using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RENKEI_TEMPLATE_MST")]
    public class RenkeiTemplateMst : EmrCloneable<RenkeiTemplateMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// テンプレートID
        /// 1以上の値
        /// </summary>
        //[Key]
        [Column("TEMPLATE_ID", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int TemplateId { get; set; }

        /// <summary>
        /// テンプレート名称
        /// 
        /// </summary>
        [Column("TEMPLATE_NAME")]
        [MaxLength(255)]
        public string TemplateName { get; set; } = string.Empty;

        /// <summary>
        /// パラメーター
        /// パラメータのテンプレート
        /// </summary>
        [Column("PARAM")]
        [MaxLength(1000)]
        public string Param { get; set; } = string.Empty;

        /// <summary>
        /// ファイル名
        /// ファイル名のテンプレート
        /// </summary>
        [Column("FILE")]
        [MaxLength(300)]
        public string File { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// メニュー表示順
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

    }
}
