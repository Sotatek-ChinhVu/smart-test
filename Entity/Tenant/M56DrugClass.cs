using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m56_drug_class")]
    public class M56DrugClass : EmrCloneable<M56DrugClass>
    {
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 系統コード
        /// 
        /// </summary>

        [Column("class_cd", Order = 1)]
        [MaxLength(8)]
        public string ClassCd { get; set; } = string.Empty;

        /// <summary>
        /// 系統名
        /// 
        /// </summary>
        [Column("class_name")]
        [MaxLength(200)]
        public string? ClassName { get; set; } = string.Empty;

        /// <summary>
        /// 系統重複チェック対象フラグ
        /// 
        /// </summary>
        [Column("class_duplication")]
        [MaxLength(1)]
        public string? ClassDuplication { get; set; } = string.Empty;
    }
}
