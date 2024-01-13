using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m38_otc_main")]
    public class M38OtcMain : EmrCloneable<M38OtcMain>
    {
        /// <summary>
        /// シリアルナンバー
        /// 管理用通し番号　1~9999999
        /// </summary>
        
        [Column("serial_num", Order = 1)]
        public int SerialNum { get; set; }

        /// <summary>
        /// ＯＴＣコード
        /// 「O」で始まる、11桁の数字
        /// </summary>
        [Column("otc_cd")]
        [MaxLength(12)]
        public string? OtcCd { get; set; } = string.Empty;

        /// <summary>
        /// 商品名
        /// </summary>
        [Column("trade_name")]
        [MaxLength(200)]
        public string? TradeName { get; set; } = string.Empty;

        /// <summary>
        /// 商品名読み
        /// </summary>
        [Column("trade_kana")]
        [MaxLength(400)]
        public string? TradeKana { get; set; } = string.Empty;

        /// <summary>
        /// 分類コード
        /// 
        /// </summary>
        [Column("class_cd")]
        [MaxLength(2)]
        public string? ClassCd { get; set; } = string.Empty;

        /// <summary>
        /// 会社コード
        /// </summary>
        [Column("company_cd")]
        [MaxLength(4)]
        public string? CompanyCd { get; set; } = string.Empty;

        /// <summary>
        /// 商品コード
        /// </summary>
        [Column("trade_cd")]
        [MaxLength(3)]
        public string? TradeCd { get; set; } = string.Empty;

        /// <summary>
        /// 剤形コード
        /// 
        /// </summary>
        [Column("drug_form_cd")]
        [MaxLength(6)]
        public string? DrugFormCd { get; set; } = string.Empty;

        /// <summary>
        /// 用法コード
        /// </summary>
        [Column("yoho_cd")]
        [MaxLength(6)]
        public string? YohoCd { get; set; } = string.Empty;
    }
}
