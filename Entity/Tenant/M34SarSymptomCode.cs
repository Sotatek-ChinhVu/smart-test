using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M34_SAR_SYMPTOM_CODE")]
    public class M34SarSymptomCode : EmrCloneable<M34SarSymptomCode>
    {
        /// <summary>
        /// 重大な副作用の初期症状コード
        /// 
        /// </summary>
        [Key]
        [Column("FUKUSAYO_INIT_CD", Order = 1)]
        [MaxLength(6)]
        public string FukusayoInitCd { get; set; } = string.Empty;

        /// <summary>
        /// 重大な副作用の初期症状コメント
        /// 
        /// </summary>
        [Column("FUKUSAYO_INIT_CMT")]
        public string FukusayoInitCmt { get; set; } = string.Empty;

    }
}
