using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m38_otc_maker_code")]
    public class M38OtcMakerCode : EmrCloneable<M38OtcMakerCode>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 会社コード
        /// 数字4桁
        /// </summary>

        [Column("maker_cd", Order = 1)]
        public string MakerCd { get; set; } = string.Empty;

        /// <summary>
        /// 会社名
        /// 
        /// </summary>
        [Column("maker_name")]
        [MaxLength(200)]
        public string? MakerName { get; set; } = string.Empty;

        /// <summary>
        /// 会社名読み
        /// ※半角に変換してDBに登録する
        /// </summary>
        [Column("maker_kana")]
        [MaxLength(400)]
        public string? MakerKana { get; set; } = string.Empty;

    }
}
