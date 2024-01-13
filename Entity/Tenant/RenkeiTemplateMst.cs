using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "renkei_template_mst")]
    public class RenkeiTemplateMst : EmrCloneable<RenkeiTemplateMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// テンプレートID
        /// 1以上の値
        /// </summary>
        
        [Column("template_id", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int TemplateId { get; set; }

        /// <summary>
        /// テンプレート名称
        /// 
        /// </summary>
        [Column("template_name")]
        [MaxLength(255)]
        public string? TemplateName { get; set; } = string.Empty;

        /// <summary>
        /// パラメーター
        /// パラメータのテンプレート
        /// </summary>
        [Column("param")]
        [MaxLength(1000)]
        public string? Param { get; set; } = string.Empty;

        /// <summary>
        /// ファイル名
        /// ファイル名のテンプレート
        /// </summary>
        [Column("file")]
        [MaxLength(300)]
        public string? File { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// メニュー表示順
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }
    }
}
