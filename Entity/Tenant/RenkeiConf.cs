using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RENKEI_CONF")]
    public class RenkeiConf : EmrCloneable<RenkeiConf>
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
        /// 連携ID
        /// 
        /// </summary>
        [Column("RENKEI_ID", Order = 2)]
        public int RenkeiId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("SEQ_NO", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SeqNo { get; set; }

        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", Order = 4)]
        public long Id { get; set; }

        /// <summary>
        /// パラメーター
        /// 
        /// </summary>
        [Column("PARAM")]
        [MaxLength(1000)]
        public string Param { get; set; } = string.Empty;

        /// <summary>
        /// 患者番号桁数
        /// 0:未指定　10まで設定可能
        /// </summary>
        [Column("PT_NUM_LENGTH")]
        [CustomAttribute.DefaultValue(0)]
        public int PtNumLength { get; set; }

        /// <summary>
        /// テンプレートID
        /// RENKEI_PARAM_TMPL_ID.TEMPLATE_ID
        /// </summary>
        [Column("TEMPLATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int TemplateId { get; set; }

        /// <summary>
        /// 無効区分
        /// 0:有効 1:無効
        /// </summary>
        [Column("IS_INVALID")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        [Column("BIKO")]
        [MaxLength(300)]
        public string Biko { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// 処理順序
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
