using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Serializable()]
    [Table(name: "TEMPLATE_DSP_CONF")]
    public class TemplateDspConf : EmrCloneable<TemplateDspConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// TEMPLATE_CATEGORY.HP_ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        //[Index("TEMPLATE_DSP_CONF_PKEY", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// テンプレートコード
        /// TEMPLATE_CATEGORY.TEMPLATE_CD
        /// </summary>
        //[Key]
        [Column("TEMPLATE_CD", Order = 2)]
        //[Index("TEMPLATE_DSP_CONF_PKEY", 2)]
        public int TemplateCd { get; set; }

        /// <summary>
        /// 連番
        /// TEMPLATE_CATEGORY.SEQ_NO
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 3)]
        //[Index("TEMPLATE_DSP_CONF_PKEY", 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 表示区分
        /// DSP_KBN
        /// 1～999   KARTE_KBN_MST.KARTE_KBN
        /// 1000     サマリー
        /// 1001     問診
        /// 1002     生活歴
        /// </summary>
        //[Key]
        [Column("DSP_KBN", Order = 4)]
        //[Index("TEMPLATE_DSP_CONF_PKEY", 4)]
        public int DspKbn { get; set; }

        /// <summary>
        /// 表示設定
        /// 0: 非表示　1: 表示
        /// </summary>
        [Column("IS_DSP")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDsp { get; set; }

    }
}
