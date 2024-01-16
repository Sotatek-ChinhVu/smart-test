using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Serializable()]
    [Table(name: "template_dsp_conf")]
    public class TemplateDspConf : EmrCloneable<TemplateDspConf>
    {
        /// <summary>
        /// 医療機関識別ID
        /// TEMPLATE_CATEGORY.HP_ID
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        //[Index("template_dsp_conf_pkey", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// テンプレートコード
        /// TEMPLATE_CATEGORY.TEMPLATE_CD
        /// </summary>
        
        [Column("template_cd", Order = 2)]
        //[Index("template_dsp_conf_pkey", 2)]
        public int TemplateCd { get; set; }

        /// <summary>
        /// 連番
        /// TEMPLATE_CATEGORY.SEQ_NO
        /// </summary>
        
        [Column("seq_no", Order = 3)]
        //[Index("template_dsp_conf_pkey", 3)]
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
        
        [Column("dsp_kbn", Order = 4)]
        //[Index("template_dsp_conf_pkey", 4)]
        public int DspKbn { get; set; }

        /// <summary>
        /// 表示設定
        /// 0: 非表示　1: 表示
        /// </summary>
        [Column("is_dsp")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDsp { get; set; }
    }
}
