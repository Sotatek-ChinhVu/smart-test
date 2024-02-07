using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 労働局マスタ
    /// </summary>
    [Table(name: "roudou_mst")]
    public class RoudouMst : EmrCloneable<RoudouMst>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 労働局コード
        /// </summary>

        [Column(name: "roudou_cd", Order = 1)]
        [MaxLength(2)]
        public string RoudouCd { get; set; } = string.Empty;

        /// <summary>
        /// 労働局名
        /// </summary>
        [Column(name: "roudou_name")]
        [MaxLength(60)]
        [Required]
        public string? RoudouName { get; set; } = string.Empty;

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }
    }
}