using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M38_OTC_MAIN")]
    public class M38OtcMain : EmrCloneable<M38OtcMain>
    {
        /// <summary>
        /// シリアルナンバー
        /// 管理用通し番号　1~9999999
        /// </summary>
        [Key]
        [Column("SERIAL_NUM", Order = 1)]
        public int SerialNum { get; set; }

        /// <summary>
        /// ＯＴＣコード
        /// 「O」で始まる、11桁の数字
        /// </summary>
        [Column("OTC_CD")]
        [MaxLength(12)]
        public string OtcCd { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        [Column("TRADE_NAME")]
        [MaxLength(200)]
        public string TradeName { get; set; }

        /// <summary>
        /// 商品名読み
        /// </summary>
        [Column("TRADE_KANA")]
        [MaxLength(400)]
        public string TradeKana { get; set; }

        /// <summary>
        /// 分類コード
        /// 
        /// </summary>
        [Column("CLASS_CD")]
        [MaxLength(2)]
        public string ClassCd { get; set; }

        /// <summary>
        /// 会社コード
        /// </summary>
        [Column("COMPANY_CD")]
        [MaxLength(4)]
        public string CompanyCd { get; set; }

        /// <summary>
        /// 商品コード
        /// </summary>
        [Column("TRADE_CD")]
        [MaxLength(3)]
        public string TradeCd { get; set; }

        /// <summary>
        /// 剤形コード
        /// 
        /// </summary>
        [Column("DRUG_FORM_CD")]
        [MaxLength(6)]
        public string DrugFormCd { get; set; }

        /// <summary>
        /// 用法コード
        /// </summary>
        [Column("YOHO_CD")]
        [MaxLength(6)]
        public string YohoCd { get; set; }
    }
}
