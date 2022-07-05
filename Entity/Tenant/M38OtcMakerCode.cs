using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M38_OTC_MAKER_CODE")]
    public class M38OtcMakerCode : EmrCloneable<M38OtcMakerCode>
    {
        /// <summary>
        /// 会社コード
        /// 数字4桁
        /// </summary>
        [Key]
        [Column("MAKER_CD", Order = 1)]
        public string MakerCd { get; set; }

        /// <summary>
        /// 会社名
        /// 
        /// </summary>
        [Column("MAKER_NAME")]
        [MaxLength(200)]
        public string MakerName { get; set; }

        /// <summary>
        /// 会社名読み
        /// ※半角に変換してDBに登録する
        /// </summary>
        [Column("MAKER_KANA")]
        [MaxLength(400)]
        public string MakerKana { get; set; }

    }
}
