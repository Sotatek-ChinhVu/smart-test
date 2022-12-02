using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 労働局マスタ
    /// </summary>
    [Table(name: "ROUDOU_MST")]
    public class RoudouMst : EmrCloneable<RoudouMst>
    {
        /// <summary>
        /// 労働局コード
        /// </summary>
        
        [Column(name: "ROUDOU_CD", Order = 1)]
        [MaxLength(2)]
        public string RoudouCd { get; set; } = string.Empty;

        /// <summary>
        /// 労働局名
        /// </summary>
        [Column(name: "ROUDOU_NAME")]
        [MaxLength(60)]
        [Required]
        public string? RoudouName { get; set; } = string.Empty;

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}