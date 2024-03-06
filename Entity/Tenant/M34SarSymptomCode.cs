using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m34_sar_symptom_code")]
    public class M34SarSymptomCode : EmrCloneable<M34SarSymptomCode>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 重大な副作用の初期症状コード
        /// 
        /// </summary>

        [Column("fukusayo_init_cd", Order = 1)]
        [MaxLength(6)]
        public string FukusayoInitCd { get; set; } = string.Empty;

        /// <summary>
        /// 重大な副作用の初期症状コメント
        /// 
        /// </summary>
        [Column("fukusayo_init_cmt")]
        public string? FukusayoInitCmt { get; set; } = string.Empty;

    }
}
