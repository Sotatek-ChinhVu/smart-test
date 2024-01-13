using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "renkei_conf")]
    public class RenkeiConf : EmrCloneable<RenkeiConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 連携ID
        /// 
        /// </summary>
        [Column("renkei_id", Order = 2)]
        public int RenkeiId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("seq_no", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SeqNo { get; set; }

        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", Order = 4)]
        public long Id { get; set; }

        /// <summary>
        /// パラメーター
        /// 
        /// </summary>
        [Column("param")]
        [MaxLength(1000)]
        public string? Param { get; set; } = string.Empty;

        /// <summary>
        /// 患者番号桁数
        /// 0:未指定　10まで設定可能
        /// </summary>
        [Column("pt_num_length")]
        [CustomAttribute.DefaultValue(0)]
        public int PtNumLength { get; set; }

        /// <summary>
        /// テンプレートID
        /// RENKEI_PARAM_TMPL_ID.TEMPLATE_ID
        /// </summary>
        [Column("template_id")]
        [CustomAttribute.DefaultValue(0)]
        public int TemplateId { get; set; }

        /// <summary>
        /// 無効区分
        /// 0:有効 1:無効
        /// </summary>
        [Column("is_invalid")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        [Column("biko")]
        [MaxLength(300)]
        public string? Biko { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// 処理順序
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
