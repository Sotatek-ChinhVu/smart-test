using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M56_DRUG_CLASS")]
    public class M56DrugClass : EmrCloneable<M56DrugClass>
    {
        /// <summary>
        /// 系統コード
        /// 
        /// </summary>
        [Key]
        [Column("CLASS_CD", Order = 1)]
        [MaxLength(8)]
        public string ClassCd { get; set; }

        /// <summary>
        /// 系統名
        /// 
        /// </summary>
        [Column("CLASS_NAME")]
        [MaxLength(200)]
        public string ClassName { get; set; }

        /// <summary>
        /// 系統重複チェック対象フラグ
        /// 
        /// </summary>
        [Column("CLASS_DUPLICATION")]
        [MaxLength(1)]
        public string ClassDuplication { get; set; }

    }
}
