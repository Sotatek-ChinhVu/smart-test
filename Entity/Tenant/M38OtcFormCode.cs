using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
   
namespace Entity.Tenant
{
    /// <summary>
    /// OTC剤形コードテーブル
    /// </summary>
    [Table(name: "m38_otc_form_code")]
    public class M38OtcFormCode : EmrCloneable<M38OtcFormCode>
    {
        /// <summary>
        /// 剤形コード
        /// FMで始まり、4桁の数字が続く
        /// </summary>
        
        [Column(name: "form_cd", Order = 1)]
        public string FormCd { get; set; } = string.Empty;

        /// <summary>
        /// 剤形
        /// </summary>
        [Column(name: "form")]
        [MaxLength(80)]
        public string? Form { get; set; } = string.Empty;
    }

}
